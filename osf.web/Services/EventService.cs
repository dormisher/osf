using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ImageResizer;
using osf.web.Data;
using osf.web.Models;

namespace osf.web.Services
{
    internal class EventService
    {
        private readonly OsfDb _db = new OsfDb();

        private const string LatestNewsBucket = "osf.latestevents";
        private const string ServiceUrl = "s3-eu-west-1.amazonaws.com";

		// validation

        internal void ValidateEvent(LatestEventModel m, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(m.Description))
            {
                modelState.AddModelError("Description", "enter a description");
            }

            if (string.IsNullOrEmpty(m.Title))
            {
                modelState.AddModelError("Title", "enter a title");
            }

            DateTime d;
            if (!DateTime.TryParseExact(m.Date, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out d))
            {
                modelState.AddModelError("Date", "enter a valid date");
            }
        }

        internal void ValidateEditEventImage(HttpPostedFileBase file, ModelStateDictionary modelState)
        {
            if (file.ContentLength > 0)
            {
                ValidateImageSize(file, modelState);
            }
        }

		internal void ValidateAddEventImage(HttpPostedFileBase file, ModelStateDictionary modelState)
        {
            if (file == null || file.ContentLength == 0)
            {
                modelState.AddModelError("image", "Please upload an image");
            }
            else
            {
                ValidateImageSize(file, modelState);
            }
        }

        internal void AddEvent(LatestEventModel latestEvent, HttpPostedFileBase image)
        {
            using (Stream imageStream = ResizeImage(image))
            {
                var e = _db.LatestEvents.Add(new LatestEvent
                                                 {
                                                     Title = latestEvent.Title,
                                                     Date = ParseDate(latestEvent.Date),
                                                     Description = latestEvent.Description
                                                 });
                _db.SaveChanges();

                UploadImage(e, imageStream);
            }
        }

        internal List<LatestEventModel> LoadEvents(int n)
        {
            return _db.LatestEvents.OrderByDescending(e => e.Date).Take(n).Select(ToLatestEventModel).ToList();
        }

        internal LatestEventModel Load(int id)
        {
            return ToLatestEventModel(_db.LatestEvents.Find(id));
        }

        internal void Delete(int id)
        {
            DeleteImage(id);

            _db.LatestEvents.Remove(_db.LatestEvents.Find(id));
            _db.SaveChanges();
        }

        internal void Edit(LatestEventModel latestEvent, HttpPostedFileBase image)
        {
            LatestEvent e = _db.LatestEvents.Find(latestEvent.Id);

            if (image.ContentLength > 0)
            {
                DeleteImage(latestEvent.Id);

                using (Stream imageStream = ResizeImage(image))
                {
                    UploadImage(e, imageStream);
                }
            }

            e.Date = ParseDate(latestEvent.Date);
            e.Description = latestEvent.Description;
            e.Title = latestEvent.Title;

            _db.SaveChanges();
        }

		internal PagedEventsModel LoadPagedEvents(int page, int take)
		{
			var events = _db.LatestEvents.OrderByDescending(e => e.Date).Skip((page - 1) * take).Take(take).ToList();
			var count = _db.LatestEvents.Count();
			int totalPages = count % take == 0 ? count / take : count / take + 1;

			if (count == 0) totalPages = 1;

			return new PagedEventsModel
			{
				LatestEvents = events.Select(ToLatestEventModel).ToList(),
				TotalPages = totalPages,
				Page = page
			};
		}

		// private methods

        private LatestEventModel ToLatestEventModel(LatestEvent e)
        {
            return new LatestEventModel
                       {
                           Date = e.Date.ToString("dd/MM/yyyy"),
                           Description = e.Description,
                           Title = e.Title,
                           Id = e.Id
                       };
        }

        private void UploadImage(LatestEvent latestEvent, Stream image)
        {
            try
            {
                using (AmazonS3 client = GetS3Client())
                {
                    string key = latestEvent.Id.ToString() + ".jpg";

                    var putImageRequest = new PutObjectRequest();
                    putImageRequest.WithBucketName(LatestNewsBucket)
                                   .WithKey(key)
                                   .WithInputStream(image);

                    using (S3Response response = client.PutObject(putImageRequest))
                    {
                        WebHeaderCollection headers = response.Headers;
                        foreach (string k in headers.Keys)
                        {
                            Console.WriteLine("Response Header: {0}, Value: {1}", k, headers.Get(k));
                        }
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                }
            }
        }

        private void DeleteImage(int id)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest();

                string key = id.ToString() + ".jpg";

                deleteRequest.WithBucketName(LatestNewsBucket).WithKey(key);

                using (DeleteObjectResponse response = GetS3Client().DeleteObject(deleteRequest))
                {
                    WebHeaderCollection headers = response.Headers;
                    foreach (string k in headers.Keys)
                    {
                        Console.WriteLine("Response Header: {0}, Value: {1}", k, headers.Get(k));
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message);
                }
            }
        }

        private AmazonS3 GetS3Client()
        {
            return AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config { ServiceURL = ServiceUrl });
        }

        private Stream ResizeImage(HttpPostedFileBase file)
        {
            var image = Image.FromStream(file.InputStream);

            if (image.Width == 620)
            {
                return file.InputStream;
            }

            file.InputStream.Seek(0, SeekOrigin.Begin);

            var ms = new MemoryStream();
            ImageBuilder.Current.Build(file, ms, new ResizeSettings("maxwidth=620&minheight300&format=jpg"));

            return ms;
        }

        private void ValidateImageSize(HttpPostedFileBase file, ModelStateDictionary modelState)
        {
            var image = Image.FromStream(file.InputStream, true, true);

            if (image.Width < 620)
            {
                modelState.AddModelError("image", "Image must be at least 620px wide");
            }
            else if (image.Height < 300)
            {
                modelState.AddModelError("image", "Image must be at least 300px high");
            }
        }

		private DateTime ParseDate(string date)
		{
			return DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None);
		}
	}
}
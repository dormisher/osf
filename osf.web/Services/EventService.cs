using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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

        internal void AddEvent(LatestEvent latestEvent, HttpPostedFileBase image)
        {
            using (Stream imageStream = ResizeImage(image))
            {
                _db.LatestEvents.Add(latestEvent);
                _db.SaveChanges();

                UploadImage(latestEvent, imageStream);
            }
        }

        internal List<LatestEvent> LoadEvents(int n)
        {
            return _db.LatestEvents.Take(n).ToList();
        }

        internal PagedEventsModel LoadPagedEvents(int page)
        {
            return _db.LoadPagedEvents(page, 5);
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

        private AmazonS3 GetS3Client()
        {
            return AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config {ServiceURL = ServiceUrl});
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
    }
}
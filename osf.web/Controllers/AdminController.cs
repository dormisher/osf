using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using osf.web.Config;
using osf.web.Data;
using osf.web.Models;

namespace osf.web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //Db _db = new Db();

        public ActionResult Index(int page = 1)
        {
            //return View(new EventAdminModel { Events = _db.LoadPagedEvents(page, 5) });
            return View();
        }

        [HttpPost]
        public ActionResult Index(LatestEvent newEvent)
        {
            if (Request.Files["Image"].ContentLength == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image");
            }

            if (!ModelState.IsValid)
            {
                return View(new EventAdminModel
                                {
                                    Event = newEvent
                                    //Events = _db.LoadPagedEvents(page, 5)
                                });
            }

            UploadImage(newEvent, Request.Files["image"]);

            //_db.LatestEvents.Add(newEvent);
            //_db.SaveChanges();

            return RedirectToAction("index");
        }

        private void UploadImage(LatestEvent latestEvent, HttpPostedFileBase image)
        {
            try
            {
                using (AmazonS3 client = AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config { ServiceURL = "s3-eu-west-1.amazonaws.com" }))
                {
                    var putImageRequest = new PutObjectRequest();
                    putImageRequest.WithMetaData("title", latestEvent.Title)
                                   .WithMetaData("description", latestEvent.Description)
                                   .WithMetaData("date", latestEvent.Date)
                                   .WithBucketName(Literals.LatestNewsBucket)
                                   .WithKey(Guid.NewGuid().ToString())
                                   .WithInputStream(image.InputStream);

                    using (S3Response response = client.PutObject(putImageRequest))
                    {
                        WebHeaderCollection headers = response.Headers;
                        foreach (string key in headers.Keys)
                        {
                            Console.WriteLine("Response Header: {0}, Value: {1}", key, headers.Get(key));
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
    }
}

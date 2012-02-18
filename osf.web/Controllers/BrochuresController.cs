using System.Web.Mvc;
using System.IO;
using System;

namespace osf.web.Controllers
{
    public class BrochuresController : Controller
    {
        public ActionResult Pdf()
        {
            string pdfPath = HttpContext.Server.MapPath("~/content/pdf1.pdf");

            FileStream fileStream = System.IO.File.OpenRead(pdfPath);

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=form.pdf");

            return new FileStreamResult(fileStream, "application/pdf");
        }
    }
}

using System.Web.Mvc;
using System.IO;

namespace osf.web.Controllers
{
    public class BrochuresController : Controller
    {
        public ActionResult Index()
        {
            string pdfPath = HttpContext.Server.MapPath("~/content/OSF Brochures.pdf");

            FileStream fileStream = System.IO.File.OpenRead(pdfPath);

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=OSF Brochure.pdf");

            return new FileStreamResult(fileStream, "application/pdf");
        }
    }
}

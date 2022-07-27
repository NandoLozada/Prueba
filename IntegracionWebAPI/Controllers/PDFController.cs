using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Controllers
{
    public class PDFController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

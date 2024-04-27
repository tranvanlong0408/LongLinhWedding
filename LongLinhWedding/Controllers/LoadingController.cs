using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LongLinhWedding.Controllers
{
    public class LoadingController : Controller
    {
        // GET: LoadingController
        public ActionResult Index()
        {
            return View();
        }
    }
}

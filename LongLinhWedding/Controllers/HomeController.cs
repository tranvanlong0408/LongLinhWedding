using LongLinhWedding.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace LongLinhWedding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetJsonWish()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "wishbox.json");
            List<WishModel> wishes = JsonConvert.DeserializeObject<List<WishModel>>(System.IO.File.ReadAllText(filePath));

            return Json(wishes);
        }

        public IActionResult PutJsonWish([FromForm] WishModel wishModel)
        {
            string statusWriteJsonCode = "";
            //CODE001: OK
            //CODE002: File Json không tồn tại
            //CODE003: Cac phần tử wish bị null
            //CODE004: File Json rỗng
            //CODE005: FullName đã tồn tại

            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "wishbox.json");
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                statusWriteJsonCode = "CODE002";
            }
            else if(String.IsNullOrEmpty(wishModel.FullName) && String.IsNullOrEmpty(wishModel.WishMessage))
            {
                statusWriteJsonCode = "CODE003";
            }
            else 
            {
                List<WishModel> wishes = JsonConvert.DeserializeObject<List<WishModel>>(System.IO.File.ReadAllText(filePath));
                if(wishes == null)
                {
                    statusWriteJsonCode = "CODE004";
                }
                else if(wishes.Any(w => w.FullName == wishModel.FullName))
                {
                    statusWriteJsonCode = "CODE005";
                }
                else
                {
                    wishes.Add(wishModel);
                    System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(wishes));
                    statusWriteJsonCode = "CODE001";
                }
            }
            return Json(statusWriteJsonCode);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
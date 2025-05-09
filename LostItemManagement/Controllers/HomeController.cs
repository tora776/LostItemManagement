using System.Diagnostics;
using LostItemManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostItemManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LostService _service;

        public HomeController(ILogger<HomeController> logger, LostRepository repository)
        {
            _logger = logger;
            _service = new LostService(repository);
        }

        public IActionResult Index()
        {
            // 起動時処理のため、引数に空文字を渡す
            string item = "";
            string place = "";
            string detailedPlace = "";
            // 検索処理を実施
            var items = _service.SelectLostService(item, place, detailedPlace);
            return View(items);
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

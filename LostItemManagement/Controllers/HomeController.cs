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
            // �N���������̂��߁A�����ɋ󕶎���n��
            string item = "";
            string place = "";
            string detailedPlace = "";
            // �������������{
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

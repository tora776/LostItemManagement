using System.Diagnostics;
using LostItemManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        public IActionResult Login()
        {
            return View();
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


        // 削除予定
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 更新ボタン押下時チェックをつけたデータをセッションに保存
        [HttpPost]
        public IActionResult Update([FromBody] List<int> lostIds)
        {
            if (lostIds == null || !lostIds.Any())
            {
                // 選択されたデータがない場合、エラーメッセージを表示
                ModelState.AddModelError("", "少なくとも1つのアイテムを選択してください。");
                return BadRequest(ModelState);
            }

            // lostIdに基づいて紛失物データを取得
            var lostItems = _service.SelectLostServiceByIds(lostIds);

            // TempDataにデータを保存
            TempData["SelectedItems"] = JsonConvert.SerializeObject(lostItems);

            return Ok();
        }

        [HttpGet]
        public IActionResult Update()
        {
            // TempDataから選択されたデータを取得
            var selectedItemsJson = TempData["SelectedItems"] as string;
            var selectedItems = string.IsNullOrEmpty(selectedItemsJson)
                ? new List<Lost>()
                : JsonConvert.DeserializeObject<List<Lost>>(selectedItemsJson);

            // Update.cshtmlにデータを渡す
            return View(selectedItems);
        }
    }
}

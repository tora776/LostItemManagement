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
            // �N���������̂��߁A�����ɋ󕶎���n��
            string item = "";
            string place = "";
            string detailedPlace = "";
            // �������������{
            var items = _service.SelectLostService(item, place, detailedPlace);
            return View(items);
        }


        // �폜�\��
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // �X�V�{�^���������`�F�b�N�������f�[�^���Z�b�V�����ɕۑ�
        [HttpPost]
        public IActionResult Update([FromBody] List<int> lostIds)
        {
            if (lostIds == null || !lostIds.Any())
            {
                // �I�����ꂽ�f�[�^���Ȃ��ꍇ�A�G���[���b�Z�[�W��\��
                ModelState.AddModelError("", "���Ȃ��Ƃ�1�̃A�C�e����I�����Ă��������B");
                return BadRequest(ModelState);
            }

            // lostId�Ɋ�Â��ĕ������f�[�^���擾
            var lostItems = _service.SelectLostServiceByIds(lostIds);

            // TempData�Ƀf�[�^��ۑ�
            TempData["SelectedItems"] = JsonConvert.SerializeObject(lostItems);

            return Ok();
        }

        [HttpGet]
        public IActionResult Update()
        {
            // TempData����I�����ꂽ�f�[�^���擾
            var selectedItemsJson = TempData["SelectedItems"] as string;
            var selectedItems = string.IsNullOrEmpty(selectedItemsJson)
                ? new List<Lost>()
                : JsonConvert.DeserializeObject<List<Lost>>(selectedItemsJson);

            // Update.cshtml�Ƀf�[�^��n��
            return View(selectedItems);
        }
    }
}

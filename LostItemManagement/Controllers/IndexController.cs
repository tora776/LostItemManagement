using Microsoft.AspNetCore.Mvc;
using LostItemManagement.Models;

namespace LostItemManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : Controller
    {

        private readonly LostService _lostService;
        public IndexController(LostService lostService)
        {
            _lostService = lostService;
        }

        [HttpPost("select")]
        public IActionResult Select([FromBody] Lost lost)
        {
            // lostItemがnullの場合、空文字を代入する
            if (lost.lostItem == null)
            {
                lost.lostItem = "";
            }

            // lostPlaceがnullの場合、空文字を代入する
            if (lost.lostPlace == null)
            {
                lost.lostPlace = "";
            }

            // lostDetailedPlaceがnullの場合、空文字を代入する
            if (lost.lostDetailedPlace == null)
            {
                lost.lostDetailedPlace = "";
            }
            // 検索処理を実施
            var items = _lostService.SelectLostService(lost.lostItem, lost.lostPlace, lost.lostDetailedPlace);
            // return View(items);
            return Json(items);
        }

        [HttpPost("insert")]
        public IActionResult Insert([FromBody] Lost lost)
        {
            _lostService.InsertLostService(lost);   

            // 起動時処理のため、引数に空文字を渡す
            string item = "";
            string place = "";
            string detailedPlace = "";

            // 検索処理を実施
            var items = _lostService.SelectLostService(item, place, detailedPlace);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] Lost lost)
        {
            _lostService.UpdateLostService(lost);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _lostService.DeleteLostService(id);
            return Ok();
        }
    }
}

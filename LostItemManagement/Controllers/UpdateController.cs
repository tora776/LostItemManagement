using LostItemManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostItemManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        private readonly ILogger<UpdateController> _logger;
        private readonly LostService _service;

        public UpdateController(ILogger<UpdateController> logger, LostRepository repository)
        {
            _logger = logger;
            _service = new LostService(repository);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("saveupdates")]
        public IActionResult SaveUpdates([FromBody] List<Lost> updatedItems)
        {
            foreach (var item in updatedItems)
            {
                _service.UpdateLostService(item);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using UFSBankingSystemWebsite.Models;
using System.Threading.Tasks;
using UFSBankingSystemWebsite.Data.Interfaces;

namespace UFSBankingSystemWebsite.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public NotificationController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _repository.Notification.FindAllAsync();
            return View(notifications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification = await _repository.Notification.FindByIdAsync(id);
            if (notification == null)
                return NotFound();

            return View(notification);
        }
    }
}
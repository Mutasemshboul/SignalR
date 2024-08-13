using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Project_Orange.Models;
using SignalR.Data;
using SignalR.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;



        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> Chat()
        {

            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(currentUserId))
            {
                // Log or handle the error
                throw new Exception("Current user ID is null or empty.");
            }

            var users = _userManager.Users.Where(u => u.Id != currentUserId).ToList();
            var messages = await _context.ChatMessages
           .Include(m => m.Sender)
           .Include(m => m.Receiver)
           .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
           .OrderBy(m => m.Timestamp)
           .ToListAsync();

            var viewModel = new ChatViewModel
            {
                Users = users,
                Messages = messages,
                CurrentUserId = currentUserId
            };
            ViewBag.CurrentUserId = currentUserId;
            return View(viewModel);
        }

        public async Task<IActionResult> GetChatWithUser(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);

            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                            (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
            ViewBag.CurrentUserId = currentUserId;
            return PartialView("_ChatHistory", messages);
        }
    }
}

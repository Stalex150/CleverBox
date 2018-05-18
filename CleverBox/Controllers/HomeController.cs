using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CleverBox.Models;
using CleverBox.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using CleverBox.Data;
using Microsoft.AspNetCore.Identity;

namespace CleverBox.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _contex;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _contex = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            int learnedFacts = _contex
                .Facts
                .Where(f => f.RepetitionLevel != 0)
                .Where(f => f.User == user)
                .Count();

            int allFacts = _contex
                .Facts
                .Where(f => f.User == user)
                .Count();

            int factsInMemory = _contex
                .Facts
                .Where(f => f.RepetitionLevel == 10)
                .Where(f => f.User == user)
                .Count();

            int userPoints = user.Points;

            var homeModel = new HomeViewModel
            {
                UserPoints = userPoints,
                FactInMemory = factsInMemory,
                LearnedFacts = learnedFacts,
                AllFacts = allFacts
            };

            return View(homeModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

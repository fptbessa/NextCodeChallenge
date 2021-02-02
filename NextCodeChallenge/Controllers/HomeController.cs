using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextCodeChallenge.Models;
using NextCodeChallenge.Models.Entities;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NextCodeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly NextCodeChallengeDbContext _context;

        public HomeController(NextCodeChallengeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nextCodeChallengeDbContext = _context.Topics.Include(t => t.CreatorUser);
            return View(await nextCodeChallengeDbContext.ToListAsync());
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

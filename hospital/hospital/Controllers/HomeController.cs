using hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HospitalContext _context;

        public HomeController(ILogger<HomeController> logger, HospitalContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View( await _context.Clinics.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Doctors(int id)
        {
            var coreContext = _context.Doctors.Where(x => x.ClincId == id).Include(d => d.Clinc).ToList();

            return View(coreContext);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
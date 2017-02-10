using System;
using System.Threading.Tasks;
using ILoveAudi.Core;
using Microsoft.AspNetCore.Mvc;
using ILoveAudi.Models;
using ILoveAudi.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ILoveAudi.Controllers
{
    public class HomeController : Controller
    {
        private IHtmlParser _parser;

        public HomeController(IHtmlParser parser)
        {
            _parser = parser;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _parser.SaveCarToDb();
            return View(result);
        }

    }
}

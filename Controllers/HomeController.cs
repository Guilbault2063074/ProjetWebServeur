using Microsoft.AspNetCore.Mvc;
using Projet_Web_Serveur.Models;
using System.Diagnostics;

namespace Projet_Web_Serveur.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjetWsContext context;

        public HomeController(ProjetWsContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var quizzez = context.Quizzes.ToList();
            return View(quizzez);
        }
    }
}
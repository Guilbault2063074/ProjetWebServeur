using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;
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

        public IActionResult NewQuiz()
        {
            return View();
        }

        public IActionResult AddQuiz(string choix1, string choix2, string choix3)
        {
            var totalquiz = context.Totalquizzes.FirstOrDefault();
            int quizId = (int)totalquiz.totalquiz;

            int nbQuestionFacile = Convert.ToInt32(choix1);
            int nbQuestionsMedium = Convert.ToInt32(choix2);
            int nbQuestionDifficile = Convert.ToInt32(choix3);

            List<Question> easyQuestions = new List<Question>();
            List<Question> mediumQuestions = new List<Question>();
            List<Question> hardQuestions = new List<Question>();

            List<Question> allEasyQuestionList = context.Questions.Where(q => q.QuestionDifficulty == 1).ToList();
            List<Question> allMediumQuestionList = context.Questions.Where(q => q.QuestionDifficulty ==2).ToList();
            List<Question> allHardQuestionList = context.Questions.Where(q => q.QuestionDifficulty == 3).ToList();

            Random random = new Random();


            for (int i = 0;i<nbQuestionFacile;i++)
            {
                int randomIndex = random.Next(0, allEasyQuestionList.Count);
                easyQuestions.Add(allEasyQuestionList[randomIndex]);
                allEasyQuestionList.RemoveAt(randomIndex);
            }

            for (int i = 0; i < nbQuestionsMedium; i++)
            {
                int randomIndex = random.Next(0, allMediumQuestionList.Count);
                mediumQuestions.Add(allMediumQuestionList[randomIndex]);
                allMediumQuestionList.RemoveAt(randomIndex);
            }

            for (int i = 0; i < nbQuestionDifficile; i++)
            {
                int randomIndex = random.Next(0, allHardQuestionList.Count);
                hardQuestions.Add(allHardQuestionList[randomIndex]);
                allHardQuestionList.RemoveAt(randomIndex);
            }

            Quiz quiz = new Quiz
            {
                QuizId = quizId,
                MediumQuestionCount = nbQuestionsMedium,
                HardQuestionCount = nbQuestionDifficile,
                EasyQuestionCount = nbQuestionFacile

            };
            
            foreach(var q in easyQuestions)
            {
                quiz.Quizquestions.Add(new Quizquestion { QuizId=quizId, QuestionId=q.QuestionId });
            }

            foreach (var q in mediumQuestions)
            {
                quiz.Quizquestions.Add(new Quizquestion { QuizId = quizId, QuestionId = q.QuestionId });
            }

            foreach (var q in hardQuestions)
            {
                quiz.Quizquestions.Add(new Quizquestion { QuizId = quizId, QuestionId = q.QuestionId });
            }

            

            context.Quizzes.Add(quiz);

            totalquiz.totalquiz = (uint)(quizId + 1);
            context.Update<Totalquiz>(totalquiz);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult SeeQuiz(int quizId)
        {
            var questionQuiz = context.Quizquestions.Where(e => e.QuizId == quizId);
            ViewBag.currentQuizId = questionQuiz.FirstOrDefault().QuizId;
            return View(questionQuiz);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Projet_Web_Serveur.Models;
using System.Diagnostics;
using System.Transactions;

namespace Projet_Web_Serveur.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjetWsContext context;
        private readonly ISession session;

        public HomeController(ProjetWsContext _context, IHttpContextAccessor httpContextAccessor)
        {
            context = _context;
            session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Email == email);

            if (user != null)
            {
                ViewBag.Fail = false;
                session.SetString("UserEmail", user.Email);
                return RedirectToAction("StartScreen", "Home");
            }
            ViewBag.Fail = true;
            return View("Login");
        }

        public IActionResult StartScreen()
        {
            return View();
        }

        public IActionResult SeeQuizzes()
        {
            var userEmail = session.GetString("UserEmail");
            var quizzez = context.Quizzes.Where(q => q.EmailNavigation.Email == userEmail).ToList();
            return View(quizzez);
        }

        public IActionResult NewQuiz()
        {
            return View();
        }

        public IActionResult AddQuiz(string choix1, string choix2, string choix3)
        {
            var totalquiz = context.Totalquizzes.FirstOrDefault();
            int quizId = (int)totalquiz.Totalquiz1;
            var userEmail = session.GetString("UserEmail");

            int nbQuestionFacile = Convert.ToInt32(choix1);
            int nbQuestionsMedium = Convert.ToInt32(choix2);
            int nbQuestionDifficile = Convert.ToInt32(choix3);

            List<Question> easyQuestions = new List<Question>();
            List<Question> mediumQuestions = new List<Question>();
            List<Question> hardQuestions = new List<Question>();

            List<Question> allEasyQuestionList = context.Questions.Where(q => q.QuestionDifficulty == 1).ToList();
            List<Question> allMediumQuestionList = context.Questions.Where(q => q.QuestionDifficulty == 2).ToList();
            List<Question> allHardQuestionList = context.Questions.Where(q => q.QuestionDifficulty == 3).ToList();

            Random random = new Random();


            for (int i = 0; i < nbQuestionFacile; i++)
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
                EasyQuestionCount = nbQuestionFacile,
                Email = userEmail
            };

            foreach (var q in easyQuestions)
            {
                quiz.Quizquestions.Add(new Quizquestion { QuizId = quizId, QuestionId = q.QuestionId });
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

            totalquiz.Totalquiz1 = (uint)(quizId + 1);
            context.Update<Totalquiz>(totalquiz);
            context.SaveChanges();

            return RedirectToAction("SeeQuizzes");
        }

        public IActionResult PlayQuiz(int quizId)
        {

            var quiz = context.Quizzes.Find(quizId);
            session.SetInt32("currentQuiz", quizId);

            var submitQuiz = new SubmitQuizView
            {
                currentQuiz = quiz,
            };

            return View(submitQuiz);
        }

        [HttpPost]
        public IActionResult SubmitAnswer(SubmitQuizView submitQuiz)
        {
            var email = session.GetString("UserEmail");
            var currentQuiz = session.GetInt32("currentQuiz");
            int score = 0;
            int total = 0;
            foreach( var userAnswer in submitQuiz.UserAnswers.Values)
            {
                var reponse = context.Choixdereponses.Where(c => c.ChoixId == userAnswer.ChoixId && c.QuestionId == userAnswer.QuestionId).FirstOrDefault();
                if(reponse != null)
                {
                    if (reponse.IsCorrectAnswer ?? false)
                    {
                        score++;
                        total++;
                    }
                    else
                    {
                        total++;
                    }
                }
            }

            var serializedAnswers = JsonConvert.SerializeObject(submitQuiz.UserAnswers);
            var quizResult = new Previousattempt
            {
                Email = email,
                QuizId = currentQuiz,
                AnswerSheet = serializedAnswers,
                Score = score,
                Total = total
            };

            var attemptAlreadyDone = context.Previousattempts.Where(p => p.Email == email && p.QuizId == currentQuiz).FirstOrDefault();

            if (attemptAlreadyDone != null)
            {
                attemptAlreadyDone.AnswerSheet = quizResult.AnswerSheet;
                attemptAlreadyDone.Score = quizResult.Score;
                attemptAlreadyDone.Total = quizResult.Total;

                context.Update(attemptAlreadyDone);
            }
            else
            {
                context.Add(quizResult);
            }

            context.SaveChanges();
            // Get the answers -done
            // Check answers by getting userchoixid.iscorrect dans un foreach -done
            /*foreach (var answer in answers)
            {
                var reponse = context.Choixdereponses.Where(c => c.ChoixId == answer.ChoixId).FirstOrDefault();
                //everytime answers is correct var result+1 et puet importe si bon ou non total+1
                if(reponse != null)
                {
                    if (reponse.IsCorrectAnswer == true)
                    {
                        score++;
                        total++;
                    }
                    else
                    {
                        total++;
                    }
                }
                
            }

            var serializedAnswerSheet = JsonConvert.SerializeObject(answers);
            //create a new answersheet object
            //serialize answersheet
            //sent email,quizID,score, answersheet to database
            var quizResult = new Previousattempt
            {
                Email = email,
                QuizId = currentQuiz,
                AnswerSheet = serializedAnswerSheet,
                Score = score,
                Total = total
            };

            var attemptAlreadyDone = context.Previousattempts.Where(p => p.Email == email && p.QuizId == currentQuiz).FirstOrDefault();
            
            if(attemptAlreadyDone != null )
            {
                attemptAlreadyDone.AnswerSheet = quizResult.AnswerSheet;
                attemptAlreadyDone.Score = quizResult.Score;
                attemptAlreadyDone.Total = quizResult.Total;

                context.Update(attemptAlreadyDone);
            }
            else
            {
                context.Add(quizResult);
            }

            context.SaveChanges();*/

            return RedirectToAction("SeeQuizzes");
        }
    }
}
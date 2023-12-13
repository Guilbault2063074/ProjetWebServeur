namespace Projet_Web_Serveur.Models
{
    public class SubmitQuizView
    {
        public Quiz currentQuiz { get; set; }
        public Dictionary<int, UserAnswer> UserAnswers { get; set; }
    }
}

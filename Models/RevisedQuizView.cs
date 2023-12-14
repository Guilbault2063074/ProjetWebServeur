namespace Projet_Web_Serveur.Models
{
    public class RevisedQuizView
    {
        public Quiz currentRevisedQuiz { get; set; }
        public List<int> userAnswerId { get; set; }
        public int? score { get; set; }
        public int? total { get; set; }


    }
}

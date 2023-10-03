namespace Projet_Web_Serveur.Models
{
    public class QuizView
    {
        public IEnumerable<Quiz> Quizzes { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Choixdereponse> Choix { get; set; }
    }
}

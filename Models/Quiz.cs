using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public int? EasyQuestionCount { get; set; }

    public int? MediumQuestionCount { get; set; }

    public int? HardQuestionCount { get; set; }

    public virtual ICollection<Quizquestion> Quizquestions { get; set; } = new List<Quizquestion>();
}

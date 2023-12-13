using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public int? EasyQuestionCount { get; set; }

    public int? MediumQuestionCount { get; set; }

    public int? HardQuestionCount { get; set; }

    public string? Email { get; set; }

    public virtual User? EmailNavigation { get; set; }

    public virtual ICollection<Previousattempt> Previousattempts { get; set; } = new List<Previousattempt>();

    public virtual ICollection<Quizquestion> Quizquestions { get; set; } = new List<Quizquestion>();
}

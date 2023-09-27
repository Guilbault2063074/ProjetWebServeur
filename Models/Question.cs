using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string? Question1 { get; set; }

    public int? QuestionDifficulty { get; set; }

    public virtual ICollection<Choixdereponse> Choixdereponses { get; set; } = new List<Choixdereponse>();

    public virtual ICollection<Quizquestion> Quizquestions { get; set; } = new List<Quizquestion>();
}

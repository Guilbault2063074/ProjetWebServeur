using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Quizquestion
{
    public ulong MyRowId { get; set; }

    public int? QuizId { get; set; }

    public int? QuestionId { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Quiz? Quiz { get; set; }
}

using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Previousattempt
{
    public ulong MyRowId { get; set; }

    public string? Email { get; set; }

    public int? QuizId { get; set; }

    public string? AnswerSheet { get; set; }

    public int? Score { get; set; }

    public int? Total { get; set; }

    public virtual Quiz? Quiz { get; set; }
}

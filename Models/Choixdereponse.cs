using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class Choixdereponse
{
    public int ChoixId { get; set; }

    public int? QuestionId { get; set; }

    public string? Choix { get; set; }

    public bool? IsCorrectAnswer { get; set; }

    public virtual Question? Question { get; set; }
}

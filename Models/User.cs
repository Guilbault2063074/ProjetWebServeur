using System;
using System.Collections.Generic;

namespace Projet_Web_Serveur.Models;

public partial class User
{
    public string Email { get; set; } = null!;

    public string? Username { get; set; }
}

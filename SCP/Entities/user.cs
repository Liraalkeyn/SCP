using System;
using System.Collections.Generic;

namespace SCP.Entities;

public partial class user
{
    public int userID { get; set; }

    public string login { get; set; } = null!;

    public string password { get; set; } = null!;
}

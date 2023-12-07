using System;
using System.Collections.Generic;

namespace SCP.Entities;

public partial class personnel
{
    public int personnelID { get; set; }

    public string fullName { get; set; } = null!;

    public string occupationName { get; set; } = null!;
}

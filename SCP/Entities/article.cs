using System;
using System.Collections.Generic;

namespace SCP.Entities;

public partial class article
{
    public int articleID { get; set; }

    public string shortDescription { get; set; } = null!;

    public string text { get; set; } = null!;

    public string header { get; set; } = null!;

    public string? imageUrl { get; set; }
}

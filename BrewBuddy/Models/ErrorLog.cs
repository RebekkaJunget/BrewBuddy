using System;
using System.Collections.Generic;

namespace BrewBuddy.Models;

public partial class ErrorLog
{
    public int LogId { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? ErrorDateTime { get; set; }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Cwiczenia10.Models
{
    public partial class Subjects
    {
        public int IdSubject { get; set; }
        public string Name { get; set; }
        public int? TotalPoints { get; set; }
    }
}

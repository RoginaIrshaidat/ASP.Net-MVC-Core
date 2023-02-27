using System;
using System.Collections.Generic;

namespace hospital.Models;

public partial class Clinic
{
    public int ClincId { get; set; }

    public string? ClincName { get; set; }

    public virtual ICollection<Doctor> Doctors { get; } = new List<Doctor>();
}

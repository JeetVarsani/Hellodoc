using System;
using System.Collections;
using System.Collections.Generic;

namespace HelloDoc2.Models;

public partial class ShiftDetail
{
    public int ShiftDetailId { get; set; }

    public int ShiftId { get; set; }

    public DateTime ShiftDate { get; set; }

    public int? RegionId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public short Status { get; set; }

    public BitArray IsDeleted { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public DateTime? LastRunningDate { get; set; }

    public string? EventId { get; set; }

    public BitArray? IsSync { get; set; }

    public int? Modifiedby { get; set; }

    public virtual AspNetUser? ModifiedbyNavigation { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual ICollection<ShiftDetailRegion> ShiftDetailRegions { get; set; } = new List<ShiftDetailRegion>();
}

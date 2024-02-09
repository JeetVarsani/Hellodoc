using System;
using System.Collections.Generic;

namespace HelloDoc2.Models;

public partial class AspNetUserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}

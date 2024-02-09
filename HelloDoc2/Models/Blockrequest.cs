using System;
using System.Collections;
using System.Collections.Generic;

namespace HelloDoc2.Models;

public partial class BlockRequest
{
    public int BlockRequestId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public BitArray? IsActive { get; set; }

    public string? Reason { get; set; }

    public string RequestId { get; set; } = null!;

    public string? Ip { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public DateOnly? ModifiedDate { get; set; }
}

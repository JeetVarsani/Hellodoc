﻿using System;
using System.Collections.Generic;

namespace HelloDoc2.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? VendorId { get; set; }

    public int? RequestId { get; set; }

    public string? FaxNumber { get; set; }

    public string? Email { get; set; }

    public string? BusinessContact { get; set; }

    public string? Prescription { get; set; }

    public int? NoOfRefill { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }
}

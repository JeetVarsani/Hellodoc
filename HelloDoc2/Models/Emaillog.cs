using System;
using System.Collections;
using System.Collections.Generic;

namespace HelloDoc2.Models;

public partial class EmailLog
{
    public decimal EmailLogId { get; set; }

    public string EmailTemplate { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string? ConfirmationNumber { get; set; }

    public string? FilePath { get; set; }

    public int? RoleId { get; set; }

    public int? RequestId { get; set; }

    public int? AdminId { get; set; }

    public int? PhysicianId { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly? SentDate { get; set; }

    public BitArray? IsEmailSent { get; set; }

    public int? SentTries { get; set; }

    public int? Action { get; set; }
}

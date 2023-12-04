using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class PaymentDetail
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentType { get; set; } = null!;

    public string? Provider { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}

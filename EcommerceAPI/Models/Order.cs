using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<PaymentDetail> Paymentdetails { get; } = new List<PaymentDetail>();

    public virtual User User { get; set; } = null!;
}

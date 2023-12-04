using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}

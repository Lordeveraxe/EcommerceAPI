using System;
using System.Collections.Generic;

namespace EcommerceAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();
}

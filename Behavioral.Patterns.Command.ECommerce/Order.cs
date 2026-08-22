using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.Command.ECommerce;

public class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; }
}

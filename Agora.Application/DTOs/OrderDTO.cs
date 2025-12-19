using Agora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agora.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? TotalAmount { get; set; }
        public int? Discount { get; set; }
        public int? Tax { get; set; }
        public int? ShippingFee { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public int? OrderStatus { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class OrderDetailDTO
    {
        public OrderDTO? Order { get; set; }
        public List<OrderItemDTO>? OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
        public int? Discount { get; set; }
        public int? Total { get; set; }
    }

}

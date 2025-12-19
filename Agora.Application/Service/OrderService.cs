using System;
using System.Linq;
using System.Threading.Tasks;
using Agora.Application.DTOs;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Agora.Application.Common;

namespace Agora.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly AgoraDbContext _context;

        public OrderService(AgoraDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request)
        {
            // 1. Get Cart and CartItems
            var cart = await _context.Carts
                .Include(c => c.CartItems!)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty or not found");
            }

            // 2. Calculate Total Amount
            int totalAmount = 0;
            foreach (var item in cart.CartItems)
            {
                if (item.Product != null && item.Product.RetailPrice.HasValue && item.Quantity.HasValue)
                {
                    totalAmount += item.Product.RetailPrice.Value * item.Quantity.Value;
                }
            }

            // 3. Create Order
            var order = new Order
            {
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                PaymentStatus = "Pending", // Waiting for payment
                OrderStatus = 0, // 0: Pending
                // ShippingAddress is not in Order entity based on previous read, let's check Order entity again or just ignore if not present.
                // Checking Order entity again...
                Note = request.Note,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Note: Order entity definition read previously:
            // public int? TotalAmount { get; set; }
            // public string? PaymentStatus { get; set; }
            // public int? OrderStatus { get; set; }
            // public string? Note { get; set; }
            // It does NOT have ShippingAddress. I will ignore ShippingAddress for now or put it in Note.
            if (!string.IsNullOrEmpty(request.ShippingAddress))
            {
                order.Note = $"Address: {request.ShippingAddress}. " + order.Note;
            }

            _context.Orders.Add(order);
            
            // 4. Create OrderItems
            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Product != null)
                {
                    var orderItem = new OrderItem
                    {
                        Order = order,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Product.RetailPrice,
                        Total = (cartItem.Product.RetailPrice ?? 0) * (cartItem.Quantity ?? 0)
                    };
                    _context.OrderItems.Add(orderItem);
                }
            }

            // 5. Clear Cart
            _context.CartItems.RemoveRange(cart.CartItems);

            // 6. Save Changes
            await _context.SaveChangesAsync();

            return new CheckoutResponse
            {
                OrderId = order.Id,
                TotalAmount = totalAmount,
                Message = "Order created successfully. Please proceed to payment."
            };
        }

        public async Task<PagedResult<OrderDTO>> GetOrders(int userId, PagedRequest req)
        {
            try
            {
                var query = _context.Orders
                    .Where(o => o.UserId == userId);
    
                if (!string.IsNullOrEmpty(req.Search))
                {
                    query = query.Where(o => o.Id.ToString().Contains(req.Search));
                }
                var total = await query.CountAsync();
    
                var items = await query
                    .Skip((req.Page - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToListAsync();
    
                var orderDtos = items.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Discount = o.Discount,
                    Tax = o.Tax,
                    ShippingFee = o.ShippingFee,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    OrderStatus = o.OrderStatus,
                    Note = o.Note,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                }).ToList();
    
                return new PagedResult<OrderDTO>
                {
                    Items = orderDtos,
                    Total = total
                };
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public async Task<OrderDetailDTO> GetOrderDetail(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == orderId);
    
                if (order == null)
                {
                    throw new Exception("Order not found");
                }
    
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == orderId)
                    .ToListAsync();
    
                var orderDto = new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Discount = order.Discount,
                    Tax = order.Tax,
                    ShippingFee = order.ShippingFee,
                    PaymentMethod = order.PaymentMethod,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    Note = order.Note,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                };
    
                var orderItemDtos = orderItems.Select(oi => new OrderItemDTO
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount,
                    Total = oi.Total
                }).ToList();
    
                return new OrderDetailDTO
                {
                    Order = orderDto,
                    OrderItems = orderItemDtos
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using System.Threading.Tasks;
using Agora.Application.DTOs;
using Agora.Application.Common;

namespace Agora.Application.Service
{
    public interface IOrderService
    {
        Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request);
       Task<PagedResult<OrderDTO>> GetOrders(int userId, PagedRequest req);
       Task<OrderDetailDTO> GetOrderDetail(int orderId);
    }
}


using System.Threading.Tasks;
using Agora.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Agora.Application.Service;

public interface IImageService
{
    Task<int> Create(IFormFile file);
    Task Delete(int id);
    Task<int?> UpdateUserImage(int userId, IFormFile file);
    Task<int?> UpdateShopImage(int shopId, IFormFile file);
    Task<int?> UpdateProductImage(int productId, IFormFile file);
    Task<ImageDTO?> GetById(int id, bool? ReSize = false, bool? isSmall = null, int? width = null, int? height = null);
}

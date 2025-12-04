using Agora.Application.Common;
using Agora.Application.DTOs;
using Agora.Domain.Entities;

namespace Agora.Application.Service;

public interface IUserService
{
    Task<PagedResult<User>> GetPaged(PagedRequest req);
    Task<User?> GetById(int id);
    Task<User> Create(User user);
    Task Update(User user);
    Task<User?> UpdateSelf(int userId, UserUpdateRequest req);
    Task Delete(int id);
    Task<LoginResponse> Login(LoginRequest req);
    Task UpdateRole(int userId, int newRoleId);
}

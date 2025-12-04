using Agora.Application.Common;
using Agora.Application.Service;
using Agora.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize(Roles = "1, 2")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<PagedResult<Category>> GetPaged([FromQuery] PagedRequest req)
        => _service.GetPaged(req);

    [HttpGet("{id}")]
    public Task<Category?> GetById(int id)
        => _service.GetById(id);

    [HttpPost]
    [Authorize(Roles = "1")]
    public Task<Category> Create(Category category)
        => _service.Create(category);

    [HttpPut("{id}")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        category.Id = id;
        await _service.Update(category);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "1")]
    public Task Delete(int id)
        => _service.Delete(id);
}

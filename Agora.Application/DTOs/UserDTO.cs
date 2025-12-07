namespace Agora.Application.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? TaxCode { get; set; }
    public int? Role { get; set; }
    public int? ImageId { get; set; }
    public DateTime? CreatedAt { get; set; }
}


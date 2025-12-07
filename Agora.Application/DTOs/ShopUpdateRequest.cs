namespace Agora.Application.DTOs;

public class ShopUpdateRequest
{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxCode { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }
        public int? ImageId { get; set; }
        public DateTime? CreatedAt { get; set; }
}
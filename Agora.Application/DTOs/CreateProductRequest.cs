namespace Agora.Application.DTOs;

public class CreateProductRequest
{
    public string? Name { get; set; }
    public int? Barcode { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? ShopId { get; set; }
    public int? CostPrice { get; set; }
    public int? RetailPrice { get; set; }
    public int? StockQty { get; set; }
    public int? DiscountPercent { get; set; }
    public int? GuaranteeMonths { get; set; }
    public string? Note { get; set; }
}

namespace DB.Models;

public partial class Purchase
{
    public Guid PurchaseId { get; set; }

    public Guid? ProductId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public decimal QuantityTotal { get; set; }

    public decimal PriceTotal { get; set; }

    public virtual ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();

    public virtual Product? Product { get; set; }
}

namespace DB.Models;

public partial class MealItem
{
    public Guid MealId { get; set; }

    public Guid PurchaseId { get; set; }

    public decimal QuantityUsed { get; set; }

    public virtual Meal Meal { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}

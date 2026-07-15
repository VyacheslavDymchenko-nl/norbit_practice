namespace DB.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? UnitId { get; set; }

    public string ProductName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Unit? Unit { get; set; }
}

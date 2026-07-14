using System;
using System.Collections.Generic;

namespace DB.Models;

public partial class Meal
{
    public Guid MealId { get; set; }

    public string? MealName { get; set; }

    public DateTime MealDate { get; set; }

    public virtual ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();
}

using System.ComponentModel.DataAnnotations;

namespace wttproductlist;

public class Category
{
    public int CategoryID { get; set; }

    [Display(Name = "Name")]
    public string? CategoryName { get; set; }
    
    [Display(Name = "Product Description")]
    public string? Description { get; set; }
}

//Products table
//ProductID int
//ProductName nvarchar(100)
//Description nvarchar(max)
//ImagePath nvarchar(max)
//UnitPrice float
//CategoryID int
namespace wttproductlist;

public class Products
{
    public int ProductID { get; set; }

    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public double? UnitPrice { get; set; }
    public int? CategoryID { get; set; }
    public Category? Category { get; set; }
}

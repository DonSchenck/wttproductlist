using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;

namespace wttproductlist.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductListController : ControllerBase
{
    const int COLUMN_ProductID = 0;
    const int COLUMN_ProductName = 1;
    const int COLUMN_Description = 2;
    const int COLUMN_ImagePath = 3;
    const int COLUMN_UnitPrice = 4;
    const int COLUMN_CategoryID = 5;
    const int COLUMN_CategoryName = 7;
    const int COLUMN_CategoryDescription = 8;

    private readonly ILogger<ProductListController> _logger;

    public ProductListController(ILogger<ProductListController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetProductList")]
    [EnableQuery]
    public IEnumerable<Products> Get()
    {

        var l = new List<Products> { };
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            string db_server = Environment.GetEnvironmentVariable("DB_DATASOURCE");
            string db_userid = Environment.GetEnvironmentVariable("DB_USERID");
            string db_pwd = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string db_database = Environment.GetEnvironmentVariable("DB_INITIALCATALOG");
            
            builder.DataSource = db_server;
            builder.UserID = db_userid;
            builder.Password = db_pwd;
            builder.InitialCatalog = db_database;

            // connect to database
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();

                String sql = "SELECT ProductID, ProductName, Products.Description, ImagePath, UnitPrice, Products.CategoryID, Categories.CategoryID, CategoryName, Categories.Description AS CategoryDescription FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", "COLUMN_ProductID:", reader.GetInt32(COLUMN_ProductID).ToString());
                            Console.WriteLine("{0} {1}", "COLUMN_ProductName", reader.GetString(COLUMN_ProductName));
                            Console.WriteLine("{0} {1}", "COLUMN_Description", reader.GetString(COLUMN_Description));
                            Console.WriteLine("{0} {1}", "COLUMN_ImagePath", reader.GetString(COLUMN_ImagePath));
                            Console.WriteLine("{0} {1}", "COLUMN_UnitPrice", reader.GetDouble(COLUMN_UnitPrice).ToString());
                            Console.WriteLine("{0} {1}", "COLUMN_CategoryID", reader.GetInt32(COLUMN_CategoryID).ToString());
                            Products p = new Products();


                            p.ProductID = reader.GetInt32(COLUMN_ProductID);
                            p.ProductName = reader["ProductName"].ToString();
                            p.Description = reader["Description"].ToString();
                            p.ImagePath = reader["ImagePath"].ToString();
                            p.UnitPrice = reader.GetDouble(COLUMN_UnitPrice);
                            p.CategoryID = reader.GetInt32(COLUMN_CategoryID);

                            Category c = new Category();
                            c.CategoryID = reader.GetInt32(COLUMN_CategoryID);
                            c.CategoryName = reader["CategoryName"].ToString();
                            c.Description = reader["CategoryDescription"].ToString();

                            p.Category = c;


                            // p.ProductName="product name";
                            // p.Description="Product Description goes here";
                            // p.ImagePath="Image path";
                            // p.UnitPrice= (float?)9.88;
                            // p.CategoryID = 1;
                            l.Add(p);
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        return l;
    }
}

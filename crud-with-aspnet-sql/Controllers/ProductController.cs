using crud_with_aspnet_sql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics;

namespace crud_with_aspnet_sql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string connectionString;

        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO products " +
                        "(name, brand, category, price, description) VALUES " +
                        "(@name, @brand, @category, @price, @description)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);

                        command.ExecuteNonQuery();
                    }
                }
            }
			catch (SqlException ex)
			{
				// Log the exception or handle it appropriately
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>(); ;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM products";

                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                                products.Add(product);
                            }
                        }
                    }
                }
            }
			catch (SqlException ex)
			{
				// Log the exception or handle it appropriately
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = new Product();
            product.Id = id;

            try
            {
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sql = "SELECT * FROM products WHERE id = @id";

					using (var cmd = new SqlCommand(sql, connection))
					{
						cmd.Parameters.AddWithValue("@id", id);

						using (var reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								product.Id = reader.GetInt32(0);
								product.Name = reader.GetString(1);
								product.Brand = reader.GetString(2);
								product.Category = reader.GetString(3);
								product.Price = reader.GetDecimal(4);
								product.Description = reader.GetString(5);
								product.CreatedAt = reader.GetDateTime(6);
							}
                            else
                            {
                                return NotFound();
                            }
						}
					}
				}

			}
			catch (SqlException ex)
			{
				// Log the exception or handle it appropriately
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateProductSql = "DECLARE @RowCount INT; " +
											  "UPDATE products " +
											  "SET name=@name, brand=@brand, category=@category, price=@price, description=@description " +
											  "WHERE id=@id;" +
											  "SET @RowCount = @@ROWCOUNT;" +
                                              "SELECT @RowCount;";

					using (var cmd = new SqlCommand(updateProductSql, connection))
					{

						cmd.Parameters.AddWithValue("@name", productDto.Name);
						cmd.Parameters.AddWithValue("@brand", productDto.Brand);
						cmd.Parameters.AddWithValue("@category", productDto.Category);
						cmd.Parameters.AddWithValue("@price", productDto.Price);
						cmd.Parameters.AddWithValue("@description", productDto.Description);
						cmd.Parameters.AddWithValue("@id", id);

						int rowCount = (int)cmd.ExecuteScalar();

                        if (rowCount == 0)
                        {
                            return NotFound();
                        }
                        
                    }
                }
            }
			catch (SqlException ex)
			{
				// Log the exception or handle it appropriately
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(new {Message = "Product updated successfully."});
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

					string deleteProductSql = "DECLARE @RowCount INT; " +
											  "DELETE FROM products WHERE id=@id; " +
											  "SET @RowCount = @@ROWCOUNT; " +
											  "SELECT @RowCount;";

					using (var cmd = new SqlCommand(deleteProductSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int rowCount = (int)cmd.ExecuteScalar();

                        if (rowCount == 0)
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
				// Log the exception or handle it appropriately
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

            return Ok(new { Message = "Product deleted successfully." });
        }
	}
}

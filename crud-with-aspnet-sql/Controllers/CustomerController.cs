using crud_with_aspnet_sql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace crud_with_aspnet_sql.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly string connectionString;

        public CustomerController(IConfiguration configuration)
        {
			connectionString = configuration["ConnectionStrings:SqlServerDb"];
        }

		[HttpPost]
		public IActionResult CreateCustomer(CustomerDto customerDto)
		{
			try
			{
				using (var sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();

					string createCustomerSql = "INSERT INTO customers (name, email, phone_number, address) VALUES (@name, @email, @phone_number, @address)";

					using (var command = new SqlCommand(createCustomerSql, sqlConnection))
					{
						command.Parameters.AddWithValue("@name", customerDto.Name);
						command.Parameters.AddWithValue("@email", customerDto.Email);
						command.Parameters.AddWithValue("@phone_number", customerDto.PhoneNumber);
						command.Parameters.AddWithValue("@address", customerDto.Address);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqlException ex)
			{
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(new { Message = "New customer created successfully" });
		}

		[HttpGet]
		public IActionResult GetCustomers()
		{
			List<Customer> customers = new List<Customer>();

			try
			{
				using (var sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();

					string getCustomersSql = "SELECT * FROM customers";

					using (var command = new SqlCommand(getCustomersSql, sqlConnection))
					{
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Customer customer = new Customer();
								customer.Id = reader.GetInt32(0);
								customer.Name = reader.GetString(1);
								customer.Email = reader.GetString(2);
								customer.PhoneNumber = reader.GetString(3);
								customer.Address = reader.GetString(4);
								customer.CreatedDate = reader.GetDateTime(5);

								customers.Add(customer);

							}
						}
					}
				}

			}
			catch (SqlException ex)
			{
				ModelState.AddModelError("Error", "Sorry, Database Error: " + ex.Message);
				return BadRequest(ModelState) ;
			}

			return Ok(customers);
		}

		[HttpGet("{id}")]
		public IActionResult GetCustomer(int id)
		{
			Customer customer = new Customer();

			try
			{
				using (var sqlConnection = new SqlConnection(connectionString)) 
				{ 
					sqlConnection.Open();

					string getCustomerSql = "SELECT * FROM customers WHERE id=@id";

					using (var command = new SqlCommand(getCustomerSql, sqlConnection))
					{
						command.Parameters.AddWithValue("@id", id);

						using (var reader = command.ExecuteReader()) 
						{
                            if (reader.Read())
                            {
								customer.Id = reader.GetInt32(0);
								customer.Name = reader.GetString(1);
								customer.Email = reader.GetString(2);
								customer.PhoneNumber = reader.GetString(3);
								customer.Address = reader.GetString(4);
								customer.CreatedDate = reader.GetDateTime(5);
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
				ModelState.AddModelError("Error", "Sorry, Database Error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(customer);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateCustomer(int id, CustomerDto customerDto) 
		{
			try
			{
				using (var sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();

					string updateCustomerSql = "DECLARE @RowCount INT;" +
												"UPDATE customers SET name=@name, email=@email, phone_number=@phone_number, address=@address WHERE id=@id;" +
												"SET @RowCount = @@ROWCOUNT; " +
												"SELECT @RowCount;";
					using (var command = new SqlCommand(updateCustomerSql, sqlConnection)) 
					{
						command.Parameters.AddWithValue("@name", customerDto.Name);
						command.Parameters.AddWithValue("@email", customerDto.Email);
						command.Parameters.AddWithValue("@phone_number", customerDto.PhoneNumber);
						command.Parameters.AddWithValue("@address", customerDto.Address);
						command.Parameters.AddWithValue("@id", id);

						int rowCount = (int)command.ExecuteScalar();

                        if (rowCount == 0)
                        {
							return NotFound();
                        }

                    }
				}
			}
			catch (SqlException ex)
			{
				ModelState.AddModelError("Error", "Database error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(new { Message = "Customer updated successfully." }); 
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteCustomer(int id) 
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();

					string deleteCustomerSql = "DECLARE @RowCount INT;" +
												"DELETE FROM customers WHERE id=@id;" +
												"SET @RowCount = @@ROWCOUNT;" +
												"SELECT @RowCount;";

					using (var cmd = new SqlCommand(deleteCustomerSql, sqlConnection))
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
			catch(SqlException ex) 
			{
				ModelState.AddModelError("Error", "Database Error: " + ex.Message);
				return BadRequest(ModelState);
			}

			return Ok(new { Message = "Customer deleted successfully." });
		}
	}
}

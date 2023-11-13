namespace crud_with_aspnet_sql.Models
{
	public class Customer
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public string? Email { get; set; }

		public string? PhoneNumber { get; set; }

		public string? Address { get; set; }

		public DateTime? CreatedDate { get; set; } = DateTime.Now;
	}
}

using System.ComponentModel.DataAnnotations;

namespace crud_with_aspnet_sql.Models
{
	public class CustomerDto
	{
		[Required]
		public string Name { get; set; } = "";

		[Required]
		public string Email { get; set; } = "";

		[Required]
		public string PhoneNumber { get; set; } = "";

		[Required]
		public string Address { get; set; } = "";






	}
}

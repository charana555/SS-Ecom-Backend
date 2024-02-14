using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
	public class Cart
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

    }
}

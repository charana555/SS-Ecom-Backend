using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
	public class Orders
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public string ImageUrl { get; set; }
    }
}

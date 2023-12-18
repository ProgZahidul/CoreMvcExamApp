using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CoreMvcExamApp.Models
{
	public class InvoiceItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ItemId { get; set; }
		[Required]
		public string ItemName { get; set; }
		[Required]
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }

        public decimal ItemTotal => this.UnitPrice * this.Quantity;

        

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataLayer.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        
        public bool IsFinaly { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        
    }
}

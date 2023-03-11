using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataLayer.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        
        public string Text { get; set; }
        [Display(Name = "تصویر")]
        
        public string? ImageName { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        
        public int Price { get; set; }
        [Display(Name = "کلمات کلیدی")]
  
        public string Tags { get; set; }


        [ForeignKey("GroupId")]
        public ProductGroup? ProductGroup { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
        public List<ProductComment>? ProductComments { get; set; }



    }
}

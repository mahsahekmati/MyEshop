using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataLayer.ViewModels
{
    public class ShowGroupViewModel
    {
        

        public int GroupId { get; set; }
       
        public string GroupTitle { get; set; }
        public int ProductCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.DTOs.Category
{
    public class EditCategoryDto
    {
        public Guid Id { get; set; }//get isteğine karşı Id bilgisini de getir.
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        public string Slug { get; set; } 
    }
}

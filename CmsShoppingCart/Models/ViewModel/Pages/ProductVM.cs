using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModel.Pages
{
    public class ProductVM
    {
        public ProductVM()
        {

        }
        public ProductVM(ProductDTO row)
        {
            this.Id = row.Id;
            this.Name = row.Name;
            this.Slug = row.Slug;
            this.Description = row.Description;
            this.Price = row.Price;
            this.ImgName = row.ImgName;
            this.CatagoryId = row.CatagoryId;
            this.catagoryName = row.catagoryName;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string ImgName { get; set; }

        public string catagoryName { get; set; }
        [Required]
        public int CatagoryId { get; set; }
      
        public virtual CatagoryDTO Catagory { get; set; }

        public IEnumerable<SelectListItem> Catagories { get; set; }

        public IEnumerable<string> GalleryImages { get; set; }
    }
}
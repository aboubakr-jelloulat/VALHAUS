using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhaus.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }


        [Required]
        public string Description { get; set; }

        [Required]
        public string SKU { get; set; }
        /*
            Stock Keeping Unit : a short unique identifier you assign to each product (used for inventory, lookup, barcodes, supplier references
            should be unique within your catalog; often alphanumeric "VH-CHAIR-001" or "VAL-CH-001" 
          
         */


        [Required]
        [Display(Name = "List Price")]
        [Range(1, 100000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 100000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 100000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        /*
         * Display Purpose: controls the human-friendly label/name that UI helpers and validation messages use for this property.
         */
        [Range(1, 100000)]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Categories { get; set; }

        [ValidateNever]
        [Required]
        public string? ImageUrl { get; set; }

    }
}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain
{
    public class Product : BaseEntity
    {
        [MaxLength(64)]
        [MinLength(1)]
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        
        [MaxLength(200)]
        [MinLength(1)]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        
        [Display(Name = "Product Value (€)")]
        public decimal ProductValue { get; set; }
        
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        
        [JsonIgnore]
        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
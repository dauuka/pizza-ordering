using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain
{
    public class ProductType : BaseEntity
    {
        [MaxLength(64)]
        [MinLength(1)]
        [Required]
        public string ProductTypeName { get; set; }
        
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
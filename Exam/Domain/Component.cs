using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain
{
    public class Component : BaseEntity
    {
        [MaxLength(64)]
        [MinLength(1)]
        [Required]
        public string ComponentName { get; set; }
        
        public decimal ComponentValue { get; set; }
        
        [JsonIgnore]
        public ICollection<ComponentInLine> ComponentInLines { get; set; }
    }
}
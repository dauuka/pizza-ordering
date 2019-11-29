using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain
{
    public class Transport : BaseEntity
    {
        [MaxLength(64)]
        [MinLength(1)]
        [Required]
        public string TransportName { get; set; }
        
        public decimal TransportValue { get; set; }
        
        [JsonIgnore]
        public ICollection<FullOrder> FullOrders { get; set; }
    }
}
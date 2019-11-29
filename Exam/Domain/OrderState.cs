using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain
{
    public class OrderState : BaseEntity
    {
        [MaxLength(64)]
        [MinLength(1)]
        [Required]
        public string OrderStateName { get; set; }
        
        [JsonIgnore]
        public ICollection<FullOrder> FullOrders { get; set; }
    }
}
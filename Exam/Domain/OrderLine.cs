using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain
{
    public class OrderLine : BaseEntity
    {
        public int ProductQuantity { get; set; }
        
        public decimal ProductValue { get; set; }
        
        public decimal LineSum { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int FullOrderId { get; set; }
        [JsonIgnore]
        public FullOrder FullOrder { get; set; }
        
        public ICollection<ComponentInLine> ComponentInLines { get; set; }
    }
}
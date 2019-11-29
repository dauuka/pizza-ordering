using Newtonsoft.Json;

namespace Domain
{
    public class ComponentInLine : BaseEntity
    {
        public int OrderLineId { get; set; }
        [JsonIgnore]
        public OrderLine OrderLine { get; set; }
        
        public int ComponentId { get; set; }
        public Component Component { get; set; }
    }
}
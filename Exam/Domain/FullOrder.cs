using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Identity;
using Newtonsoft.Json;

namespace Domain
{
    public class FullOrder : BaseEntity
    {
        public decimal Sum { get; set; }
        
        public DateTime? Time { get; set; }
        
        [MaxLength(64)]
        [MinLength(1)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        
        [MaxLength(25)]
        [MinLength(1)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Order State")]
        public int? OrderStateId { get; set; }
        public OrderState OrderState { get; set; }
        
        public int AppUserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        
        [Display(Name = "Transport")]
        public int? TransportId { get; set; }
        public Transport Transport { get; set; }
        
        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
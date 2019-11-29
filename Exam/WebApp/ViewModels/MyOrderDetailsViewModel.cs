using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class MyOrderDetailsViewModel
    {
        public FullOrder FullOrder { get; set; }
        public SelectList TransportSelectList { get; set; }
    }
}
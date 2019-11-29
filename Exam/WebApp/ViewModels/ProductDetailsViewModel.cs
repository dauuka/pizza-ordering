using System.Collections.Generic;
using Domain;

namespace WebApp.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Component> Components { get; set; }
    }
}
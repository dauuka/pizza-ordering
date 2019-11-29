using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class FullOrderEditViewModel
    {
        public FullOrder FullOrder { get; set; }
        public SelectList OrderStateSelectList { get; set; }
    }
}
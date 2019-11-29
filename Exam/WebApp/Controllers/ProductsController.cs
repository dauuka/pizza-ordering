using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using ee.itcollege.dauuka.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Products;
            return View(await appDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var components = await _context.Components.ToListAsync();
            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                Components = components
            };
            
            return View(vm);
        }

        public async Task<IActionResult> AddToCart([Bind("ProductValue,Id")] Product product)
        {
            var fullOrder = _context.FullOrders.LastOrDefault(f => f.AppUserId == User.GetUserId());
            if (fullOrder == null)
            {
                var f = new FullOrder()
                {
                    Sum = 0,
                    AppUserId = User.GetUserId()
                };
                await _context.FullOrders.AddAsync(f);
                await _context.SaveChangesAsync();
                fullOrder = _context.FullOrders.LastOrDefault(o => o.AppUserId == User.GetUserId());
            }

            if (fullOrder != null)
            {
                var orderLine = new OrderLine()
                {
                    ProductQuantity = 1,
                    ProductValue = product.ProductValue,
                    LineSum = product.ProductValue,
                    ProductId = product.Id,
                    FullOrderId = fullOrder.Id
                };
                fullOrder.Sum += product.ProductValue;
                _context.FullOrders.Update(fullOrder);
                await _context.OrderLines.AddAsync(orderLine);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

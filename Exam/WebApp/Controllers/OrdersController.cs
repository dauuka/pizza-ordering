using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FullOrders
                .Include(f => f.OrderState)
                .Include(f => f.Transport)
                .Where(f => f.Address != null)
                .OrderByDescending(f => f.Id);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fullOrder = await _context.FullOrders.FindAsync(id);
            if (fullOrder == null)
            {
                return NotFound();
            }
            var vm = new FullOrderEditViewModel();
            vm.FullOrder = fullOrder;
            vm.OrderStateSelectList = new SelectList(_context.OrderStates,
                nameof(OrderState.Id), nameof(OrderState.OrderStateName), 
                fullOrder.OrderStateId);
            return View(vm);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FullOrderEditViewModel vm)
        {
            if (id != vm.FullOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(vm.FullOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.OrderStateSelectList = new SelectList(_context.OrderStates,
                nameof(OrderState.Id), nameof(OrderState.OrderStateName), 
                vm.FullOrder.OrderStateId);
            return View(vm);
        }
    }
}

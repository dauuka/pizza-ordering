using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using ee.itcollege.dauuka.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "User")]
    public class MyOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public MyOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyOrders
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FullOrders
                .Include(f => f.OrderState)
                .Where(f => f.AppUserId == User.GetUserId())
                .OrderByDescending(f => f.Id);
            return View(await appDbContext.ToListAsync());
        }
        
        // GET: MyOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!await _context.FullOrders.AnyAsync(p => p.Id == id && (p.AppUserId == User.GetUserId())))
            {
                return NotFound();
            }
            
            var fullOrder = await _context.FullOrders
                .Include(f => f.OrderLines)
                    .ThenInclude(o => o.Product)
                .Include(f => f.OrderLines)
                    .ThenInclude(o => o.ComponentInLines).ThenInclude(c => c.Component)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (fullOrder == null)
            {
                return NotFound();
            }
            var vm = new MyOrderDetailsViewModel();
            vm.FullOrder = fullOrder;
            vm.TransportSelectList = new SelectList(_context.Transports,
                nameof(Transport.Id), nameof(Transport.TransportName), fullOrder.TransportId);
            return View(vm);
        }

        // POST: MyOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MyOrderDetailsViewModel vm)
        {
            if (id != vm.FullOrder.Id)
            {
                return NotFound();
            }
            
            if (!await _context.FullOrders.AnyAsync(p => p.Id == id && (p.AppUserId == User.GetUserId())))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var transport = _context.Transports.Find(vm.FullOrder.TransportId);
                vm.FullOrder.Sum += transport.TransportValue;
                vm.FullOrder.Time = DateTime.Now;
                _context.Update(vm.FullOrder);
                var fullOrder = new FullOrder()
                {
                    Sum = 0,
                    AppUserId = User.GetUserId()
                };
                _context.Add(fullOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.TransportSelectList = new SelectList(_context.Transports,
                nameof(Transport.Id), nameof(Transport.TransportName), 
                vm.FullOrder.TransportId);
            return View(vm);
        }
    }
}

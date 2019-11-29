using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using ee.itcollege.dauuka.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FullOrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FullOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FullOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FullOrder>>> GetFullOrders()
        {
            return await _context.FullOrders
                .Include(f => f.OrderState)
                .Where(f => f.AppUserId == User.GetUserId()).OrderByDescending(f => f.Id).ToListAsync();
        }

        // GET: api/FullOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FullOrder>> GetFullOrder(int id)
        {
            if (!await _context.FullOrders.AnyAsync(p => p.Id == id && (p.AppUserId == User.GetUserId())))
            {
                return NotFound();
            }
            
            var fullOrder = await _context.FullOrders
                .Include(f => f.OrderLines).ThenInclude(o => o.Product)
                .Include(f => f.OrderLines).ThenInclude(o => o.ComponentInLines)
                .ThenInclude(c => c.Component)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fullOrder == null)
            {
                return NotFound();
            }

            return fullOrder;
        }

        // PUT: api/FullOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFullOrder(int id, FullOrder fullOrder)
        {
            if (id != fullOrder.Id)
            {
                return BadRequest();
            }

            if (!await _context.FullOrders.AnyAsync(p => p.Id == id && (p.AppUserId == User.GetUserId())))
            {
                return NotFound();
            }
            
            var f = new FullOrder()
            {
                Sum = 0,
                AppUserId = User.GetUserId()
            };
            _context.Add(f);
            fullOrder.Time = DateTime.Now;
            _context.Update(fullOrder);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/FullOrders
        [HttpPost]
        public async Task<ActionResult<FullOrder>> PostFullOrder(FullOrder fullOrder)
        {
            fullOrder.Sum = 0;
            fullOrder.AppUserId = User.GetUserId();
            _context.FullOrders.Add(fullOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFullOrder", new { id = fullOrder.Id }, fullOrder);
        }
    }
}

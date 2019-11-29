using System.Linq;
using System.Threading.Tasks;
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
    public class OrderLinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderLinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderLine>> GetOrderLine(int id)
        {
            var orderLine = await _context.OrderLines.FindAsync(id);

            if (orderLine == null)
            {
                return NotFound();
            }
            
            if (!await _context.OrderLines
                .AnyAsync(p => p.Id == id && (p.FullOrder.AppUserId == User.GetUserId())))
            {
                return NotFound();
            }

            return orderLine;
        }

        // POST: api/OrderLines
        [HttpPost]
        public async Task<ActionResult<OrderLine>> PostOrderLine(OrderLine orderLine)
        {
            var fullOrder = _context.FullOrders
                .FirstOrDefault(f => f.AppUserId == User.GetUserId() && f.Address == null);
            if (fullOrder == null)
            {
                _context.FullOrders.Add(new FullOrder()
                {
                    Sum = 0,
                    AppUserId = User.GetUserId()
                });
                _context.SaveChanges();
                fullOrder = _context.FullOrders
                    .FirstOrDefault(f => f.AppUserId == User.GetUserId() && f.Address == null);
            }

            if (fullOrder != null)
            {
                var lineSum = orderLine.LineSum;
                orderLine.FullOrderId = fullOrder.Id;
                _context.OrderLines.Add(orderLine);
                _context.SaveChanges();
                fullOrder.Sum += lineSum;
                _context.FullOrders.Update(fullOrder);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrderLine", new {id = orderLine.Id}, orderLine);
        }
        
        // DELETE: api/OrderLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderLine>> DeleteOrderLine(int id)
        {
            var orderLine = await _context.OrderLines.FindAsync(id);
            if (orderLine == null)
            {
                return NotFound();
            }

            if (orderLine.ComponentInLines != null)
            {
                _context.ComponentInLines.RemoveRange(orderLine.ComponentInLines);
                await _context.SaveChangesAsync();
            }

            var fullOrder = _context.FullOrders.Find(orderLine.FullOrderId);
            fullOrder.Sum -= orderLine.LineSum;
            _context.FullOrders.Update(fullOrder);
            await _context.SaveChangesAsync();
            
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();

            return orderLine;
        }
    }
}

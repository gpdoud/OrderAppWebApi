using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OrderAppWebApi.Data;
using OrderAppWebApi.Models;

namespace OrderAppWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Orders/proposed
        [HttpGet("proposed")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersInProposedStatus() {
            return await _context.Orders
                                .Include(c => c.Customer)
                                .Where(o => o.Status == "PROPOSED")
                                .ToListAsync();

            //var orders = from o in _context.Orders
            //             where o.Status == "PROPOSED"
            //             select o;
            //return await orders.ToListAsync();
        }

        // PUT: api/orders/edit/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> SetOrderStatusToEdit(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }
            order.Status = "EDIT";
            return await PutOrder(order.Id, order);
        }

        // PUT: api/orders/proposed/5
        [HttpPut("proposed/{id}")]
        public async Task<IActionResult> SetOrderStatusToProposed(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }
            order.Status = (order.Total <= 100) ? "FINAL" : "PROPOSED";
            return await PutOrder(order.Id, order);
        }

        // PUT: api/orders/final/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> SetOrderStatusToFinal(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }
            order.Status = "FINAL";
            return await PutOrder(order.Id, order);
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
            return await _context.Orders
                                .Include(c => c.Customer)
                                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id) {
            var order = await _context.Orders
                                .Include(c => c.Customer)
                                .Include(s => s.Salesperson)
                                .Include(l => l.Orderlines)
                                .ThenInclude(i => i.Item)
                                .SingleOrDefaultAsync(o => o.Id == id);

            if(order == null) {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order) {
            if(id != order.Id) {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order) {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id) {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

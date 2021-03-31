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
    public class OrderlinesController : ControllerBase {
        private readonly AppDbContext _context;

        public OrderlinesController(AppDbContext context) {
            _context = context;
        }

        private async Task<IActionResult> CalculateOrderTotal(int id) {
            var order = await _context.Orders.FindAsync(id);
            if(order == null) {
                return NotFound();
            }
            order.Total = _context.Orderlines
                                    .Where(li => li.OrderId == id)
                                    .Sum(li => li.Quantity * li.Item.Price);

            var total = (from l in _context.Orderlines
                         join i in _context.Items
                         on l.ItemId equals i.Id
                         where l.OrderId == id
                         select new { LineTotal = l.Quantity * i.Price }).Sum(x => x.LineTotal);

            var rowsAffected = await _context.SaveChangesAsync();
            if(rowsAffected != 1) {
                throw new Exception("Failed to update Order Total");
            }
            return Ok();
        }

        // GET: api/Orderlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderline>>> GetOrderlines() {
            return await _context.Orderlines
                                    .Include(x => x.Item)
                                    .ToListAsync();
        }

        // GET: api/Orderlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderline>> GetOrderline(int id) {
            var orderline = await _context.Orderlines
                                    .Include(x => x.Item)
                                    .SingleOrDefaultAsync(x => x.Id == id);

            if(orderline == null) {
                return NotFound();
            }

            return orderline;
        }

        // PUT: api/Orderlines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderline(int id, Orderline orderline) {
            if(id != orderline.Id) {
                return BadRequest();
            }

            _context.Entry(orderline).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                await CalculateOrderTotal(orderline.OrderId);
            } catch(DbUpdateConcurrencyException) {
                if(!OrderlineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orderlines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Orderline>> PostOrderline(Orderline orderline) {
            _context.Orderlines.Add(orderline);
            await _context.SaveChangesAsync();
            await CalculateOrderTotal(orderline.OrderId);

            return CreatedAtAction("GetOrderline", new { id = orderline.Id }, orderline);
        }

        // DELETE: api/Orderlines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Orderline>> DeleteOrderline(int id) {
            var orderline = await _context.Orderlines.FindAsync(id);
            if(orderline == null) {
                return NotFound();
            }

            _context.Orderlines.Remove(orderline);
            await _context.SaveChangesAsync();
            await CalculateOrderTotal(orderline.OrderId);

            return orderline;
        }

        private bool OrderlineExists(int id) {
            return _context.Orderlines.Any(e => e.Id == id);
        }
    }
}

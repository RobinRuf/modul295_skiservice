using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiService.Models;
using SkiService.Models.dto;

namespace SkiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly SkiServiceContext _context;

        public ServiceController(SkiServiceContext context)
        {
            _context = context;
        }

        // Create new Service
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostServiceOrder(ServiceOrderDto serviceOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceOrder = new ServiceOrder
            {
                CustomerName = serviceOrderDto.CustomerName,
                CustomerEmail = serviceOrderDto.CustomerEmail,
                CustomerPhone = serviceOrderDto.CustomerPhone,
                ServiceType = serviceOrderDto.ServiceType,
                Priority = serviceOrderDto.Priority,
                Status = serviceOrderDto.Status,
                CreationDate = DateTime.Now,
                StartDate = serviceOrderDto.StartDate,
                CompletionDate = serviceOrderDto.CompletionDate
            };

            _context.ServiceOrders.Add(serviceOrder);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, serviceOrder);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetServiceOrders()
        {
            return await _context.ServiceOrders.ToListAsync();
        }

        [HttpDelete("{orderId}")]
        [Authorize]
        public async Task<IActionResult> DeleteServiceOrder(int orderId)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(orderId);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            _context.ServiceOrders.Remove(serviceOrder);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateServiceOrderStatus(int orderId, [FromBody] UpdateStatusDto updateStatusDto)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(orderId);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            serviceOrder.Status = updateStatusDto.Status;
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
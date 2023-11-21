using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        /// <summary>
        /// Create New Services
        /// </summary>
        /// <param name="serviceOrderDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostServiceOrder(CreateServiceOrderDto serviceOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check und Create Customer
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Email == serviceOrderDto.CustomerEmail);
            if (customer == null)
            {
                customer = new CustomerModel
                {
                    Name = serviceOrderDto.CustomerName,
                    Email = serviceOrderDto.CustomerEmail,
                    Phone = serviceOrderDto.CustomerPhone
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // Find Priority and ServiceType by Name
            var priority = await _context.Priorities
                .SingleOrDefaultAsync(p => p.Priority == serviceOrderDto.Priority);
            var serviceType = await _context.Services
                .SingleOrDefaultAsync(s => s.ServiceType == serviceOrderDto.ServiceType);

            // Überprüfungen für nicht gefundene Priority und ServiceType
            if (priority == null || serviceType == null)
            {
                return BadRequest("Ungültige Priority oder ServiceType.");
            }

            var serviceOrder = new ServiceOrderModel
            {
                CustomerID = customer.ID,
                ServiceType = serviceType.ID,
                Priority = priority.ID,
                Status = 1, // hardcoded status ID 1 = "Offen", because it is always "Offen" if a new service will be created. In aspect of performance, this is the way I choose.
                CreationDate = DateTime.Now,
                StartDate = serviceOrderDto.StartDate,
                CompletionDate = serviceOrderDto.EndDate,
                Sum = serviceOrderDto.Sum
            };

            _context.ServiceOrders.Add(serviceOrder);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }




        /// <summary>
        /// Get all Service Orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetServiceOrderDto>>> GetServiceOrders()
        {
            var serviceOrders = await _context.ServiceOrders
                .Include(so => so.Employee)
                .Include(so => so.Customer)
                .Include(so => so.PriorityInfo)
                .Include(so => so.StatusInfo)
                .Include(so => so.Service)
                .Select(so => new GetServiceOrderDto
                {
                    OrderID = so.OrderID,
                    EmployeeName = so.Employee.Name ?? "N/A",
                    CustomerName = so.Customer.Name,
                    CustomerEmail = so.Customer.Email,
                    CustomerPhone = so.Customer.Phone,
                    ServiceType = so.Service.ServiceType,
                    Priority = so.PriorityInfo.Priority,
                    Status = so.StatusInfo.Status,
                    CreationDate = so.CreationDate,
                    StartDate = so.StartDate,
                    CompletionDate = so.CompletionDate,
                    Comment = so.Comment ?? "Kein Kommentar",
                    Sum = so.Sum,
                })
                .ToListAsync();

            return Ok(serviceOrders);
        }

        /// <summary>
        /// "Delete" Service Order (actually, just update status to "Gelöscht")
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPatch("delete/{orderId}")]
        [Authorize]
        public async Task<IActionResult> SetServiceOrderStatusToDelete(int orderId)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(orderId);

            if (serviceOrder == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses
                .Where(s => s.Status == "Gelöscht")
                .SingleOrDefaultAsync();

            if (status == null)
            {
                return BadRequest("Der Status 'Gelöscht' existiert nicht.");
            }

            serviceOrder.Status = status.ID;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Change the Status of an Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="updateStatusDto"></param>
        /// <returns></returns>
        [HttpPatch("update/{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateServiceOrderStatus(int orderId, [FromBody] UpdateStatusDto updateStatusDto)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(orderId);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            // Find the status ID based on the status name provided
            var status = await _context.Statuses
                .Where(s => s.Status == updateStatusDto.Status)
                .SingleOrDefaultAsync();

            if (status == null)
            {
                return BadRequest("Der angeforderte Status existiert nicht.");
            }

            // Update the service order status ID
            serviceOrder.Status = status.ID;
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Assign yourself as the employee that do this order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="assignOrderDto"></param>
        /// <returns></returns>
        [HttpPatch("assign/{orderId}")]
        [Authorize]
        public async Task<IActionResult> AssignOrderToEmployee(int orderId, [FromBody] AssignOrderDto assignOrderDto)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(orderId);
            if (serviceOrder == null)
            {
                return NotFound("Serviceauftrag nicht gefunden.");
            }

            // Suche den Mitarbeiter anhand des Usernamens
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == assignOrderDto.Username);
            if (employee == null)
            {
                return NotFound("Mitarbeiter nicht gefunden.");
            }

            // Weise den Serviceauftrag dem Mitarbeiter zu
            serviceOrder.EmployeeID = employee.ID;
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
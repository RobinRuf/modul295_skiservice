using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkiService.Models;
using SkiService.Models.dto;

namespace SkiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly SkiServiceContext _context;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(SkiServiceContext context, ILogger<ServiceController> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogError("BadRequest Error");
                return BadRequest(ModelState);
            }

            var customerDto = new CreateCustomerDto
            {
                Name = serviceOrderDto.CustomerName,
                Email = serviceOrderDto.CustomerEmail,
                Phone = serviceOrderDto.CustomerPhone
            };

            // Check und Create Customer
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Email == customerDto.Email);
            if (customer == null)
            {
                customer = new CustomerModel
                {
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    Phone = customerDto.Phone
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Kunde {customerDto.Name} erfolgreich erstellt.");
            }


            // Find Priority and ServiceType by Name
            var checkPriorityDto = new CheckPriorityDto
            {
                Priority = serviceOrderDto.Priority
            };

            var checkServiceTypeDto = new CheckServiceTypeDto
            {
                ServiceType = serviceOrderDto.ServiceType
            };

            var priority = await _context.Priorities
                .SingleOrDefaultAsync(p => p.Priority == checkPriorityDto.Priority);
            var serviceType = await _context.Services
                .SingleOrDefaultAsync(s => s.ServiceType == checkServiceTypeDto.ServiceType);

            // Check if Priority or ServiceType are null (should not be)
            if (priority == null || serviceType == null)
            {
                _logger.LogError("BadRequest Error: Priority oder ServiceType ist ungültig.");
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
            _logger.LogInformation("Ein Auftrag wurde erfolgreich erstellt.");

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

            _logger.LogInformation("Erfolgreich alle Aufträge erhalten.");
            return Ok(serviceOrders);
        }

        /// <summary>
        /// Get all Service Orders filtered
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<GetServiceOrderDto>>> GetServiceOrdersByPriority(string priority)
        {
            List<ServiceOrderModel> serviceOrders;

            if (priority.ToLower() == "alle")
            {
                serviceOrders = await _context.ServiceOrders
                    .Include(so => so.Employee)
                    .Include(so => so.Customer)
                    .Include(so => so.PriorityInfo)
                    .Include(so => so.StatusInfo)
                    .Include(so => so.Service)
                    .ToListAsync();
            }
            else
            {
                var priorityRecord = await _context.Priorities
                    .SingleOrDefaultAsync(p => p.Priority == priority);

                if (priorityRecord == null)
                {
                    _logger.LogError("Keine Aufträge mit der angegebenen Priorität gefunden.");
                    return NotFound($"Keine Aufträge mit der Priorität '{priority}' gefunden.");
                }

                serviceOrders = await _context.ServiceOrders
                    .Where(so => so.Priority == priorityRecord.ID)
                    .Include(so => so.Employee)
                    .Include(so => so.Customer)
                    .Include(so => so.PriorityInfo)
                    .Include(so => so.StatusInfo)
                    .Include(so => so.Service)
                    .ToListAsync();
            }

            var serviceOrderDtos = serviceOrders.Select(so => new GetServiceOrderDto
            {
                OrderID = so.OrderID,
                EmployeeName = so.Employee != null ? so.Employee.Name : "N/A",
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
                Sum = so.Sum
            }).ToList();

            _logger.LogInformation("Erfolgreich die gewünschten Aufträge erhalten.");
            return Ok(serviceOrderDtos);
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
                _logger.LogError("Der Serviceauftrag wurde nicht gefunden und konnte somit auch nicht gelöscht werden.");
                return NotFound();
            }

            var status = await _context.Statuses
                .Where(s => s.Status == "Gelöscht")
                .SingleOrDefaultAsync();

            if (status == null)
            {
                _logger.LogCritical("Der Status 'Gelöscht' existiert in der DB-Tabelle 'statuses' nicht. Bitte überprüfen.");
                return BadRequest("Der Status 'Gelöscht' existiert nicht.");
            }

            serviceOrder.Status = status.ID;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Erfolgreich den Status des Auftrags mit der ID {orderId} auf 'Gelöscht' aktualisiert.");

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
                _logger.LogError("Der Serviceauftrag konnte nicht gefunden und somit auch der Status nicht aktualisiert werden.");
                return NotFound();
            }

            // Find the status ID based on the status name provided
            var status = await _context.Statuses
                .Where(s => s.Status == updateStatusDto.Status)
                .SingleOrDefaultAsync();

            if (status == null)
            {
                _logger.LogCritical("Der gewünschte Status existiert in der DB-Tabelle 'statuses' nicht. Bitte überprüfen.");
                return BadRequest("Der angeforderte Status existiert nicht.");
            }

            // Update the service order status ID
            serviceOrder.Status = status.ID;
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Erfolgreich den Status des Auftrags mit der ID {orderId} aktualisiert.");

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
                _logger.LogError("Der Serviceauftrag konnte nicht gefunden und der Mitarbeiter somit auch nicht zugewiesen werden.");
                return NotFound("Serviceauftrag nicht gefunden.");
            }

            // Suche den Mitarbeiter anhand des Usernamens
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == assignOrderDto.Username);
            if (employee == null)
            {
                _logger.LogCritical("Der Mitarbeiter konnte in der DB-Tabelle 'employees' nicht gefunden werden. Bitte überprüfen.");
                return NotFound("Mitarbeiter nicht gefunden.");
            }

            // Weise den Serviceauftrag dem Mitarbeiter zu
            serviceOrder.EmployeeID = employee.ID;
            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Erfolgreich den Mitarbeiter {employee.Name} dem Auftrag ");

            return Ok();
        }

    }
}
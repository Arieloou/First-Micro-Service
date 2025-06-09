using Microsoft.AspNetCore.Mvc;
using NotificationsService.Application.DTOs;
using NotificationsService.Application.Interfaces;
using NotificationsService.Application.Services;
using NotificationsService.Infraestructure.Persistence;

namespace NotificationsService.Presentation.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class NotificationsController : ControllerBase
    {
        //private readonly Func<string, INotificationProvider> _factoryProvider;
        //private readonly ApplicationDBContext _context;
        //private readonly ILogger<NotificationsController> _logger;
        private readonly NotificationAppService _appService;
        public NotificationsController(NotificationAppService appService)
        {
            //_factoryProvider = factoryProvider;
            //_logger = logger;
            //_context = context;
            _appService = appService;
        }

        [HttpPost] 
        public async Task<ActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var status = await _appService.CreateAndSendNotificationAsync(request);
            return Ok(status);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Authentication;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories;

namespace SamaritanAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationRepository notificationRepository;
        private readonly UserManager<AppUser> userManager;

        public NotificationController(NotificationRepository notificationRepository, UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            this.notificationRepository = notificationRepository;
        }

        [HttpGet($"{{userId}}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUserNotifications(string userId)
        {
            var notifications = await notificationRepository.GetUserNotifications(userId);
            if (notifications is null)
            {
                return NotFound();
            }
            return Ok(notifications);
        }

        [HttpGet($"{{Id}}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int Id)
        {
            var notification = await notificationRepository.GetNotificationById(Id);
            if (notification is null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNotification([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await notificationRepository.CreateNotification(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, notification);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateNotification([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await notificationRepository.UpdateNotification(notification);
            return Ok();
        }

        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> DeleteNotification(int notificationId)
        {
            var notification = await notificationRepository.GetNotificationById(notificationId);
            if (notification is null)
            {
                return NotFound();
            }
            await notificationRepository.DeleteNotification(notificationId);
            return Ok();
        }

        [HttpPost("send/{userId}")]
        public async Task<ActionResult> SendNotification(string userId, [FromBody] string message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            await notificationRepository.SendNotification(userId, message);
            return Ok();
        }
        
    }
}
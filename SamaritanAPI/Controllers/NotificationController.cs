using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Authentication;
using SamaritanAPI.Models;
using SamaritanAPI.Repositories;

namespace SamaritanAPI.Controllers
{
    [Authorize]
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await notificationRepository.GetUserNotifications(userId);
            if (notifications is null)
            {
                return NotFound();
            }
            return Ok(notifications);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetNotificationById(int Id)
        {
            var notification = await notificationRepository.GetNotificationById(Id);
            if (notification is null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid Format!");
                return BadRequest(ModelState);
            }
            await notificationRepository.CreateNotification(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, notification);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid Format!");
                return BadRequest(ModelState);
            }
            var existingNotification = notificationRepository.GetNotificationById(id);
            if(existingNotification is null)
                return NotFound();
            await notificationRepository.UpdateNotification(notification);
            return Ok();
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
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
        public async Task<IActionResult> SendNotification(string userId, [FromBody] string message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();
            await notificationRepository.SendNotification(userId, message);
            return Ok();
        }
        
    }
}
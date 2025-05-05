using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SamaritanAPI.Authentication;
using SamaritanAPI.Models;
using SamaritanAPI.Models.DTOs;
using SamaritanAPI.Repositories;
using SamaritanAPI.Repositories.Interfaces;

namespace SamaritanAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository notificationRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IRequestRepository requestRepository;

        public NotificationController(INotificationRepository notificationRepository, 
            UserManager<AppUser> userManager,
            IRequestRepository requestRepository)
        {
            this.userManager = userManager;
            this.requestRepository = requestRepository;
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
        public async Task<IActionResult> SendNotification(string userId, [FromBody] NotificationDto notification)
        {
            string title = notification.title;
            string body = notification.body;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();
            await notificationRepository.SendNotification(userId, title, body);
            return Ok();
        }

        [HttpPut("notify-all/{requestId}")]
        public async Task<IActionResult> NotifyAll(int requestId, [FromBody] NotificationDto notification)
        {
            string title = notification.title;
            string body = notification.body;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = await requestRepository.GetRequest(requestId);
            if (request is null)
                return NotFound();
            await notificationRepository.NotifyAll(request.Id, title, body);
            return Ok();
        }
        
    }
}
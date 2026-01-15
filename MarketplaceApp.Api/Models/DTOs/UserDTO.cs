using System.ComponentModel.DataAnnotations;

namespace MarketplaceApp.Api.Models.DTOs;

public class UserDTO
{
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        
        public string? ProfileDescription { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

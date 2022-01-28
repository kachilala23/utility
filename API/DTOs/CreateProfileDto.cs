using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.DTOs
{
    public class CreateProfileDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public string LGA { get; set; } = string.Empty;

        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string GuarantorName { get; set; } = string.Empty;
        [Required]
        public string GuarantorAddress { get; set; } = string.Empty;
        [Required]
        public string GuarantorPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string GuarantorOfficeAddress { get; set; } = string.Empty;

    }
}
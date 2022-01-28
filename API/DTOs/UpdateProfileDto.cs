using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.DTOs
{
    public class UpdateProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LGA { get; set; } = string.Empty;
        public IFormFile File { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string GuarantorName { get; set; } = string.Empty;
        public string GuarantorAddress { get; set; } = string.Empty;
        public string GuarantorPhoneNumber { get; set; } = string.Empty;
        public string GuarantorOfficeAddress { get; set; } = string.Empty;

    }
}
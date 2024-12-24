using Padiatric.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Padiatric.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Professor, or Assistant

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // For professors only
        public int? DepartmentId { get; set; }

        // List of departments for dropdown
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}

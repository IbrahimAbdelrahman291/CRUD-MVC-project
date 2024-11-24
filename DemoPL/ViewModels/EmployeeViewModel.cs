using DemoDAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace DemoPL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "max length of name is 50 ")]
        [MinLength(5, ErrorMessage = "min length of name is 5 ")]
        public string Name { get; set; }

        [Range(22, 35, ErrorMessage = "Age must be in range from 22 to 35")]
        public int? Age { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }

        [ForeignKey("department")]
        public int? DepartmentId { get; set; }
        [InverseProperty(nameof(Department.Employees))]
        public Department department { get; set; }
    }
}

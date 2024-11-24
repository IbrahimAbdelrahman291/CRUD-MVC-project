using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDAL.Models
{
   
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }
        [InverseProperty(nameof(Employee.department))]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}

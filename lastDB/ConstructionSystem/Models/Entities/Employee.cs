using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionSystem.Models.Entities
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must Be Between 2 & 50 Character")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must Be Between 2 & 50 Character")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }



        [RegularExpression("^[0-9]*$", ErrorMessage = "PHONE Must be Numeric")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        [Phone]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Please Enter a Valid Phone Number")]
        //[Remote("IsPhoneExists", "Employees", ErrorMessage = "Phone already in use")]
        public string Phone { get; set; }

        public decimal? Salary { get; set; }

        public bool? Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? HireDate { get; set; }

        [Required]
        [Display(Name = "Email")]
        [Remote("IsUserExists", "Employees", ErrorMessage = "Email already in use")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        public Employee employee { get; set; }

        [ForeignKey("employee")]
        [Display(Name = "Supervisor")]
        public int? SuperId { get; set; }

        public Department Department { get; set; }

        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }

    }
}
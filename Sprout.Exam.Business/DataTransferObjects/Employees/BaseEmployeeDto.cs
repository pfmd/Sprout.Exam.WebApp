using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects.Employees
{
    public abstract class BaseEmployeeDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength (100)]
        [RegularExpression("^[0-9.-]*$", ErrorMessage = "TIN must be properly formatted")]
        public string Tin { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public int TypeId { get; set; }
    }
}

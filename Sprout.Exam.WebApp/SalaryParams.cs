using System.ComponentModel.DataAnnotations;

namespace Sprout.Exam.WebApp
{
    public class SalaryParams
    {   
        [Required]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Invalid absent days")]
        [Range(typeof(decimal), "0", "22", ErrorMessage = "Absent days must be within the total working days and not less than zero")]
        public decimal absentDays { get; set; }

        [Required]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Invalid input for worked days")]
        [Range(typeof(decimal), "0", "365", ErrorMessage = "Worked days should not be less than zero")]
        public decimal workedDays { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects.EmployeeTypes
{
    public abstract class BaseSaveEmployeeTypeDto
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string TypeName { get; set; }
    }
}

using Sprout.Exam.Common.Enums;
using System;

namespace Sprout.Exam.WebApp.Salary
{
    public class Salary
    {
        public static ISalary Compute(EmployeeTypeEnum employeeType)
        {
            switch (employeeType)
            {
                case EmployeeTypeEnum.Regular:
                    return new RegularSalary();

                case EmployeeTypeEnum.Contractual:
                    return new ContractualSalary();

                default:
                    throw new NotSupportedException(
                       $"{employeeType} is not currently Supported as a employee type" 
                        );
            }
        }

    }
}

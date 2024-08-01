namespace Sprout.Exam.WebApp.Salary
{
    public class RegularSalary : ISalary
    {

        public decimal NetIncome(SalaryParams salaryParams)
        {
            decimal workingDays = 22m;
            decimal monthlySalary = 20000m;
            decimal tax = 0.12m;
            decimal netIncome = 0;
            netIncome = monthlySalary - ((monthlySalary / workingDays) * salaryParams.absentDays) - (monthlySalary * tax);
            return netIncome;
        }
    }
}

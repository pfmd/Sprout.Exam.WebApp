namespace Sprout.Exam.WebApp.Salary
{
    public class ContractualSalary : ISalary
    {
        public decimal NetIncome(SalaryParams salaryParams)
        {
            decimal netIncome = 0;

            decimal dailyRate = 500m;
            netIncome = dailyRate * salaryParams.workedDays;

            return netIncome;
        }
    }
}

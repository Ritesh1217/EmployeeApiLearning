namespace EmployeeApiLearning.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        public string GenerateEmployeeCode(int number)
        {
            return $"EMP{number:D5}";
        }
    }
}

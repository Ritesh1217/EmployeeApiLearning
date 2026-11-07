namespace EmployeeApiLearning.DTO
{
    public class EmployeeQueryParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Department { get; set; }
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; } = "Name";
        public bool IsDescending { get; set; } = false;
    }
}

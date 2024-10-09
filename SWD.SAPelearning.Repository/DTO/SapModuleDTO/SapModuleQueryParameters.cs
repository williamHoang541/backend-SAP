

namespace SWD.SAPelearning.Repository.DTO.SapModuleDTO
{
    public class SapModuleQueryParameters
    {
        

        public string SearchTerm { get; set; } // Search term for module name
        public string SortBy { get; set; } // Column to sort by
        public bool IsDescending { get; set; } = false; // Sort direction
        public bool? Status { get; set; } // Filter by status (active/inactive)

        const int maxPageSize = 50; // Maximum page size
        public int PageNumber { get; set; } = 1; // Default page number
        private int _pageSize = 10; // Default page size

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

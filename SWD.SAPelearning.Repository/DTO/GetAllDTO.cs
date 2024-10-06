namespace SWD.SAPelearning.Repository.DTO
{
    public class GetAllDTO
    {
        public string FilterOn { get; set; }
        public string FilterQuery { get; set; }
        public string SortBy { get; set; }
        public bool? IsAscending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

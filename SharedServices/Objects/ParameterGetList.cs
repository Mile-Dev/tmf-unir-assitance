namespace SharedServices.Objects
{
    public class ParameterGetList
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = string.Empty;   

        public string? SortOrder { get; set; } = string.Empty;
    }
}

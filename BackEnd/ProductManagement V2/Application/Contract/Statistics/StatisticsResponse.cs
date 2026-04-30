namespace ProductManagement_V2.Application.Contract.Statistics
{
    public class StatisticsResponse
    {
        public ProductStatisticsResponse Products { get; set; } = new();
        public StatusChangeStatisticsResponse StatusChanges { get; set; } = new();
        public UserStatisticsResponse Users { get; set; } = new();
    }

    public class ProductStatisticsResponse
    {
        public int Total { get; set; }
        public int Available { get; set; }
        public int OutOfStock { get; set; }
        public int Discontinued { get; set; }
    }

    public class StatusChangeStatisticsResponse
    {
        public int Total { get; set; }
    }

    public class UserStatisticsResponse
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
    }
}
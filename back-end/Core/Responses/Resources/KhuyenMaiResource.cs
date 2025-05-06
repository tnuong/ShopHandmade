namespace back_end.Core.Responses.Resources
{
    public class KhuyenMaiResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PromotionType { get; set; }   
        public double DiscountValue { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
    }
}

namespace back_end.Core.Requests
{
    public class PromotionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string PromotionType { get; set; }
        public bool IsActive { get; set; }
        public double DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

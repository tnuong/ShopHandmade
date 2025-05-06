namespace back_end.Core.Requests
{
    public class AssignPromotionRequest
    {
        public List<int> ProductIds { get; set; }
        public List<int> PromotionIds { get; set; }
    }
}

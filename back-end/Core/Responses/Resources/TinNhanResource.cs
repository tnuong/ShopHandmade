namespace back_end.Core.Responses.Resources
{
    public class TinNhanResource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public NguoiDungResource Sender { get; set; }
        public NguoiDungResource Recipient { get; set; }
        public DateTime SentAt { get; set; }
        public List<string> Images { get; set; }
    }

}

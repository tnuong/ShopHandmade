namespace back_end.Core.Responses.Resources
{
    public class ThongBaoResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ReferenceId { get; set; }
        public NguoiDungResource Recipient { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HaveRead { get; set; }
        public string NotificationType { get; set; }

    }
}

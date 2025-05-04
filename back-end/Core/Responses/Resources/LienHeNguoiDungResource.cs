namespace back_end.Core.Responses.Resources
{
    public class LienHeNguoiDungResource
    {
        public NguoiDungResource User { get; set; }
        public TinNhanResource Message { get; set; }
        public int TotalUnreadMessages { get; set; }
    }
}

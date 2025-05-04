namespace back_end.Core.Responses.Resources
{
    public class DanhMucResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DanhMucResource ParentCategory { get; set; }
    }
}

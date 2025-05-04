namespace back_end.Core.Responses.Resources
{
    public class PhanCapDanhMucResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PhanCapDanhMucResource> CategoryChildren { get; set; }
    }
}

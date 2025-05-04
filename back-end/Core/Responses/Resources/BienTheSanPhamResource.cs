namespace back_end.Core.Responses.Resources
{
    public class BienTheSanPhamResource
    {
        public int Id { get; set; }
        public int InStock { get; set; }
        public MauSacResource Color { get; set; }
        public KichThuocResource Size { get; set; }
        public SanPhamResource Product { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<HinhAnhSanPhamResource> Images { get; set; }
    }
}

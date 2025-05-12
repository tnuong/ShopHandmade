namespace back_end.Core.Responses.Resources
{
    public class SanPhamResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double OldPrice { get; set; }
        public double Price { get; set; }
        public double PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public DanhMucResource Category { get; set; }
        public ThuongHieuResource Brand { get; set; }
        public NhaSanXuatResource Manufacturer { get; set; }
        public string Thumbnail { get; set; }
        public string ZoomImage { get; set; }
        public bool HasWishlist { get; set; }
        public List<HinhAnhSanPhamResource> Images { get; set; }
        public List<KhuyenMaiResource> Promotions { get; set; } = new List<KhuyenMaiResource>();
    }
}

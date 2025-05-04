using back_end.Core.Responses.Resources;

namespace back_end.Core.Responses.Report
{
    public class ProductReport
    {
        public SanPhamResource Product { get; set; }
        public int Quantity { get; set; }
    }
}

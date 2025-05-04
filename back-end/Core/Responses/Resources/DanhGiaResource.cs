namespace back_end.Core.Responses.Resources
{
    public class DanhGiaResource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedAt { get; set; }
        public NguoiDungResource? User { get; set; }
        public int Favorites { get; set; }
        public bool IsFavoriteIncludeMe { get; set; }
    }

    public class PhanTichDanhGiaResource
    {
        public int TotalEvaluation { get; set; }
        public List<TiLeSao> StarsPercents { get; set; }
        public double AverageStar { get; set; }
    }

    public class TiLeSao
    {
        public int Star { get; set; }
        public int TotalEvaluation { get; set; }
        public double Percent { get; set; }
    }

    public class ReportEvaluationResource
    {
        public List<DanhGiaResource> Results { get; set; }
        public PhanTichDanhGiaResource Report { get; set; }
    }
}

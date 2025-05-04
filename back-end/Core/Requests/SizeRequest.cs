using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Requests
{
    public class SizeRequest
    {
        [Required(ErrorMessage = "Kích cỡ không được để trống")]
        public string ESize { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        
    }
}

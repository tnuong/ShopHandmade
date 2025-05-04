using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Requests
{
    public class CreateSupplierRequest
    {
        [Required(ErrorMessage = "Tên nhà sản xuất không được để trống")]
        [StringLength(100, ErrorMessage = "Tên nhà sản xuất không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 ký tự")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }
        public bool Status { get; set; } = true;
    }
}

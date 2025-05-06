namespace back_end.Core.Responses.Resources
{
    public class DonHangResource
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public double TotalPriceBeforeDiscount { get; set; }
        public double TotalPriceAfterDiscount { get; set; }
        public double TotalDiscount { get; set; }
        public string OrderStatus { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Note { get; set; }
        public List<OrderItemResource> Items { get; set; }
        public List<OrderProcessItem> OrderSteps { get; set; }
        public PaymentResource Payment { get; set; }
        public AddressOrderResource AddressOrder { get; set; }
        public NguoiDungResource User { get; set; }

    }

    public class OrderItemResource
    {
        public int? Id { get; set; }
        public double? Price { get; set; }
        public double SubTotalBeforeDiscount { get; set; }
        public double SubTotalAfterDiscount { get; set; }
        public double SubTotalDiscount { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public double? ProductPrice { get; set; }
        public BienTheSanPhamResource? Variant { get; set; }

    }

    public class OrderProcessItem
    {
        public DateTime? ModifyAt { get; set; }
        public string OrderStatus { get; set; }
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
    }


    public class OrderHistoryResource
    {
        public int Id { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string OrderStatus { get; set; }
        public string? Note { get; set; }
    }

    public class PaymentResource
    {
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public string PaymentCode { get; set; }
    }

    public class AddressOrderResource
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}

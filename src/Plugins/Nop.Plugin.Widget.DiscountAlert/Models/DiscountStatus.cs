namespace Nop.Plugin.Widget.DiscountAlert.Models
{
    public record DiscountStatus
    {
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public string Currency { get; set; }
        public bool HaveDiscount { get; set; }
    }
}

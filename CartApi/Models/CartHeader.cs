using System.ComponentModel.DataAnnotations.Schema;

namespace CartApi.Models
{
    public class CartHeader
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        [NotMapped]
        public double Discount { get; set; }
        [NotMapped]
        public double Total { get; set; }

    }
}

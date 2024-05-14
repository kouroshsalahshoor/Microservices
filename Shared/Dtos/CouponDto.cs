using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        [Required] 
        public string Code { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double DiscountAmount { get; set; }
        [Range(0, int.MaxValue)]
        public int MinAmount { get; set; }
    }
}

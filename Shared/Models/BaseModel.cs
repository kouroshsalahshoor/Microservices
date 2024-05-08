namespace Shared.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; } = string.Empty;
        public DateTime LastEditedOn { get; set; }
    }
}

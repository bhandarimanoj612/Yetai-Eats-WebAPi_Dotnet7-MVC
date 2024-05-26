using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class PendingMessage
    {
        [Key]
        public string UserId { get; set; }
        public string Message { get; set; }

        public DateTime DateTime { get; set; }=DateTime.Now;
        public string Action { get; set; }//accepted or rejected
    }
}

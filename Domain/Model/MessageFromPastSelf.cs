using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace infrastructure
{
    [Table("messages")]

    public class MessageFromPastSelf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public string To { get; set; }
        public string Subject{ get; set; }
        public string Body { get; set; }
        public string jobid { get; set; }
        public int when { get; set; }
        public bool IsBodyHtml{ get; set; }
    }
}
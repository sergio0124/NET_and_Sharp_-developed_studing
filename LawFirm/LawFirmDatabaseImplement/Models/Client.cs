using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LawFirmDatabaseImplement.Models
{
    public class Client
    {
        public int Id { set; get; }
        [Required]
        public string ClientFIO { get; set; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Email { set; get; }
        [ForeignKey("ClientId")]
        public virtual List<Order> Orders { set; get; }
        [ForeignKey("ClientId")]
        public virtual List<MessageInfo> MessageInfos { set; get; }
    }
}

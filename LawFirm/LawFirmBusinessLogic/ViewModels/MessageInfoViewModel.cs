using LawFirmBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class MessageInfoViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public string MessageId { get; set; }
        [Column(title: "Отправитель", width: 150)]
        [DataMember]
        public string SenderName { get; set; }
        [Column(title: "Дата письма", width: 100,format:"D")]
        [DataMember]
        public DateTime DateDelivery { get; set; }
        [Column(title: "Заголовок", width: 150)]
        [DataMember]
        public string Subject { get; set; }
        [Column(title: "Текст", width: 150)]
        [DataMember]
        public string Body { get; set; }

    }
}

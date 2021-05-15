using LawFirmBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { set; get; }
        [Column(title: "ФИО клиента", width: 150)]
        [DataMember]
        public string ClientFIO { get; set; }
        [DataMember]
        [Column(title: "е-Почта", width: 150)]
        public string Email { get; set; }
        [DataMember]
        [Column(title: "Пароль", gridViewAutoSize:GridViewAutoSize.Fill)]
        public string Password { set; get; }
    }
}

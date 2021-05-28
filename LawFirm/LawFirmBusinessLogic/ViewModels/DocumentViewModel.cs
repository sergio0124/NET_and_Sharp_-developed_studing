using LawFirmBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class DocumentViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [Column(title: "Название изделия", width: 150)]
        public string DocumentName { get; set; }
        [DataMember]
        [Column(title: "Цена", width: 100)]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> DocumentBlanks { get; set; }
    }
}

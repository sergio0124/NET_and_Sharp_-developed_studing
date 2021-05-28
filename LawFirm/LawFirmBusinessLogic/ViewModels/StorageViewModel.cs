using LawFirmBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    [DataContract]
    public class StorageViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [Column(title: "Название", width: 100)]
        public string StorageName { get; set; }
        [DataMember]
        [Column(title: "ФИО ответственного", width: 100)]
        public string StorageManager { get; set; }
        [DataMember]
        [Column(title: "Дата создания", width: 100,format:"D")]
        public DateTime DateCreate { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> StorageBlanks { get; set; }
    }
}

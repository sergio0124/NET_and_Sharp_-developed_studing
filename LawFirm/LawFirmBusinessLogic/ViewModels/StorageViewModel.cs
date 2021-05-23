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
        [DisplayName("Название")]
        public string StorageName { get; set; }
        [DataMember]
        [DisplayName("ФИО ответственного")]
        public string StorageManager { get; set; }
        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> StorageBlanks { get; set; }
    }
}

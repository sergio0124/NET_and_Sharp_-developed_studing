using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using LawFirmBusinessLogic.Attributes;

namespace LawFirmBusinessLogic.ViewModels
{
    public class BlankViewModel
    {
        [Column(title: "Номер", width: 100)]
        public int Id { get; set; }
        [Column(title: "Название бланка", width: 100)]
        public string BlankName { get; set; }
    }
}

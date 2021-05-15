using LawFirmBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    public class ImplementerViewModel
    {
        [Column(title: "Номер", width: 100)]
        public int Id { get; set; }
        [Column(title: "Исполнитель", width: 150)]
        public string ImplementerFIO { get; set; }
        [Column(title: "Время на заказ", width: 50)]
        public int WorkingTime { get; set; }
        [Column(title: "Время на перерыв", width: 50)]
        public int PauseTime { get; set; }
    }
}

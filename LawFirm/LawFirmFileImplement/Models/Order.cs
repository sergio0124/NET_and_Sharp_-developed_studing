﻿using LawFirmBusinessLogic.Enums;
using System;
namespace LawFirmFileImplement.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public decimal Sum { set; get; }
        public int Count { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public int? ClientId { get; set; }
        public int? ImplementerId { set; get; }
    }
}
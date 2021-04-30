﻿using System;
using System.Collections.Generic;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Enums;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class OrderLogic
    {
        private readonly IOrderStorage _orderStorage;
        private readonly IStorageStorage _storageStorage;
        private readonly IDocumentStorage _documentStorage;
        public OrderLogic(IOrderStorage orderStorage, IDocumentStorage documentStorage, IStorageStorage storageStorage)
        {
            _storageStorage = storageStorage;
            _documentStorage = documentStorage;
            _orderStorage = orderStorage;
        }
        public List<OrderViewModel> Read(OrderBindingModel model)
        {
            if (model == null)
            {
                return _orderStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<OrderViewModel> { _orderStorage.GetElement(model) };
            }

            return _orderStorage.GetFilteredList(model);
        }
        public void CreateOrder(CreateOrderBindingModel model)
        {
            _orderStorage.Insert(new OrderBindingModel
            {
                Sum = model.Sum,
                DocumentId = model.DocumentId,
                Count = model.Count,
                DateCreate = DateTime.Now,
                Status = OrderStatus.Принят
            });
        }
        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id =
           model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            if (!_storageStorage.TakeFromStorage(_documentStorage.GetElement(new DocumentBindingModel { Id = order.DocumentId }).DocumentBlanks, order.Count))
            {
                throw new Exception("Недостаточно бланков для заказа");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                Sum = order.Sum,
                DocumentId = order.DocumentId,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = DateTime.Now,
                Status = OrderStatus.Выполняется
            });
        }
        public void FinishOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id =
           model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                Sum = order.Sum,
                DocumentId = order.DocumentId,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Готов
            });
        }
        public void PayOrder(ChangeStatusBindingModel model)
        {
            var order = _orderStorage.GetElement(new OrderBindingModel
            {
                Id = model.OrderId
            });
            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }
            if (order.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                Id = order.Id,
                DocumentId = order.DocumentId,
                Sum = order.Sum,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Оплачен
            });
        }
    }
}

using System;
using System.Collections.Generic;
using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Enums;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class OrderLogic
    {
        private readonly object locker = new object();
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
                ClientId=model.ClientId,
                Sum = model.Sum,
                DocumentId = model.DocumentId,
                Count = model.Count,
                DateCreate = DateTime.Now,
                Status = OrderStatus.Принят
            });
        }
        public void TakeOrderInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                OrderStatus status = OrderStatus.Выполняется;
                var order = _orderStorage.GetElement(new OrderBindingModel
                {
                    Id = model.OrderId
                });
                if (order == null)
                {
                    throw new Exception("Не найден заказ");
                }
                if (order.Status != OrderStatus.Принят && order.Status!=OrderStatus.Требуются_бланки)
                {
                    throw new Exception("Заказ не в статусе \"Принят\" или \"Требуются_бланки\"");
                }
                if (order.ImplementerId.HasValue && order.Status != OrderStatus.Требуются_бланки)
                {
                    throw new Exception("У заказа уже есть исполнитель");
                }

                if (!_storageStorage.CheckBlanks(_documentStorage.GetElement(new DocumentBindingModel { Id = order.DocumentId }), order.Count))
                {
                    status = OrderStatus.Требуются_бланки;
                }
                _orderStorage.Update(new OrderBindingModel
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    ImplementerId = model.ImplementerId,
                    DocumentId = order.DocumentId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    DateImplement = DateTime.Now,
                    Status = status
                });
            }
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
            if (order.Status == OrderStatus.Требуются_бланки && _storageStorage.CheckBlanks(_documentStorage.GetElement(new DocumentBindingModel { Id = order.DocumentId }), order.Count))
            {
                order.Status = OrderStatus.Выполняется;
            }
            if (order.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            _orderStorage.Update(new OrderBindingModel
            {
                ClientId = order.ClientId,
                Id = order.Id,
                Sum = order.Sum,
                DocumentId = order.DocumentId,
                ImplementerId = order.ImplementerId,
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
                ClientId = order.ClientId,
                Id = order.Id,
                DocumentId = order.DocumentId,
                ImplementerId = order.ImplementerId,
                Sum = order.Sum,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = OrderStatus.Оплачен
            });
        }
    }
}

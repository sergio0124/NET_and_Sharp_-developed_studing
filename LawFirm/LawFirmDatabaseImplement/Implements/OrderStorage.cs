using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.Enums;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LawFirmDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public void Delete(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                Order element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Orders.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<OrderViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                //List<Order> Ord = context.Orders
                //    .ToList();
                //foreach (var ord in Ord)
                //{
                //    context.Remove(ord);
                //}
                //context.SaveChanges();
                return context.Orders
                    .Include(rec => rec.Document)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .Select(CreateModel).ToList();
            }
        }
        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                DocumentId = order.DocumentId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                ClientFIO = order.Client.ClientFIO,
                DocumentName = order.Document.DocumentName,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order?.DateImplement,
                ImplementerFIO = order.ImplementerId.HasValue ?
                    order.Implementer.ImplementerFIO : string.Empty
            };
        }
        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                return context.Orders
                    .Include(rec => rec.Document)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .Where(rec => (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    rec.DateCreate.Date == model.DateCreate.Date) ||
                     (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                     (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value == true && (rec.Status ==
                    OrderStatus.Принят || rec.Status ==
                    OrderStatus.Требуются_бланки)) ||
                     (model.ImplementerId.HasValue && rec.ImplementerId ==
                    model.ImplementerId && rec.Status == OrderStatus.Выполняется))
                    .Select(CreateModel).ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new LawFirmDatabase())
            {
                var order = context.Orders
                    .Include(rec => rec.Document)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .FirstOrDefault(rec => rec.Id == model.Id);

                return order != null ?
                    CreateModel(order) :
                    null;
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Orders.Add(CreateModel(model, new Order(), context));
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order, LawFirmDatabase context)
        {
            order.DocumentId = model.DocumentId;
            order.ClientId = (int)model.ClientId;
            order.ImplementerId = model.ImplementerId;
            order.Sum = model.Sum;
            order.Count = model.Count;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new LawFirmDatabase())
            {
                var element = context.Orders.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
            }
        }
    }
}

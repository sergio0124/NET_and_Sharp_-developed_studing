using LawFirmBusinessLogic.BindingModels;
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

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new LawFirmDatabase())
            {
                var order = context.Orders
               .FirstOrDefault(rec => rec.Id
               == model.Id || rec.ClientId==model.ClientId);
                return order != null ?
                new OrderViewModel
                {
                    Id = order.Id,
                    DocumentName = order.Document?.DocumentName,
                    DocumentId = order.DocumentId,
                    ClientFIO = order.Client?.ClientFIO,
                    ClientId = order.ClientId,
                    Count = order.Count,
                    Sum = order.Sum,
                    Status = order.Status,
                    DateCreate = order.DateCreate,
                    DateImplement = order.DateImplement
                } :
               null;
            }
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
               .Where(rec => rec.Id==model.Id || rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo || rec.ClientId == model.ClientId)
               .Include(rec=>rec.Document)
               .Select(rec => new OrderViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.Document.DocumentName,
                   DocumentId = rec.DocumentId,
                   ClientFIO = rec.Client.ClientFIO,
                   ClientId = rec.ClientId,
                   Count = rec.Count,
                   Sum = rec.Sum,
                   Status = rec.Status,
                   DateCreate = rec.DateCreate,
                   DateImplement = rec.DateImplement
               })
               .ToList();
            }
        }

        public List<OrderViewModel> GetFullList()
        {
            using (var context = new LawFirmDatabase())
            {
                return context.Orders
               .Select(rec => new OrderViewModel
               {
                   Id = rec.Id,
                   DocumentName = rec.Document.DocumentName,
                   DocumentId = rec.DocumentId,
                   ClientFIO = rec.Client.ClientFIO,
                   Count = rec.Count,
                   Sum = rec.Sum,
                   Status = rec.Status,
                   DateCreate = rec.DateCreate,
                   DateImplement = rec.DateImplement,
                   ClientId = rec.ClientId,
               })
               .ToList();
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

using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using LawFirmBusinessLogic.BindingModels;
using LawFirmListImplement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LawFirmListImplement.Models;

namespace LawFirmListImplements.Implements
{
    public class MessageInfoStorage: IMessageInfoStorage
    {
        private readonly DataListSingleton source;
        public MessageInfoStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<MessageInfoViewModel> GetFullList()
        {
            return source.MessageInfos.Select(CreateModel).ToList();
        }

        private MessageInfoViewModel CreateModel(MessageInfo messageInfo)
        {
            return new MessageInfoViewModel
            {
                DateDelivery = messageInfo.DateDelivery,
                Body = messageInfo.Body,
                MessageId = messageInfo.MessageId,
                SenderName = messageInfo.SenderName,
                Subject = messageInfo.Subject
            };
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.MessageInfos.Where(rec =>
                rec.DateDelivery == model.DateDelivery || rec.SenderName.Contains(model.FromMailAddress) || rec.Subject.Contains(model.Subject) || rec.Body.Contains(model.Body)).Select(CreateModel).ToList();
        }
        public void Insert(MessageInfoBindingModel model)
        {
            MessageInfo element = source.MessageInfos.FirstOrDefault(rec =>
               rec.MessageId == model.MessageId);
            if (element != null)
            {
                throw new Exception("Уже есть письмо с таким идентификатором");
            }
            source.MessageInfos.Add(new MessageInfo
            {
                MessageId = model.MessageId,
                ClientId = model.ClientId,
                SenderName = model.FromMailAddress,
                DateDelivery = model.DateDelivery,
                Subject = model.Subject,
                Body = model.Body
            });
        }
    }
}

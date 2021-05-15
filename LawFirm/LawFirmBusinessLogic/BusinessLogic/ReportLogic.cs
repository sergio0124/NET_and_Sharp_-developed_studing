using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.HelperModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class ReportLogic
    {

        private readonly IBlankStorage _blankStorage;
        private readonly IDocumentStorage _documentStorage;
        private readonly IOrderStorage _orderStorage;
        private readonly IStorageStorage _storageStorage;
        public ReportLogic(IDocumentStorage documentStorage, IBlankStorage
       blankStorage, IOrderStorage orderStorage, IStorageStorage storageStorage)
        {
            _documentStorage = documentStorage;
            _blankStorage = blankStorage;
            _orderStorage = orderStorage;
            _storageStorage = storageStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>104
        public List<ReportDocumentBlankViewModel> GetDocumentBlanks()
        {
            var blanks = _blankStorage.GetFullList();
            var documents = _documentStorage.GetFullList();
            var list = new List<ReportDocumentBlankViewModel>();
            foreach (var blank in blanks)
            {
                var record = new ReportDocumentBlankViewModel
                {
                    BlankName = blank.BlankName,
                    Documents = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var document in documents)
                {
                    if (document.DocumentBlanks.ContainsKey(blank.Id))
                    {
                        record.Documents.Add(new Tuple<string, int>(document.DocumentName,
                       document.DocumentBlanks[blank.Id].Item2));
                        record.TotalCount +=
                       document.DocumentBlanks[blank.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }

        public List<OrderReportByDateViewModel> GetOrderReportByDate()
        {
            return _orderStorage.GetFullList()
               .GroupBy(order => order.DateCreate.ToShortDateString())
               .Select(rec => new OrderReportByDateViewModel
               {
                   Date = Convert.ToDateTime(rec.Key),
                   Count = rec.Count(),
                   Sum = rec.Sum(order => order.Sum)
               })
               .ToList();
        }

        public void SaveOrderReportByDateToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDocOrderReportByDate(new PdfInfoOrderReportByDate
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrderReportByDate()
            });
        }

        public void SaveStorageBlanksToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateStoragesDoc(new StorageExcelInfo
            {
                FileName = model.FileName,
                Title = "Список загруженности складов",
                StorageBlanks = GetStorageBlanks()
            });
        }

        /// <summary>
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom =
           model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                DocumentName = x.DocumentName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
           .ToList();
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveDocumentsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список документов",
                Documents = _documentStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveDocumentBlanksToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонент",
                DocumentBlanks = GetDocumentBlanks()
            });
        }

        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        [Obsolete]
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }

        public void SaveStoragesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateStoragesDoc(new StorageWordInfo
            {
                FileName = model.FileName,
                Title = "Список складов",
                Storages = _storageStorage.GetFullList()
            });
        }

        public List<ReportStorageBlanksViewModel> GetStorageBlanks()
        {
            var storages = _storageStorage.GetFullList();
            var list = new List<ReportStorageBlanksViewModel>();
            foreach (var storage in storages)
            {
                var record = new ReportStorageBlanksViewModel
                {
                    StorageName = storage.StorageName,
                    Blanks = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var blank in storage.StorageBlanks)
                {
                    record.Blanks.Add(new Tuple<string, int>(blank.Value.Item1, blank.Value.Item2));
                    record.TotalCount += blank.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }
    }
}

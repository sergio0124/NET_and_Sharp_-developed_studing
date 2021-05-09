using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.HelperModels
{
    public class StorageExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportStorageBlanksViewModel> StorageBlanks { get; set; }
    }
}

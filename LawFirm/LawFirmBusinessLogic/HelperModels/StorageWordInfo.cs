using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.HelperModels
{
    public class StorageWordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<StorageViewModel> Storages { get; set; }
    }
}

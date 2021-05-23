using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.ViewModels
{
    public class ReportStorageBlanksViewModel
    {
        public string StorageName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Blanks { get; set; }
    }
}

using LawFirmBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LawFirmBusinessLogic.HelperModels
{
    public class MailCheckInfo
    {
        public string PopHost { get; set; }
        public int PopPort { get; set; }
        public IMessageInfoStorage Storage { get; set; }

    }
}

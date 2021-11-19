using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Utils
{
    internal static class LangHelper
    {
        internal static readonly Dictionary<string, string> LangTypeDictionary = new Dictionary<string, string>
        {
            {"String","Строка"},
            {"DateTime","Дата"},
            {"Int32","Число"},
            {"Int64","Число"},
            {"Format","Формат документа"},
        };

        internal static readonly Dictionary<string, string> AccountingTypes = new Dictionary<string, string>
        {
            {"AccountingForNotifications_Model","Извещение"},
            {"AccountingForOriginals_Model","Подлинник"},
        };
    }
}

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

        internal static readonly Dictionary<string, string> AccountingForNotification_ModelDictionary = new Dictionary<string, string>()
        {
            {"Id", "Id" },
            {"Date", "Дата"},
            {"Designation_Notice", "Обозначение Извещения"},
            {"Document_Change_Designation", "Обозначение Изменяемого документа" },
            {"Product_index", "Индекс изделия" },
            {"Change_litera", "Литера изменения"},
            {"Change_code", "Шифр изменения"},
            {"Number_of_sheets", "Количество листов"},
            {"Department_sector", "Отдел сектор" },
            {"Released_by", "Кем выпущен" },
            {"Change_time", "Срок изменения" },
            {"Rented", "Сдано" },
            {"Execution_Date", "Дата исполнения" },
        };

        internal static readonly Dictionary<string, string> AccountingForOriginals_ModelDictionary = new Dictionary<string, string>()
        {
            {"Id", "Id" },
            {"Inventory_Document_Number", "Инвентаризационный номер документа"},
            {"Registration_Date", "Дата регистрации"},
            {"Document_Name", "Обозначение документа" },
            {"Number_of_sheets", "Количество листов" },
            {"Document_format", "Формат документа"},
            {"Title_of_the_document", "Наименование документа"},
            {"Released_by", "Кем выпущен документ"},
            {"Note", "Примечание" },
        };
    }
}

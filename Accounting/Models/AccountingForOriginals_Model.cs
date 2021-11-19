using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Models
{
    internal class AccountingForOriginals_Model : IAccountingModel
    {
        public long Id { get; set; }
        public long Инвентаризационный_номер_документа {get;set;}
        public DateTime Дата_регистрации { get; set; }
        public string Обозначение_документа { get; set; }
        public int Количество_листов { get; set; }
        public Format Формат_документа { get; set; }
        public string Наименование_документа { get; set; }
        public string Кем_выпущен_документ { get; set; }
        public string Примечание { get; set; }
        long IAccountingModel.ID { get => Id; set => Id = value; }
    }

    internal enum Format { Неизвестно, A0, A1, A2, A3, A4, A5 }
}
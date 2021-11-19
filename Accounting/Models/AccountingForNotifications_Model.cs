using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Models
{
    internal class AccountingForNotifications_Model : IAccountingModel
    {
        public long Id { get; set; }
        public DateTime Дата { get; set; }
        public string Обозначение_Извещения { get; set; }
        public string Обозначение_Изменяемого_документа {get;set;}
        public string Индекс_изделия { get; set; }
        public long Литера_изменения { get; set; }
        public long Шифр_изменения { get; set; }
        public int Количество_листов { get; set; }
        public string Отдел_сектор { get; set; }
        public string Кем_выпущен { get; set; }
        public DateTime Срок_изменения { get; set; }
        public DateTime Сдано { get; set; }
        public DateTime Дата_исполнения { get; set; }
        long IAccountingModel.ID { get => Id; set => Id = value; }
    }
}

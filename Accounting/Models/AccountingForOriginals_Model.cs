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
        public long Inventory_Document_Number { get;set;}
        public DateTime Registration_Date { get; set; }
        public string Document_Name { get; set; }
        public int Number_of_sheets { get; set; }
        public Format Document_format { get; set; }
        public string Title_of_the_document { get; set; }
        public string Released_by { get; set; }
        public string Note { get; set; }
        long IAccountingModel.ID { get => Id; set => Id = value; }
    }

    internal enum Format { Неизвестно, A0, A1, A2, A3, A4, A5 }
}
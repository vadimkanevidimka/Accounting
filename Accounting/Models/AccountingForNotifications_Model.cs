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
        public DateTime Date { get; set; }
        public string Designation_Notice { get; set; }
        public string Document_Change_Designation { get;set;}
        public string Product_index { get; set; }
        public long Change_litera { get; set; }
        public long Change_code { get; set; }
        public int Number_of_sheets { get; set; }
        public string Department_sector { get; set; }
        public string Released_by { get; set; }
        public DateTime Change_time { get; set; }
        public DateTime Rented { get; set; }
        public DateTime Execution_Date { get; set; }
        long IAccountingModel.ID { get => Id; set => Id = value; }
    }
}

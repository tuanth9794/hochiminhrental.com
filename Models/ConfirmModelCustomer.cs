using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Models
{
    public class ConfirmModelCustomer
    {
        public int CustomerConfirm_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Phone { get; set; }
        public DateTime? CustomerConfirm_Date { get; set; }
        public int? CustomerConfirm_Active { get; set; }
        public DateTime? CustomerConfirm_PublishDate { get; set; }

        public string CustomerConfirm_Comment { get; set; }

        public string NEWS_TITLE { get; set; }

        public string NEWS_TITLE_EN { get; set; }



    }
}

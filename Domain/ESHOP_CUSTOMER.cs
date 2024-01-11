using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace CoreCnice.Domain
{
   public class ESHOP_CUSTOMER
    {
        [Key]
        public int CUSTOMER_ID { get; set; }
        public string CUSTOMER_FULLNAME { get; set; }
        public string CUSTOMER_UN { get; set; }
        public string CUSTOMER_PW { get; set; }
        public int? CUSTOMER_SEX { get; set; }
        public string CUSTOMER_ADDRESS { get; set; }
        public string CUSTOMER_PHONE1 { get; set; }

        public string CUSTOMER_PHONE2 { get; set; }
        public int? CUSTOMER_NEWSLETTER { get; set; }        
        public DateTime? CUSTOMER_PUBLISHDATE { get; set; }
        public DateTime? CUSTOMER_UPDATE { get; set; }
        public Guid? CUSTOMER_OID { get; set; }
        public int? CUSTOMER_SHOWTYPE { get; set; }
        public int? CUSTOMER_TOTAL_POINT { get; set; }
        public int? CUSTOMER_REMAIN { get; set; }
        public string CUSTOMER_FIELD1 { get; set; }
        public string CUSTOMER_IP { get; set; }

        public string CUSTOMER_NGANSACH { get; set; }

        public string CUSTOMER_KHUVU { get; set; }

        public string CUSTOMER_LOAICANHO { get; set; }

        public string CUSTOMER_THOIGIANTHUE { get; set; }
        public string CUSTOMER_SONGUOIO { get; set; }

        public string CUSTOMER_EMAIL { get; set; }

        public string CUSTOMER_FIELD3 { get; set; }

        public string CUSTOMER_FIELD4 { get; set; }

        public int? CUSTOMER_ACTIVE { get; set; }



        public int? USER_ID { get; set; }

        public int? USER_ID_PHUTRACH { get; set; }

        public int? CUSTOMER_HIDEN { get; set; }
    }
}

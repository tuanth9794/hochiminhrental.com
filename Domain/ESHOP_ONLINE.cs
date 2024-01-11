using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CoreCnice.Domain
{
    public class ESHOP_ONLINE
    {
        [Key]
        public int ONLINE_ID { get; set; }
        public string ONLINE_NICKNAME { get; set; }
        public string ONLINE_DESC { get; set; }
        public string ONLINE_IMAGE { get; set; }
        public int ONLINE_ORDER { get; set; }
        public int ONLINE_TYPE { get; set; }
        public int? CUS_ID { get; set; }      
        public DateTime? ONLINE_OUT_DATE { get; set; } // Thời gian ra dùng tính khoảng thời gian ở lại tính tỷ lệ thoát
        public DateTime? ONLINE_START_BEGIN_DATE { get; set; } // Thời gian vào web
        public string ONLINE_DESC_EN { get; set; }
        public string ONLINE_IP { get; set; } // Xác định IP truy cập website
        public string ONLINE_LOCATION { get; set; } // Nơi đang truy cập
        public string ONLINE_COMBACK { get; set; } // Quay lại lần nữa
        public string ONLINE_LINK { get; set; } // các link đã xem trong 1 lần truy cập
        public int? ONLINE_ACTIVE { get; set; } // tình trạng còn online hay out 
        public Guid? ONLINE_GUID { get; set; } // Cooke lần đầu tiên truy câp vào website
    }
}

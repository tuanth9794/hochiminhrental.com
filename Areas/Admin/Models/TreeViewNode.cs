using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCnice.Areas.Admin.Models
{
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public JsTreeAttribute state;
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
        public bool opened;
    }

}

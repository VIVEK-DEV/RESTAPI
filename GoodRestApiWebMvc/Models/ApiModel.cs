using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodRestApiWebMvc.Models
{
    public class ApiModel
    {
    }
    public class RootObject
    {
        public string id { get; set; }
        public DateTime stamp { get; set; }
        public string text { get; set; }
    }
}
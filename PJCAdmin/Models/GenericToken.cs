using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PJCAdmin.Models
{
    public partial class Token<T>
    {
        public string token;
        public T obj;
    }
}
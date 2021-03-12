using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasitPortal
{
    public class User
    {
        public string userName { get; set; }

        public string userPassword { get; set; }

        public bool? isActive { get; set; }
    }
}
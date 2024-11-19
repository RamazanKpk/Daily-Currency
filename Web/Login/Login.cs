using Entitiy.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Login
{
    public class Login
    {
        public static bool aktifVarmi
        {
            get { return aktifKul != null; }
        }
        public static User aktifKul
        {
            get
            {
                return (User)System.Web.HttpContext.Current.Session["login"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["login"] = value;
            }
        }
        public static void cikisYap()
        {
            aktifKul = null;
            System.Web.HttpContext.Current.Session.Abandon();
        }
    }
}
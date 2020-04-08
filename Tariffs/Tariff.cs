using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tariffs
{
    public class Tariff
    {
        public String oper;
        public String name;
        public String mins;
        public String sms;
        public String mms;
        public String inet;
        public String pay;

        public static Tariff CreateTariff(String myOper, String myName, String myMins, String mySms,
                                          String myMms,  String myInet, String myPay)
        {
            return new Tariff()
            {
                oper = myOper,
                name = myName,
                mins = myMins,
                sms = mySms,
                mms = myMms,
                inet = myInet,
                pay = myPay
            };
        }
    }
}

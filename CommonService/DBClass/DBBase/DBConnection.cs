using Microsoft.Extensions.Configuration;
using System;

namespace CommonService.DBClass.DBBase
{

    public class DBConnection : DB_Base
    {
        public virtual IConfiguration _Configuration { get; set; }

        public void Connection(string dbName)
        {
            #if (DEBUG)
            String ip = _Configuration[dbName + ":publicip"];
            #else
            String ip = _Configuration[dbName + ":privateip"];
            #endif

            String user = _Configuration[dbName + ":user"];
            String pwd = _Configuration[dbName + ":pwd"];
            Connection(ip, user, pwd);
        }
    }
}

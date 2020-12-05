using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Services
{
    public class TimingService
    {
        private DateTime _stratTime;
        private DateTime _endtime;
        public int _useSecond => (_endtime - _stratTime).Milliseconds;

        public void start() {
            _stratTime = DateTime.Now;
        }
        public void end()
        {
            _endtime = DateTime.Now;
        }
    }
}

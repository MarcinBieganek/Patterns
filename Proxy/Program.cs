using System;
using System.Collections.Generic;

namespace Zadanie_3
{
    public interface IAirport
    {
        Plain AcquireReusable();
        void ReleaseReusable(Plain plain);
    }
    public class Plain
    {

    }
    public class Airport : IAirport
    {
        private int _capacity;
        private List<Plain> _ready = new List<Plain>();
        private List<Plain> _realeased = new List<Plain>();
        public Airport(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException();
            }

            this._capacity = capacity;
        }
        public Plain AcquireReusable()
        {
            if (_realeased.Count == this._capacity)
            {
                throw new ArgumentException();
            }
            if (_ready.Count == 0)
            {
                Plain newPlain = new Plain();
                _ready.Add(newPlain);
            }
            var plain = _ready[0];
            _ready.Remove(plain);
            _realeased.Add(plain);

            return plain;
        }

        public void ReleaseReusable(Plain plain)
        {
            if (!_realeased.Contains(plain))
            {
                throw new ArgumentException();
            }
            _realeased.Remove(plain);
            _ready.Add(plain);
        }
    }

    public class AirportTimeProxy : IAirport
    {
        private Airport _airport;
        private TimeSpan _from;
        private TimeSpan _to;

        public AirportTimeProxy(int capacity)
        {
            _airport = new Airport(capacity);
            _from    = new TimeSpan(8, 0, 0);
            _to      = new TimeSpan(22, 0, 0);
        }
        public Plain AcquireReusable()
        {
            EnsureTimeOk();

            return _airport.AcquireReusable();
        }

        public void ReleaseReusable(Plain plain)
        {
            EnsureTimeOk();
            
            _airport.ReleaseReusable(plain);
        }

        private void EnsureTimeOk()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now < _from || _to < now)
            {
                throw new TimeoutException();
            }
        }
    }

    public class AirportLogProxy : IAirport
    {
        private Airport _airport;
        public List<string> log;

        public AirportLogProxy(int capacity)
        {
            _airport = new Airport(capacity);
            log = new List<string>();
        }
        public Plain AcquireReusable()
        {
            LogStart("AcquireReusable");
            Plain res =  _airport.AcquireReusable();
            LogEnd("AcquireReusable", res);
            return res;
        }

        public void ReleaseReusable(Plain plain)
        {
            LogStart("ReleaseReusable", plain);
            _airport.ReleaseReusable(plain);
            LogEnd("ReleaseReusable");
        }

        private void LogStart(params object[] args)
        {
            string elem = "START:";
            elem += "   date: " + DateTime.Now;
            elem += "    method: " + args[0];
            elem += "   args :";
            for (int i=1; i<args.Length; i++)
                elem += " " + args[i];
            log.Add(elem);
        }

        private void LogEnd(params object[] args)
        {
            string elem  = "END: ";
            elem        += "    date: " + DateTime.Now;
            elem        += "    method: " + args[0];
            elem        += "   result :";
            if (args.Length > 1)
                elem    += " " + args[1];
            log.Add(elem);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AirportTimeProxy atimeproxy = new AirportTimeProxy(10);
            Plain p = atimeproxy.AcquireReusable();

            Console.WriteLine("Plain taken!");

            atimeproxy.ReleaseReusable(p);

            Console.WriteLine("Plain released!");



            AirportLogProxy alogproxy = new AirportLogProxy(10);
            Plain p1 = alogproxy.AcquireReusable();
            Plain p2 = alogproxy.AcquireReusable();

            alogproxy.ReleaseReusable(p1);
            alogproxy.ReleaseReusable(p2);

            Console.WriteLine();
            Console.WriteLine("Log:");

            foreach(var s in alogproxy.log)
            {
                Console.WriteLine(s);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Zadanie_3
{
    public class Plain
    {

    }
    public class Airport
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
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}

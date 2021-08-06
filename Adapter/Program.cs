using System;
using System.Collections;

namespace Zadanie_4
{
    public class IntSorter : IComparer
    {
        Comparison<int> comparison;

        public IntSorter(Comparison<int> comp)
        {
            this.comparison = comp;
        }
        int IComparer.Compare(object x, object y)
        {
            return this.comparison((int) x, (int) y);
        }
    }
    class Program
    {
        /* this is the Comparison<int> to be converted */

        static int IntComparer( int x, int y )
        {
            return x.CompareTo( y );
        }

        static void Main( string[] args )
        {
            ArrayList a = new ArrayList() { 1, 5, 3, 3, 2, 4, 3 };

            a.Sort( new IntSorter(IntComparer) );

            foreach(var item in a )
            {
                Console.Write(item + " ");
            }

        }
    }
}

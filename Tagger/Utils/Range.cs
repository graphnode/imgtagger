using System;
using System.Collections.Generic;
using System.Text;

namespace Tagger.Utils
{
    public struct Range
    {
        public int Maximum, Minimum;

        public int Difference
        {
            get { return this.Maximum - this.Minimum; }
        }

        public Range(int val1, int val2)
        {
            this.Minimum = Math.Min(val1, val2);
            this.Maximum = Math.Max(val1, val2);
        }

        public override string ToString()
        {
            return "[Range:(" + this.Minimum + "<->" + this.Maximum + ")]";
        }
    }
}

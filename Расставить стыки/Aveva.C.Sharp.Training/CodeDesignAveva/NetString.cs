using Aveva.Core.PMLNet;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Aveva.C.Sharp.Training
{
    [PMLNetCallable()]
    class NetString
    {
        
        private string mval;
        [PMLNetCallable()]
        public NetString()
        {
            mval = "";
        }
        [PMLNetCallable()]
        public NetString(string val)
        {
            mval = val;
        }
        [PMLNetCallable()]
        public void Assign(NetString that)
        {
            mval = that.mval;
        }

        [PMLNetCallable()]
        public string Val
        {
            get { return mval; }
            set { mval = value; }
        }
        [PMLNetCallable()]
        public string Append(string val)
        {
            mval = mval + val;
            return mval;
        }
        [PMLNetCallable()]
        public double Length()
        {
            return mval.Length;
        }
        public void Reset()
        {
            mval = "";
        }
    }
}

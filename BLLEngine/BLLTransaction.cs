using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLEngine
{
    public class BLLTransaction : LinKin.DALEngine.DALTranction
    {
        public BLLTransaction(IsolationLevel iso)
            : base(iso)
        {

        }

        public BLLTransaction()
        {

        }
    }
}

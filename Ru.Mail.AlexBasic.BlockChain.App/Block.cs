using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Mail.AlexBasic.BlockChain.App
{
    public class Block
    {
        public int Index { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string PreviousBlockHash { get; set; }
        public byte[] Data { get; set; }
        public int Proof { get; set; }
        //public string Hash { get; set; }
    }
}

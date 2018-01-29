using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Currencies
{
    public class IOTA
    {
        public float Btc { get; set; }

        public decimal Usd { get; set; }

        public decimal Eur { get; set; }

        public decimal Eth { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

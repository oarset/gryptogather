using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Currencies
{
    public class BTC
    {
        public float Btc { get; set; }

        public float Usd { get; set; }

        public float Eur { get; set; }

        public float Eth { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

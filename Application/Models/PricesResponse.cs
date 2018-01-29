using Application.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PricesResponse
    {
        public ETH ETH { get; set; }

        public BTC BTC { get; set; }

        public IOTA IOTA { get; set; }
    }
}

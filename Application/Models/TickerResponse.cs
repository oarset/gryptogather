using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TickerResponse
    {
        public DateTime TimeStamp { get; set; }

        public double Bid { get; set; }

        public double BidSize { get; set; }

        public double Ask { get; set; }

        public double AskSize { get; set; }

        public double DailyChange { get; set; }

        public double DailyChangePerc { get; set; }

        public double LastPrize { get; set; }

        public double Volume { get; set; }

        public double High { get; set; }

        public double Low { get; set; }
    }
}

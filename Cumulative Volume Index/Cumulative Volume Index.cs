// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
//    Cumulative Volume Index (CVI) is a momentum indicator that gauges the movement of funds into and out of the entire stock market by computing the difference between advancing and declining stocks as a running total.
//
// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CumulativeVolumeIndex : Indicator
    {
        private Bars[] _stockBars;

        [Parameter("Stock Symbols", DefaultValue = "AAPL,TSLA,AMZN,MSFT")]
        public string StockSymbols { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _stockBars = StockSymbols.Split(',').Select(symbol => MarketData.GetBars(TimeFrame, symbol)).ToArray();
        }

        public override void Calculate(int index)
        {
            if (_stockBars == null || _stockBars.Length == 0 || _stockBars.Any(bars => bars == null)) return;

            var advancingStocks = 0;
            var decliningStocks = 0;

            foreach (var stockBars in _stockBars)
            {
                var stockBarIndex = stockBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

                if (stockBarIndex == 0) continue;

                if (stockBars.ClosePrices[stockBarIndex] > stockBars.ClosePrices[stockBarIndex - 1])
                {
                    advancingStocks++;
                }
                else
                {
                    decliningStocks++;
                }
            }

            var diff = advancingStocks - decliningStocks;

            Result[index] = index == 0 ? diff : Result[index - 1] + diff;
        }
    }
}
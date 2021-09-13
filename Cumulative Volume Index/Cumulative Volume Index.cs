// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//
//    Cumulative Volume Index (CVI) is a momentum indicator that gauges the movement of funds into and out of the entire stock market by computing the difference between advancing and declining stocks as a running total.
//
// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CumulativeVolumeIndex : Indicator
    {
        private Bars _advancingBars, _decliningBars;

        [Parameter("Advancing Symbol", DefaultValue = "EURUSD")]
        public string AdvancingSymbol { get; set; }

        [Parameter("Declining Symbol", DefaultValue = "USDCAD")]
        public string DecliningSymbol { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _advancingBars = MarketData.GetBars(TimeFrame, AdvancingSymbol);
            _decliningBars = MarketData.GetBars(TimeFrame, DecliningSymbol);
        }

        public override void Calculate(int index)
        {
            if (_advancingBars == null || _decliningBars == null) return;

            var advancingBarsIndex = _advancingBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);
            var decliningBarsIndex = _decliningBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

            var diff = _advancingBars.ClosePrices[advancingBarsIndex] - _decliningBars.ClosePrices[decliningBarsIndex];

            Result[index] = index == 0 ? diff : Result[index - 1] + diff;
        }
    }
}
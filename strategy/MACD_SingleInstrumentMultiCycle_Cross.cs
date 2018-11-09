using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiFeng
{
	class MACD_3cycle_Cross : Strategy
	{
		private MACD macdMin1;
        private MACD macdMin5;
        private MACD macdMin15;
        private List<WMA> wmaMin15 = new List<WMA>();
        private List<WMA> wmaMin5 = new List<WMA>();
        private List<WMA> wmaMin1 = new List<WMA>();

        public override void Initialize()
		{
            string instrument = this.InstrumentID;
            int min1Index = 0;
            for(int i=0; i<this.Datas.Count; i++)
            {
                Data data = this.Datas[i];
                if (instrument != data.Instrument)
                {
                    throw new ArgumentException(String.Format("multi instruments exist. expect {0}, get {1}", instrument, data.Instrument));
                }
                if (data.IntervalType == EnumIntervalType.Min)
                {
                    switch (data.Interval)
                    {
                        case 1:
                            this.macdMin1 = MACD(data.C, 8, 15, 9);
                            min1Index = i;
                            this.wmaMin1.Add(WMA(data.C, 10));
                            break;
                        case 5:
                            this.macdMin5 = MACD(data.C, 8, 15, 9);
                            this.wmaMin5.Add(WMA(data.C, 10));
                            this.wmaMin5.Add(WMA(data.C, 26));
                            this.wmaMin5.Add(WMA(data.C, 55));
                            this.wmaMin5.Add(WMA(data.C, 135));
                            break;
                        case 15:
                            this.macdMin15 = MACD(data.C, 8, 15, 9);
                            this.wmaMin15.Add(WMA(data.C, 10));
                            this.wmaMin15.Add(WMA(data.C, 26));
                            this.wmaMin15.Add(WMA(data.C, 55));
                            this.wmaMin15.Add(WMA(data.C, 135));
                            break;
                        default:
                            break;
                    }
                }

            }
            if (this.macdMin15 == null || this.macdMin5 == null || this.macdMin1 == null)
            {
                throw new ArgumentException(String.Format("data series expect min1 5 15, exist min1= {0}, 5={1}, 15={2}", this.macdMin1 != null, this.macdMin5!= null, this.macdMin15 != null));
            }
            if (min1Index != 0)
            {
                Data firstold = this.Datas[1];
                this.Datas[1] = this.Datas[min1Index];
                this.Datas[min1Index] = firstold;
            }
		}

        private void buyLongInStrategy()
        {
            if (this.macdMin15.Diff[1] > this.macdMin15.Avg[1] &&
                this.macdMin5.Diff[1] > this.macdMin5.Avg[1])
            {
                foreach(WMA wma in this.wmaMin15)
                {
                    if (this.C[1] <=wma[1] )
                    {
                        return;
                    }
                }
                foreach (WMA wma in this.wmaMin5)
                {
                    if (this.C[1] <= wma[1])
                    {
                        return;
                    }
                }

            } else
            {
                return;
            }

            if (this.macdMin1.Diff.Count < 2)
            {
                return;
            }
            if (this.macdMin1.Diff[1] > this.macdMin1.Avg[1] && this.macdMin1.Diff[1] > 0 &&
                this.macdMin1.Diff[2] <= this.macdMin1.Avg[2])
            {
                this.Buy(1, this.C[1]);
            }

        }

        private void sellLongInStrategy()
        {
            if (this.macdMin1.Diff[1] <= this.macdMin1.Avg[1]) 
            {
                this.Sell(this.PositionLong, this.C[1]);
            }
            foreach(WMA wma in this.wmaMin1)
            {
                if (this.C[1] <= wma[1])
                {
                    this.Sell(this.PositionLong, this.C[1]);
                    break;
                }
            }
        }

		public override void OnBarUpdate()
		{
            this.buyLongInStrategy();
            this.sellLongInStrategy();
		}
	}
}

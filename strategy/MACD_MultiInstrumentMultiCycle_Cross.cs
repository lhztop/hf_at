using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiFeng
{
    /// <summary>
    /// 铁矿石和螺纹钢的对冲处理逻辑。
    /// 要求：
    /// 都是一分钟数据
    /// </summary>
	class MACD_2Instrument_1Cycle_Cross : MultiInstrumentStrategy
	{
		private MACD macdMinIron; //铁矿石
        private MACD macdMinRB; //螺纹钢
        private string ironInstrumentId;
        private string rbInstrumentId;

        public override void InitializeMultiAfter()
        {
            foreach(string instrument in this.instrumentMap.Keys)
            {
                if (instrument.StartsWith("rb"))
                {
                    this.rbInstrumentId = instrument;
                    this.macdMinRB = MACD(this.instrumentMap[instrument][0].C, 8, 15, 9);
                    List<Data> datas = this.instrumentMap[instrument];
                    for(int i=0; i< datas.Count; i++)
                    {
                        Data data = datas[i];
                        if (data.Interval != 1 || data.IntervalType != EnumIntervalType.Min)
                        {
                            datas.RemoveAt(i);
                            i--;
                        }
                    }
                    if (datas.Count <= 0)
                    {
                        throw new ArgumentException("rb not contains min 1 data");
                    }
                    continue;
                }
                if (instrument.StartsWith("i"))
                {
                    this.ironInstrumentId = instrument;
                    this.macdMinIron = MACD(this.instrumentMap[instrument][0].C, 8, 15, 9);
                    List<Data> datas = this.instrumentMap[instrument];
                    for (int i = 0; i < datas.Count; i++)
                    {
                        Data data = datas[i];
                        if (data.Interval != 1 || data.IntervalType != EnumIntervalType.Min)
                        {
                            datas.RemoveAt(i);
                            i--;
                        }
                    }
                    if (datas.Count <= 0)
                    {
                        throw new ArgumentException("iron not contains min 1 data");
                    }
                    continue;
                }
                if (this.macdMinIron != null && this.macdMinRB != null)
                {
                    break;
                }
            }
            if (this.macdMinIron == null || this.macdMinRB == null)
            {
                throw new ArgumentException("rbxxx or ixxx doesn't exist in strategy");
            }
            int ironIndex = 0;
            for(int i=0; i<this.Datas.Count; i++)
            {
                Data data = this.Datas[i];
                if (data.Instrument.StartsWith("i"))
                {
                    ironIndex = i;
                    break;
                }
            }
            if (ironIndex != 0)
            {
                Data oldfirst = this.Datas[0];
                this.Datas[0] = this.Datas[ironIndex];
                this.Datas[ironIndex] = oldfirst;
            }
        }

        private void buyLongInStrategy()
        {
            if (this.macdMinIron.Diff.Count < 2)
            {
                return;
            }
            if(this.macdMinRB.Diff[1] > this.macdMinRB.Diff[2])
            {
                this.Buy(this.ironInstrumentId, 1, this.InstrumentMap[this.ironInstrumentId][0].C[1]);
            }

        }

        private void sellLongInStrategy()
        {
            if (this.macdMinIron.Diff.Count < 1)
            {
                return;
            }
            if (this.macdMinIron.Diff[1] <= 0)
            {
                this.Sell(this.ironInstrumentId, 1, this.InstrumentMap[this.ironInstrumentId][0].C[1]);
            }
        }

		public override void OnBarUpdate()
		{
            this.buyLongInStrategy();
            this.sellLongInStrategy();
		}
	}
}

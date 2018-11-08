using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiFeng
{
	class MACD_3Instrument_3Cycle_Cross : MultiInstrumentStrategy
	{
		private MACD macdMin1;
        private MACD macdMin5;
        private MACD macdMin15;
        

        public override void Initialize()
		{
		}

        private void buyLongInStrategy()
        {

        }

        private void sellLongInStrategy()
        {

        }

		public override void OnBarUpdate()
		{
            this.buyLongInStrategy();
            this.sellLongInStrategy();
		}
	}
}

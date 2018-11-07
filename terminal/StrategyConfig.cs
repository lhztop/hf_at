
using System;
using System.Collections.Generic;
using System.Text;

namespace HaiFeng
{
    class StrategyConfig
    {
		public string Name { get; set; }
		public string TypeFullName { get; set; }
		public string Instrument { get; set; }
		public string InstrumentOrder { get; set; }
		public string Interval { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Params { get; set; }
	}

    class StrategyConfigNew
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public List<DataConfig> Datas { get; set; }
        public string Params { get; set; }
    }

    class DataConfig
    {
        public string Instrument { get; set; }
        public string InstrumentOrder { get; set; }
        public int Interval { get; set; }
        public EnumIntervalType IntervalType { get; set; }
        public InstrumentInfo InstrumentInfo { get; set; }
    }

}

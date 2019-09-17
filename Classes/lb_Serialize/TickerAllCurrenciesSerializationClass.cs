////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class TickerAllCurrenciesSerializationClass
    {
        [DataMember]
        public RatesClass rates;

        [DataMember]
        public string volume_btc;

        [DataMember]
        public string avg_1h;

        [DataMember]
        public string avg_6h;

        [DataMember]
        public string avg_12h;

        [DataMember]
        public string avg_24h;
    }
}

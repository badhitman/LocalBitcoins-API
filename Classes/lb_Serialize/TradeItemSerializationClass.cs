////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class TradeItemSerializationClass
    {
        [DataMember]
        public int tid;

        [DataMember]
        public int date;

        [DataMember]
        public string amount;

        [DataMember]
        public string price;
    }
}

////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class PaymentMethodsSerializationClass
    {
        [DataMember]
        public string code;

        [DataMember]
        public string name;

        [DataMember]
        public string[] currencies;
    }
}

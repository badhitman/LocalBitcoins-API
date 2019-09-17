////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class DataCountryCodes
    {
        [DataMember]
        public int cc_count;

        [DataMember]
        public string[] cc_list;
    }
}

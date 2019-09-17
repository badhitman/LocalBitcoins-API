////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class PaginationClass
    {
        [DataMember]
        public string next;

        [DataMember]
        public string prev;
    }
}

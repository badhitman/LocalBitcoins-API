////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class AdListBitcoinsOnlineSerializationClass
    {
        [DataMember]
        public PaginationClass pagination;

        [DataMember]
        public DataListWrap data;
    }
}

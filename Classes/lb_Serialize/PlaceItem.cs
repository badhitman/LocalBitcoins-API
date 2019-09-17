////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class PlaceItem
    {
        [DataMember]
        public string sell_local_url;

        [DataMember]
        public string location_string;

        [DataMember]
        public string url;

        [DataMember]
        public string lon;

        [DataMember]
        public string lat;

        [DataMember]
        public string buy_local_url;
    }
}

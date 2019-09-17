////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class UserProfileSerializationClass
    {
        [DataMember]
        public string username;

        [DataMember]
        public int feedback_score;

        [DataMember]
        public string trade_count;

        [DataMember]
        public string last_online;

        [DataMember]
        public string name;
    }
}

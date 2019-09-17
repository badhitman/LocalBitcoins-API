////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class ActionsClass
    {
        [DataMember]
        public string html_form;

        [DataMember]
        public string public_view;

        [DataMember]
        public string change_form;
    }
}

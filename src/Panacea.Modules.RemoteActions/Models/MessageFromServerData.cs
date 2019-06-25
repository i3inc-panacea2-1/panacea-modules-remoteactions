using System.Runtime.Serialization;

namespace Panacea.Modules.RemoteActions.Models
{
    [DataContract]
    public class MessageFromServerData
    {
        [DataMember(Name = "delay")]
        public int Delay { get; set; }

        [DataMember(Name = "logout")]
        public bool Logout { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.RemoteActions.Models
{
    [DataContract]
    public class MessageFromServer
    {
        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "data")]
        public MessageFromServerData Data { get; set; }
    }
}

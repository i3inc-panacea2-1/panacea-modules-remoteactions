using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.RemoteActions.Models
{
    [DataContract]
    public class Terminal
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "mac_addresses")]
        public List<string> MacAddresses { get; set; }

        [DataMember(Name = "terminalIp")]
        public string Ip { get; set; }
    }
}

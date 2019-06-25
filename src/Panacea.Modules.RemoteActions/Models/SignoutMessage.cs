using Panacea.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.RemoteActions.Models
{

    [DataContract]
    public class SignoutMessage
    {
        [DataMember(Name = "user")]
        public IUser User { get; set; }

        [DataMember(Name = "terminal")]
        public Terminal Terminal { get; set; }
    }

}

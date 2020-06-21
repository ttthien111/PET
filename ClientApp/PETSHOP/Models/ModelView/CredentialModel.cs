using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class CredentialModel
    {
        public string AccountUserName { get; set; }
        public string JwToken { get; set; }
        public string AccountId { get; set; }
        public UserProfile Profile { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Models
{
    public class Token
    {
        //Use this api to get the right algorithm if you want to make a token
        //=> such as: SecurityAlgorithms.HmacSha256Signature
        public string SecurityAlgorithm { get; set; }

        public int ExpireMinutes { get; set; }

        public Claim[] Claims { get; set; }
    }
}

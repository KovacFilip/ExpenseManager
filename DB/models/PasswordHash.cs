using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB.models
{
    public class PasswordHash
    {
        public int PersonId { get; set; }
        public string Hash { get; set; }
    }
}

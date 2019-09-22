using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Models
{
    public class AgentData
    {
        public string ID { get; set; }
        public DateTime LastSeen { get; set; }
        public string[] Issues { get; set; }
    }
}

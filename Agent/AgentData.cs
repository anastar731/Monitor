using System;
using System.Collections.Generic;
using System.Text;

namespace Agent
{
    public class AgentData
    {
        public string ID { get; set; }
        public DateTime LastSeen { get; set; }
        public string[] Issues { get; set; }
    }
}

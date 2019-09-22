using System;
using Monitor.Models;

namespace Monitor.Devices
{
    public class Agent
    {
        AgentData data;
        public Agent(AgentData _data)
        {
            data = _data;
        }
        public string ID { get { return data.ID; } }
        public bool isActive { get { return InactiveTime.TotalSeconds <= 30; } }
        public TimeSpan InactiveTime { get { return DateTime.Now - data.LastSeen; } }      
        public DateTime LastSeen { get { return data.LastSeen; } }
        public string[] Issues { get { return data.Issues; } }      
    }
}

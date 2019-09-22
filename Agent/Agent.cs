using System;
using System.Collections.Generic;
using System.Text;

namespace Agent
{
    class Agent
    {
        int ID;
        AgentData data = new AgentData();
        public Agent(int _ID)
        {
            ID = _ID;
            data.ID = ID.ToString();
        }
        public AgentData Data { get {return data;} }
        public void UpdateIssues()
        {
            Random random = new Random();
            List<string> Issues = new List<string>();
            int NumOfIssues = random.Next(10);
            for (int i = 0; i < NumOfIssues; i++)
            {
                Issues.Add( "Issue" + random.Next(1000) );
            }
            data.Issues = Issues.ToArray();
        }
    }
}

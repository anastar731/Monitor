
using System.Threading;
using System.Threading.Tasks;
using Monitor.Devices;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Monitor.Models;

namespace Monitor.Reporting.Scheduler
{
    public class CreateReportTask: IScheduledTask
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CreateReportTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            List<Agent> ReportedAgents = new List<Agent>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AgentDataContext>();
                var agents = context.AgentDataEntities;
                foreach (var agent in agents)
                {
                    ReportedAgents.Add(new Agent(agent));
                }
            }
            CreateReport(ReportedAgents);
        }
        private void CreateReport(List<Agent> agents)
        {          
            string path = @"c:\\temp\\" + System.AppDomain.CurrentDomain.FriendlyName + "\\reports\\";
            string fileName = "AgentReport" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            int padding = 30;
                if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            List<string> lines = new List<string>();
            lines.Add("Agent ID".PadRight(padding, ' ') + "\t" + "Last Seen".PadRight(padding, ' ') + "\t" + "Active".PadRight(padding,' ') + "\t" + "Inactive time (for inactive agents)\n");
            foreach (var agent in agents)
            {

                lines.Add(agent.ID.PadRight(padding, ' ') + "\t" + agent.LastSeen.ToString().PadRight(padding, ' ') + "\t" + agent.isActive.ToString().PadRight(padding, ' ') + "\t" + (agent.isActive?"N/A":(agent.InactiveTime.ToString())));
                if (agent.isActive)
                {
                    lines.Add("Issues:");
                    foreach (var issue in agent.Issues)
                    {
                        lines.Add(issue);
                    }
             
                }
            }          
            System.IO.File.AppendAllLines(path + fileName, lines.ToArray());        
        }
    }
}

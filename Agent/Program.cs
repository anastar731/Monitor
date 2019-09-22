using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace Agent
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static IConfigurationRoot configuration;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
            client.BaseAddress = new Uri(configuration["baseURI"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(configuration["httpHeaderString"]));
            try
            {
                int numOfAgents = Int32.Parse(configuration["numberOfAgents"]);
                for (int i = 1; i <= numOfAgents; i++)
                {
                    EmulateAgent(i);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();

        }
        static async Task<Uri> CreateAgentDataAsync(AgentData data)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                configuration["httpRequestURI"], data);

            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<AgentData> UpdateAgentDataAsync(AgentData data)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                configuration["httpRequestURI"]+"{ data.ID}", data);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            data = await response.Content.ReadAsAsync<AgentData>();
            return data;
        }
        private static async void EmulateAgent(int Id)
        {
            Agent agent = new Agent(Id);
            agent.UpdateIssues();           
            var uri = await CreateAgentDataAsync(agent.Data);
            Console.WriteLine($"Created Agent {Id} at {uri}");
            while (true)
            {              
                agent.UpdateIssues();
                await UpdateAgentDataAsync(agent.Data);
                Console.WriteLine("Updated Agent {0}. Time:{1}", Id, DateTime.Now);
                await Task.Delay(Id*5000); 
            }
        }
    }
}





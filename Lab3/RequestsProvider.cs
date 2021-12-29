namespace Lab3
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;


    public static class RequestsProvider
    {
        private static readonly HttpClient client = new();

        private static Account account;
        private static int userId = 777666777;
        public static string mode = "Lcg";

        static RequestsProvider()
        {
            client.BaseAddress = new Uri("http://95.217.177.249/casino/");
            
            var random = new Random();
            var id = random.Next();
            account = Get<Account>($"createacc?id={id}");
        }

        public static BetResult Play(long bet, long number)
        {
            var url = $"play{mode}?id={account.Id}&bet={bet}&number={number}";
            var betResult = Get<BetResult>(url);
            return betResult;
        }

        private static T Get<T>(string url)
        {
            var response = client.GetAsync(url).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(message);
            }
            throw new Exception(message);
        }
    }
}
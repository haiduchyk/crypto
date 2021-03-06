namespace Lab3
{
    using System;
    using System.Net.Http;
    using Newtonsoft.Json;

    public static class CasinoRequestProvider
    {
        private static readonly HttpClient client = new();

        private static Account account;
        public static int Money => account.Money;

        static CasinoRequestProvider()
        {
            client.BaseAddress = new Uri("http://95.217.177.249/casino/");
            CreateUser();
        }

        private static void CreateUser()
        {
            var random = new Random();
            var id = random.Next();
            account = Get<Account>($"createacc?id={id}");
        }

        public static BetResult Play(long bet, long number, Mode mode)
        {
            var url = $"play{mode}?id={account.Id}&bet={bet}&number={number}";
            var betResult = Get<BetResult>(url);
            account = betResult.Account;
            betResult.BetNumber = number;
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
namespace Lab3
{
    public class BetResult
    {
        public string Message { get; set; }
        public Account Account { get; set; }
        public long RealNumber { get; set; }
        public long BetNumber { get; set; }
        
        public override string ToString()
        {
            return $"{Message}\nRealNumber => {RealNumber}; Bet => {BetNumber}";
        }
    }
}
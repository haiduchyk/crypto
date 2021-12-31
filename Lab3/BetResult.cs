namespace Lab3
{
    public record BetResult(string Message, Account Account, long RealNumber)
    {
        public long BetNumber { get; set; }
    }
}
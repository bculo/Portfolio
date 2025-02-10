namespace Time.Abstract.Contracts
{
    public interface IDateTimeProvider
    {
        public DateTime Time { get; }
        public DateTimeOffset TimeOffset { get; }
    }
}

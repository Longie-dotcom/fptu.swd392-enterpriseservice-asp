namespace Domain.DomainException
{
    public class WasteTypeAggregateException : Exception
    {
        public WasteTypeAggregateException(string message) : base(message) { }
    }
}

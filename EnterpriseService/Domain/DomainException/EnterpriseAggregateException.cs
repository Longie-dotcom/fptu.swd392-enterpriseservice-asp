namespace Domain.DomainException
{
    public class EnterpriseAggregateException : Exception
    {
        public EnterpriseAggregateException(string message) : base(message) { }   
    }
}

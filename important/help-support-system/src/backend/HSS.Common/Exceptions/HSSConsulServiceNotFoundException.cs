namespace HSS.Common.Exceptions
{
    [Serializable]
    public class HSSConsulServiceNotFoundException : Exception
    {
        public HSSConsulServiceNotFoundException(string? message) : base(message)
        {
        }
    }
}
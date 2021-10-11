namespace Array.Test.Core
{
    // Base class for all application response types.
    public abstract class ServiceResponse 
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
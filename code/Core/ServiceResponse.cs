namespace code.Core.Operations
{
    public abstract class ServiceResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
using System;

namespace Docker.Test.Core 
{
    public class DefaultDateTimeProvider : IDateTimeProvider 
    {
        public DateTime GetUtcNow() 
        {
            return DateTime.UtcNow;
        }
    }

}
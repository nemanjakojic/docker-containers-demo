using System;

namespace Array.Test.Core 
{
    public class DefaultDateTimeProvider : IDateTimeProvider 
    {
        public DateTime GetUtcNow() 
        {
            return DateTime.UtcNow;
        }
    }

}
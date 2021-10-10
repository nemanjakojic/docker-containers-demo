using System;

namespace code.Core 
{
    public class DefaultDateTimeProvider : IDateTimeProvider 
    {
        public DateTime GetUtcNow() 
        {
            return DateTime.UtcNow;
        }
    }

}
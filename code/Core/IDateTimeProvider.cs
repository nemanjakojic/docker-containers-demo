using System;

namespace code.Core 
{
    public interface IDateTimeProvider 
    {
        // Retrieves current time in UTC timezone.
        DateTime GetUtcNow();
    }

}
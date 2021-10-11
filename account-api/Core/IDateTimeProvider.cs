using System;

namespace Array.Test.Core 
{
    public interface IDateTimeProvider 
    {
        // Retrieves current time in UTC timezone.
        DateTime GetUtcNow();
    }

}
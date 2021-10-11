using System;

namespace Account.Api.Core 
{
    public interface IDateTimeProvider 
    {
        // Retrieves current time in UTC timezone.
        DateTime GetUtcNow();
    }

}
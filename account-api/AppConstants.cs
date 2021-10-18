using System;

namespace Docker.Test
{
    public static class AppConstants 
    {
        public const string LoggedInUserSessionKey = "LoggedInUser";
        public const string AppCookieName = "AppCookie";
        public static TimeSpan DefaultSessionIdleTimeout = TimeSpan.FromMinutes(10);
    }
}
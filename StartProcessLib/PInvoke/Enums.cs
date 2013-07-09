namespace StartProcessLib.PInvoke
{
    using System;

    public partial class NativeMethods
    {
        [Flags]
        public enum LogonType
        {
            LOGON32_LOGON_INTERACTIVE       = 2,
            LOGON32_LOGON_NETWORK           = 3,
            LOGON32_LOGON_BATCH             = 4,
            LOGON32_LOGON_SERVICE           = 5,
            LOGON32_LOGON_UNLOCK            = 7,
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
            LOGON32_LOGON_NEW_CREDENTIALS   = 9
        }

        [Flags]
        public enum LogonProvider
        {
            LOGON32_PROVIDER_DEFAULT = 0,
            LOGON32_PROVIDER_WINNT35,
            LOGON32_PROVIDER_WINNT40,
            LOGON32_PROVIDER_WINNT50
        }

        public enum SecurityImpersonationLevel
        {
            SecurityAnonymous      = 0,
            SecurityIdentification = 1,
            SecurityImpersonation  = 2,
            SecurityDelegation     = 3
        }

        public enum TokenType
        {
            TokenPrimary       = 1, 
            TokenImpersonation = 2
        } 
    }
}

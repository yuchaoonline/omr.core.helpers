using System;
using System.Runtime.InteropServices;
using System.Security;

namespace OMR.Core.Helpers
{
    public static class SecureStringHelper
    {
        public static SecureString ToSecureString(this string str, bool isReadOnly)
        {
            AssertHelper.AreNotNull(str);

            SecureString secureString;

#if UNSAFE
            unsafe
            {
                fixed (char* p = str)
                {
                    secureString = new SecureString(p, str.Length);

                    if (isReadOnly)
                        secureString.MakeReadOnly();
                }
            }
#else
            secureString = new SecureString();
            for (int i = 0; i < str.Length; i++)
            {
                secureString.AppendChar(str[i]);
            }
#endif
            
            if (isReadOnly)
                secureString.MakeReadOnly();

            return secureString;
        }

        public static string ToSystemString(this SecureString secureString)
        {
            AssertHelper.AreNotNull(secureString);

            IntPtr p = IntPtr.Zero;
            try
            {
                p = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(p);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(p);
            }
        }


    }
}

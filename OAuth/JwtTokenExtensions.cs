using System;
using System.Text;

namespace CustomExtensions.OAuth
{
    public static class JwtTokenExtensions
    {
        public static bool JwtTryUnpack(string jwt, out string result)
        {
            try
            {
                var payload = jwt.Split('.')[1];
                var padLength = Math.Ceiling(payload.Length / 4.0) * 4;
                payload = payload
                    .PadRight(Convert.ToInt32(padLength), '=')
                    .Replace('-', '+')
                    .Replace('_', '/');

                result = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

                return true;
            }
            catch (Exception exception)
            {
                result = exception.Message;

                return false;
            }
        }
    }
}
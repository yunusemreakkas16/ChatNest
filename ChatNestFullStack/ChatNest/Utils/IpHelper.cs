namespace ChatNest.Utils
{
    public class IpHelper
    {
        public static string NormalizeIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return ip;
            }
            if (ip == "::1") return "127.0.0.1"; // Normalize IPv6 loopback to IPv4 loopback
            return ip;
        }
    }
}

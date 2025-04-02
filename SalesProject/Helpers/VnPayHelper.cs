using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SalesProject.Helpers
{
    public static class VnPayHelper
    {
        public static string CreateRequestUrl(Dictionary<string, string> vnpParams, string hashSecret, string vnpUrl)
        {
            var sortedParams = vnpParams
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .OrderBy(p => p.Key)
                .ToList();

            var signData = string.Join("&", sortedParams
                .Where(p => p.Key != "vnp_SecureHash" && p.Key != "vnp_SecureHashType")
                .Select(p => $"{p.Key}={p.Value}"));

            var secureHash = HmacSHA512(hashSecret, signData);

            // 🔥 Fix lỗi tại đây:
            var query = string.Join("&", sortedParams.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));

            return $"{vnpUrl}?{query}&vnp_SecureHash={secureHash}";
        }

        public static bool ValidateVnpaySignature(IQueryCollection queryParams, string hashSecret)
        {
            var vnpParams = queryParams
                .Where(p => p.Key.StartsWith("vnp_") && p.Key != "vnp_SecureHash" && p.Key != "vnp_SecureHashType")
                .ToDictionary(p => p.Key, p => p.Value.ToString());

            var sortedParams = vnpParams.OrderBy(p => p.Key).ToList();
            var signData = string.Join("&", sortedParams.Select(p => $"{p.Key}={p.Value}"));

            var secureHash = HmacSHA512(hashSecret, signData);
            var vnpSecureHash = queryParams["vnp_SecureHash"].ToString();

            return secureHash.Equals(vnpSecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string HmacSHA512(string key, string input)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(input);

            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

}

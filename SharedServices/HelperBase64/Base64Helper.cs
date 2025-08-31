namespace SharedServices.HelperBase64
{
    public static class Base64Helper
    {
        public static byte[] DecodeBase64Pdf(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static string EncodeToBase64Pdf(byte[] fileBytes)
        {
            return Convert.ToBase64String(fileBytes);
        }
    }
}

namespace ML_UAR.Class
{
    public class EncodeAndDecode
    {
        public string Encode(string text)
        {
            byte[] mybyte = System.Text.Encoding.UTF8.GetBytes(text);
            string textencode = System.Convert.ToBase64String(mybyte);
            return textencode;
        }

        public string Decode(string text)
        {
            byte[] mybyte = System.Convert.FromBase64String(text);
            string textdecode = System.Text.Encoding.UTF8.GetString(mybyte);
            return textdecode;
        }
    }
}

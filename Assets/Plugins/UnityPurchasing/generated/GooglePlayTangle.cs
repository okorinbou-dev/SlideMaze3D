#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("0qSDni/udkLEyn7H7RoUM/ZEQLKKqy1jxVebrAYdOMssNkN7xoZ6NJspqombpq2igS3jLVymqqqqrquoAcZM375QM6C6lWRLntKKQbPE8OgIU5PzlSATFhsfaYSFN1onZr3I2zV3jwzME0DrNu9GgrEIBueh8DmiKaqkq5spqqGpKaqqqyhJZTCwLLEKD+XtaC6TFlQ3QHGJW7i6jB7k5LZcefDjeVC4riTY7DT6ITsI/+tj+YM2hlzXF04X3d10fY6dmTwCq9qrRzQFDKpDfUXJlSM8WdU3Fh8POXTsTM80tdJ6JV8GmfH4Gk2hQuneNbjGTw2NCwGFOBXzHE5sCJMNZd5Z8U+BmWxRloItZJ+MD1UR/PEauVU0qukEKxspOqmoqquq");
        private static int[] order = new int[] { 11,10,11,9,8,13,10,8,8,10,11,13,13,13,14 };
        private static int key = 171;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("1rheD7+1h/CmyOf++63l2/zgyO5TW4lqv6utOVfXuUsAAxuINR5btHPhcSYBs5QN/1PayPoQ4MYAqPEripmbjJGbndiLjJmMnZWdloyL1shtZoL0XL9zoyzuz8szPPe1NuyRKb2G57STqG65cTyMmvPoe7l/y3J53BoTKU+IJ/e9Gd8yCZWAFR9N7+93i3mYPuOj8ddqSgC8sAiYwGbtDYiUndi7nYqMkZ6Rm5mMkZeW2LmN2JmWnNibnYqMkZ6Rm5mMkZeW2Iiczdvts+2h5UtsDw5kZjeoQjmgqPDT/vn9/f/6+e7mkIyMiIvC19ePyOn++6388uvyuYiIlJ3YsZab1snnfXt942HFv88KUWO4dtQsSWjqIILIevmOyPb++63l9/n5B/z8+/r5RgyLYxYqnPczgbfMIFrGAYAHkzDIevxDyHr7W1j7+vn6+vn6yPX+8e7I7P77rfz76/W5iIiUndiql5eMUCSG2s0y3S0h9y6TLFrc2+kPWVTLzqLImsnzyPH++638/uv6ravJ6/CmyHr56f77reXY/Hr58Mh6+fzIiJSd2KqXl4zYu7nI5u/1yM7IzMqWnNibl5ackYyRl5aL2Jee2I2LnZ938EzYDzNU1NiXiE7H+ch0T7s3j4/WmYiIlJ3Wm5eV15mIiJSdm5mRnpGbmYyRl5bYuY2MkJeKkYyByaqdlJGZlpud2JeW2IyQkYvYm52K1NibnYqMkZ6Rm5mMndiIl5SRm4H1/vHSfrB+D/X5+f39+Pt6+fn4pE3CVQz39vhq80nZ7taMLcT1I5rusSCOZ8vsnVmPbDHV+vv5+Plbevk4m8uPD8L/1K4TIvfZ9iJCi+G3TdJ+sH4P9fn5/f34yJrJ88jx/vut52kj5r+oE/0VpoF81RPOWq+0rRR6+fj+8dJ+sH4Pm5z9+ch5CsjS/oHYmYuLjZWdi9iZm5udiIyZlpud/vut5fb87vzs0yiRv2yO8QYMk3XeyNz++6388+vluYiIlJ3Yu52KjE/jRWu63OrSP/flTrVkppsws3jvjJGekZuZjJ3YmoHYmZaB2IiZiozOYbTVgE8VdGMkC49jCo4qj8i3Of8UhcF7c6vYK8A8SUdit/KTB9ME92XFC9Ox0OIwBjZNQfYhpuQuM8VJyKAUovzKdJBLd+UmnYsHn6adRP34+3r59/jIevny+nr5+fgcaVHx2Jee2IyQndiMkJ2W2JmIiJSRm5mhX/3xhO+4runmjCtPc9vDv1stl/7I9/77reXr+fkH/P3I+/n5B8jljJCXipGMgcnuyOz++638++v1uYh47NMokb9sjvEGDJN11rheD7+1h83KyczIy86i7/XLzcjKyMHKyczIxd6f2HLLkg/1ejcmE1vXAauSo5yalJ3Yi4yZlpyZipzYjJ2KlYvYmTHhig2l9i2Hp2MK3ftCrXe1pfUJh7lQYAEpMp5k3JPpKFtDHOPSO+chzoc5f60hX2FByroDIC2JZoZZqvz+6/qtq8nryOn++6388uvyuYiI18h5O/7w0/75/f3/+vrIeU7ieUuUndixlpvWyd7I3P77rfzz6+W5iNi7uch6+drI9f7x0n6wfg/1+fn5qFJyLSIcBCjx/89IjY3Z");
        private static int[] order = new int[] { 31,24,16,46,18,25,53,55,18,45,20,35,45,49,28,50,31,45,54,54,33,53,43,53,36,35,38,44,47,53,44,57,36,50,34,40,44,50,59,39,48,51,43,59,47,51,52,53,48,53,55,56,59,54,56,58,57,58,59,59,60 };
        private static int key = 248;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

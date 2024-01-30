namespace Utils
{
    public static class MathfUtils
    {
        public static float Remap(float value, float low0, float high0, float low1, float high1)
        {
            return low1 + (value - low0) * (high1 - low1) / (high0 - low0);
        }
    }
}
namespace STVR.SimpleDelayer
{
    public static class DelayExtension
    {
        public static bool NotRunning(this Delay delay)
        {
            if (delay == null)
                return new Delay().Started;
            return delay.Expired();
        }
    }
}
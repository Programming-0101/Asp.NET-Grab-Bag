namespace ForMicroBus.DemoMicroBus
{
    using Enexure.MicroBus;

    public class SlowWorkingCommand : ICommand
    {
        /// <summary>Sleep time in milliseconds.</summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// A sample MicroBus command that requests a timeout.
        /// </summary>
        /// <param name="timeout">Sleep time in milliseconds</param>
        public SlowWorkingCommand(int timeout)
        {
            Timeout = timeout;
        }
    }
}
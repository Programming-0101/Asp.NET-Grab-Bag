namespace ForMicroBus.DemoMicroBus
{
    using Enexure.MicroBus;
    using System.Threading;
    using System.Threading.Tasks;

    public class DemoCommandHandler
        : ICommandHandler<SlowWorkingCommand>
        , ICancelableCommandHandler<InterruptableSlowWorkingCommand> // Footnote #1
    {
        public Task Handle(SlowWorkingCommand cmd)
        {
            // Validate - not negative
            if (cmd.Timeout < 0)
                return Task.FromException(new TimeTravelNotSupportedException());

            return Task.CompletedTask;
        }

        public Task Handle(InterruptableSlowWorkingCommand cmd, CancellationToken token)
        {
            // Validate - not negative
            if (cmd.Timeout < 0)
                return Task.FromException(new TimeTravelNotSupportedException());
            #region CancellationToken and Thread.Sleep - Footnote #2
            if (token.WaitHandle.WaitOne(cmd.Timeout))
                //while (!token.IsCancellationRequested && !token.WaitHandle.WaitOne(cmd.Timeout))
                //    ;
                //if (token.IsCancellationRequested)
                return Task.FromCanceled(token);
            #endregion
            //// Validate - not too long
            //if (cmd.Timeout > 5000)
            //{
            //    return Task.FromCanceled(token); // Footnote #2
            //}

            // Just right!
            //Thread.Sleep(cmd.Timeout);
            return Task.CompletedTask;
        }
    }
    /* ***************
     * Footnotes:
     *  1 - See https://github.com/Lavinski/Enexure.MicroBus/wiki/Cancellable-Handlers
     *  2 - Adapted ideas about cancelling tokens from
     *      - http://classport.blogspot.ca/2014/05/cancellationtoken-and-threadsleep.html
     *      - https://stackoverflow.com/questions/28328421/using-waithandle-waitone
     */
}
namespace ForMicroBus.UserControls
{
    using Enexure.MicroBus;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class MicroBusUserControlBase : System.Web.UI.UserControl
    {
        #region MicroBus Entry Points
        public ICancelableMicroBus MicroBus { get; set; } // Footnote #1

        /// <summary>
        /// Sends a command through the MicroBus and raises events before and after the <see cref="System.Threading.Tasks.Task"/> is issued.
        /// </summary>
        /// <param name="cmd"></param>
        protected void SendCommand(ICommand cmd)
        {
            // Check that the DI has provided the IMicroBus instance.
            if (MicroBus == null) throw new MicroBusInstanceNullReferenceException(GetType().Name);

            // Issue the command
            MicroBusCommandIssuedEventArgs evt = new MicroBusCommandIssuedEventArgs(cmd);
            Task task = null;
            try
            {
                task = MicroBus.SendAsync(cmd);
                OnMicroBusCommandIssued(evt);
                task.Wait();
                if (task.IsCompleted)
                    OnMicroBusCommandCompleted(evt);
                if (task.IsCanceled)
                    OnMicroBusCommandCanceled(evt);
            }
            catch (Exception)
            {
                // TODO: it should be handled/logged
                if (task.IsFaulted)
                    OnMicroBusCommandFaulted(new MicroBusCommandFaultedEventArgs(cmd, task.Exception));
            }
        }

        #region //// UNDER CONSTRUCTION \\\\\
        protected void SendCommandAsync(ICommand cmd, CancellationToken token)
        {
            // Check that the DI has provided the IMicroBus instance.
            if (MicroBus == null) throw new MicroBusInstanceNullReferenceException(GetType().Name);

            // Issue the command
            MicroBusCommandIssuedEventArgs evt = new MicroBusCommandIssuedEventArgs(cmd);
            Task task = null;
            try
            {
                task = MicroBus.SendAsync(cmd, token); // footnote 1
                OnMicroBusCommandIssued(evt);
                task.Wait();
                if (task.IsCompleted)
                    OnMicroBusCommandCompleted(evt);
                if (task.IsCanceled)
                    OnMicroBusCommandCanceled(evt);
            }
            //catch (OperationCanceledException)
            //{
            //    // TODO: it should be handled/logged
            //}
            catch (Exception)
            {
                // TODO: it should be handled/logged
                if (task.IsFaulted)
                    OnMicroBusCommandFaulted(new MicroBusCommandFaultedEventArgs(cmd, task.Exception));
            }
        }
        #endregion
        #endregion

        #region Events around sending commands via the MicroBus
        /// <summary>
        /// Raised immediately after calling MicroBus.SendAsync().
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMicroBusCommandIssued(MicroBusCommandIssuedEventArgs e)
        {
            var handler = MicroBusCommandIssued;
            if (handler != null)
                handler(this, e);
        }

        public event MicroBusCommandIssuedEventHandler MicroBusCommandIssued;

        /// <summary>
        /// Raised when the MicroBus.SendAsync() Task is completed.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMicroBusCommandCompleted(MicroBusCommandIssuedEventArgs e)
        {
            // A more terse way of invoking than shown in OnMicroBusCommandIssued
            MicroBusCommandCompleted?.Invoke(this, e);
        }

        public event MicroBusCommandCompletedEventHandler MicroBusCommandCompleted;

        /// <summary>
        /// Raised when the MicroBus.SendAsync() Task is faulted.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMicroBusCommandFaulted(MicroBusCommandFaultedEventArgs e)
        {
            MicroBusCommandFaulted?.Invoke(this, e);
        }

        public event MicroBusCommandFaultedEventHandler MicroBusCommandFaulted;

        /// <summary>
        /// Raised when the MicroBus.SendAsync() Task is canceled.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMicroBusCommandCanceled(MicroBusCommandIssuedEventArgs e)
        {
            MicroBusCommandCanceled?.Invoke(this, e);
        }

        public event MicroBusCommandCanceledEventHandler MicroBusCommandCanceled;

        #endregion
    }

    #region Supporting EventArg & Exception classes
    public delegate void MicroBusCommandIssuedEventHandler(Object sender, MicroBusCommandIssuedEventArgs e);
    public delegate void MicroBusCommandCompletedEventHandler(Object sender, MicroBusCommandIssuedEventArgs e);
    public delegate void MicroBusCommandFaultedEventHandler(Object sender, MicroBusCommandFaultedEventArgs e);
    public delegate void MicroBusCommandCanceledEventHandler(Object sender, MicroBusCommandIssuedEventArgs e);

    public class MicroBusCommandFaultedEventArgs : MicroBusCommandIssuedEventArgs
    {
        public Exception Exception { get; private set; }
        public MicroBusCommandFaultedEventArgs(ICommand cmd, Exception exception) : base(cmd)
        {
            Exception = exception;
        }
    }

    public class MicroBusCommandIssuedEventArgs : EventArgs
    {
        public ICommand Command { get; private set; }
        public DateTime TimeIssued { get; private set; } = DateTime.Now;
        public MicroBusCommandIssuedEventArgs(ICommand cmd)
        {
            Command = cmd;
        }
    }

    public class MicroBusInstanceNullReferenceException : NullReferenceException
    {
        public string ControlContext { get; private set; }
        public MicroBusInstanceNullReferenceException(string controlContext)
        {
            ControlContext = controlContext;
        }
    }
    #endregion
}

/* ***************
 * Footnotes:
 *  1 - See https://github.com/Lavinski/Enexure.MicroBus/wiki/Cancellable-Handlers
 */

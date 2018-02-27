using ForMicroBus.DemoMicroBus;
using System;
using System.Threading;

namespace ForMicroBus.UserControls
{
    public partial class MiroBusDemoControl : MicroBusUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void WaitButton_Click(object sender, EventArgs e)
        {
            // a) Construct the command object
            var cmd = new SlowWorkingCommand(int.Parse(WaitTime.Text));

            // b) Set up a cancellation token (so we can interrupt the command's execution for some reason)
            //CancellationTokenSource source = new CancellationTokenSource(); // TBD
            //CancellationToken token = source.Token; // TBD

            // c) Issue the command (starts the Task)
            // SendCommandAsync(cmd, token); // TBD
            SendCommand(cmd);

            // d) Change my mind (capricious), if the selected timeout was "too long" for my liking
            //if (cmd.Timeout > 5000) // TBD
            //    source.Cancel(); // Footnote #2 // TBD
        }
    }
    /* ***************
     * Footnotes:
     *  1 - See https://github.com/Lavinski/Enexure.MicroBus/wiki/Cancellable-Handlers
     *  2 - For more on canceling tasks, see
     *      - https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken?view=netframework-4.6.2
     *      - https://johnbadams.wordpress.com/2012/03/10/understanding-cancellationtokensource-with-tasks/
     *      - https://lbadri.wordpress.com/2016/10/04/cancellationtoken-with-task-run-and-wait/
     */
}
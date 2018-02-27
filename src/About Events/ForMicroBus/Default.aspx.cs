using ForMicroBus.DemoMicroBus;
using ForMicroBus.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ForMicroBus
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void MicroBusDemoControl_MicroBusCommandIssued(Object sender, MicroBusCommandIssuedEventArgs e)
        {
            var evt = e.Command as SlowWorkingCommand;
            MessageLabel.Text = $"Started command at {e.TimeIssued.ToLongTimeString()} for {evt.Timeout} milliseconds. ";
        }

        protected void MicroBusDemoControl_MicroBusCommandCompleted(Object sender, MicroBusCommandIssuedEventArgs e)
        {
            MessageLabel.Text += $"Completed by {DateTime.Now.ToLongTimeString()}.";
            MessageContainer.Attributes["class"] = "alert alert-success";
        }

        protected void MicroBusDemoControl_MicroBusCommandFaulted(Object sender, MicroBusCommandFaultedEventArgs e)
        {
            MessageLabel.Text += $"Message threw an error out with a {e.Exception.GetType().Name} exception.";
            MessageContainer.Attributes["class"] = "alert alert-danger";
        }

        protected void MicroBusDemoControl_MicroBusCommandCanceled(Object sender, MicroBusCommandIssuedEventArgs e)
        {
            MessageLabel.Text += $"Message was canceled.";
            MessageContainer.Attributes["class"] = "alert alert-warning";
        }
    }
}
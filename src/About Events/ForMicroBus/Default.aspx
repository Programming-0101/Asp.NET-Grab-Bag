<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ForMicroBus._Default" %>

<%@ Register Src="~/UserControls/MiroBusDemoControl.ascx" TagPrefix="dg" TagName="MiroBusDemoControl" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HtmlHead" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.11.0/themes/prism.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.11.0/themes/prism-coy.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.11.0/components/prism-core.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.11.0/components/prism-clike.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.11.0/components/prism-csharp.min.js"></script>

    <style>
        .navbar-inverse .navbar-brand {
            color: darkorange;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>MicroBus User Control Base Class</h1>
        <p class="lead">Demonstrates using MicroBus, Autofac, and a custom user control base class.</p>
        
        <div id="MessageContainer" runat="server"><b><asp:Label ID="MessageLabel" runat="server" /></b></div>

        <blockquote>
            <dg:MiroBusDemoControl runat="server" ID="MicroBusDemoControl"
                OnMicroBusCommandIssued="MicroBusDemoControl_MicroBusCommandIssued"
                OnMicroBusCommandCompleted="MicroBusDemoControl_MicroBusCommandCompleted"
                OnMicroBusCommandFaulted="MicroBusDemoControl_MicroBusCommandFaulted"
                OnMicroBusCommandCanceled="MicroBusDemoControl_MicroBusCommandCanceled" />
        </blockquote>
    </div>

    <div class="row">
        <div class="col-md-6">
            <h2>MicroBus Entry Points</h2>
            <p>The <code>MicroBusUserControlBase</code> class exposes entry points into working with the MicroBus instance.</p>
            <pre><code class="language-csharp">
#region MicroBus Entry Points
public ICancelableMicroBus MicroBus { get; set; }

// Sends a command through the MicroBus and raises events before and after the Task is issued.
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
#endregion</code></pre>
        </div>
        <div class="col-md-6">
            <h2>Events</h2>
            <p>Likewise, the <code>MicroBusUserControlBase</code> raises events to track when commands are run (and if there are any errors).</p>
            <pre><code class="language-csharp">
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
                 </code></pre>
        </div>
    </div>

</asp:Content>

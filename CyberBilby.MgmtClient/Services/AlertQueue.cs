using CyberBilby.MgmtClient.Components.Models;
using CyberBilby.MgmtClient.Events;

namespace CyberBilby.MgmtClient.Services;

public class AlertQueue
{
    public event EventHandler<AlertPushedEventArgs>? AlertPushed;

    public void Push(AlertType type, string message, int lifetime = 0)
    {
        AlertPushed?.Invoke(this, new AlertPushedEventArgs(type, message, lifetime));
    }
}

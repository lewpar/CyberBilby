namespace CyberBilby.MgmtClient.Events;

public class CreatePostResponseEventArgs : EventArgs
{
    public bool Result { get; set; }
    public string Message { get; set; }

    public CreatePostResponseEventArgs(bool result, string message)
    {
        Result = result;
        Message = message;
    }
}

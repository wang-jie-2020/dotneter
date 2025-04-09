namespace OnlineEditorsServer.DocumentServer;

public class Callback
{
    public string Key { get; set; }

    public TrackerStatus Status { get; set; }
    
    public string Url { get; set; }
    
    public string ChangesUrl { get; set; }

    public List<CallbackAction> Actions { get; set; }

    public History? History { get; set; }
    
    public string FileType { get; set; }

    // public ForcesaveType ForcesaveType { get; set; }
    //
    // public string FormsDataUrl { get; set; }
    //
    // public List<string> Users { get; set; }
    //
    // public string Token { get; set; }
}

public class CallbackAction
{
    public ActionType Type { get; set; }

    public string UserId { get; set; }
}

public class History
{
    public string ServerVersion { get; set; }

    public List<Changes> Changes { get; set; }
}

public class Changes
{
    public string Created { get; set; }

    public User User { get; set; }
}

public class User
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Group { get; set; }
    
    public string Image { get; set; }
}
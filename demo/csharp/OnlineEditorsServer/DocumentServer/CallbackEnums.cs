namespace OnlineEditorsServer.DocumentServer;

public enum TrackerStatus
{
    NotFound = 0,
    Editing = 1,
    MustSave = 2,
    Corrupted = 3,
    Closed = 4,
    MustForceSave = 6,
    CorruptedForceSave = 7
}

public enum ActionType
{
    DisConnected = 0,
    Connected = 1,
    ForceSave = 2
}

public enum ForcesaveType
{
    CommandService = 0,
    SaveButton = 1,
    Timer = 2,
    SubmitForm = 3
}


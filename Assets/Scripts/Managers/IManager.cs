public enum ManagerState
{
    Offline,
    Initializing,
    Online
}

internal interface IManager
{
    ManagerState ManState { get; }

    void BootSequence ();
}
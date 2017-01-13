namespace RemoteTech.CommandHandler
{
    public interface IAction
    {
        void Activate();
        bool IsReady
        {
            get;
        }
        bool IsRunning
        {
            get;
        }
        bool HasFinished
        {
            get;
        }
        bool HasFailed
        {
            get;
        }
        string ProviderName
        {
            get;
        }
        IAction[] SubCommands
        {
            get;
        }
        void Save(ConfigNode node);
        void AddSubAction(IAction cmd);
    }
}
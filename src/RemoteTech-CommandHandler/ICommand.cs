namespace RemoteTech.CommandHandler
{
    public interface ICommand
    {
        void Run();
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
        string providerName
        {
            get;
        }
        void Save(ConfigNode node);
    }
}
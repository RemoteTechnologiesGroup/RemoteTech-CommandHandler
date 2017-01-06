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
        string ProviderName
        {
            get;
        }
        ICommand[] SubCommands
        {
            get;
        }
        void Save(ConfigNode node);
        void AddSubCommand(ICommand cmd);
    }
}
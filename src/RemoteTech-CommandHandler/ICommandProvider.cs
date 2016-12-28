namespace RemoteTech.CommandHandler
{
    public interface ICommandProvider
    {
        string Name
        {
            get;
        }

        ICommand LoadCommand(ConfigNode node);
    }
}
namespace RemoteTech.CommandHandler
{
    public interface IProvider
    {
        string Name
        {
            get;
        }

        ICommand LoadCommand(ConfigNode node);
        ICondition LoadCondition(ConfigNode node);
    }
}
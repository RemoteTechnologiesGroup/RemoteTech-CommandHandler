namespace RemoteTech.CommandHandler
{
    public interface IProvider
    {
        ICommand LoadCommand(ConfigNode node);
        ICondition LoadCondition(ConfigNode node);
    }
}
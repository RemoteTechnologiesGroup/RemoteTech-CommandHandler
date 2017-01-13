namespace RemoteTech.CommandHandler
{
    public interface IProvider
    {
        IAction LoadAction(ConfigNode node);
        ICondition LoadCondition(ConfigNode node);
    }
}
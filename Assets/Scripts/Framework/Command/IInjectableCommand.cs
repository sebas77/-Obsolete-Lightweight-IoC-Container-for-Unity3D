namespace Command
{
	public interface IInjectableCommand : ICommand
	{
	    void Inject<T>(T dependency);
	}
}

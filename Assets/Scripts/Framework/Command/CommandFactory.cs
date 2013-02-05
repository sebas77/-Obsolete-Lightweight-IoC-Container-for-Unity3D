using System;
using IoC;

namespace Command
{
	public interface ICommandFactory
	{
		TCommand Build<TCommand>() where TCommand:ICommand, new();
		TCommand Build<TCommand>(Func<ICommand> constructor) where TCommand:ICommand;
	}
	
	public class CommandFactory:ICommandFactory
	{
		[Inject] public IContainer container { set; private get; }
		
		public TCommand Build<TCommand>() where TCommand:ICommand, new()
		{
			TCommand command = new TCommand();
			
			container.Inject(command);
			
			return command;
		}

		public TCommand Build<TCommand>(params object[] args) where TCommand:ICommand, new()
		{
			Type commandClass = typeof(TCommand);
			
			TCommand command = (TCommand)Activator.CreateInstance(commandClass, args);
			
			container.Inject(command);
			
			return command;
		}
		
		public TCommand Build<TCommand>(Func<ICommand> constructor) where TCommand:ICommand
		{
			TCommand command = (TCommand)constructor();
			
			container.Inject(command);
			
			return command;
		}
	}
}


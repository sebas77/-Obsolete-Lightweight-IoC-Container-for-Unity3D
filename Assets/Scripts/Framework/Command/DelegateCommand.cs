using System;

namespace Command
{
	public class DelegateCommand:ICommand
	{
		public delegate void ToExecute();
		
		public DelegateCommand (ToExecute lambda)
		{
			_lambda = lambda;	
		}
		
		public void Execute()
		{
			_lambda();
		}
		
		private ToExecute _lambda;
	}
}


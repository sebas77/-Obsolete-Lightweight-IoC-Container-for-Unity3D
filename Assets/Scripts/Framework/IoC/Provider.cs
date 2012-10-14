using System;

namespace IoC
{
	public interface IProvider
	{
		object Create();
		
		Type contract { get; }
	}
	
	public class StandardProvider:IProvider
	{
		public StandardProvider(System.Type type)
		{
			_type = type;
		}
		
		public object Create()
		{
			return Activator.CreateInstance(_type);
		}
		
		public Type contract { get { return _type; } }
		
		private System.Type _type;
	} 
	
	public class StandardProvider<T>:IProvider where T:new()
	{
		public object Create()
		{
			return new T();
		}
		
		public System.Type contract { get { return typeof(T); } }
	}

}


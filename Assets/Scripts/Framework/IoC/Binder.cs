using System;

namespace IoC
{
	/// <summary>
	/// Use this class to register an interface
	/// or class into the container.
	/// </summary>
	public class Binder<Contractor>: IBinder<Contractor>
	{
		virtual public void Bind<ToBind>(IInternalContainer container)
		{
			_container = container;
			
			type = typeof(ToBind);
		}
		
		virtual public void AsSingle() 
		{
			_container.Register(type);
		}
		
		public void ToFactory<T>(IProvider provider) where T:IProvider, Contractor
		{
			_container.Register(type, provider);
		}
		
		virtual public void AsSingle<T>() where T:Contractor, new()
		{
			_container.Register(type, typeof(T));
		}
		
		virtual public void AsSingle<T>(T istance) where T:class, Contractor
		{
			_container.Map(type, typeof(T), istance);
		}
		
		private 	IInternalContainer  _container;
		
		protected 	Type				type { get; private set; }
	}
}


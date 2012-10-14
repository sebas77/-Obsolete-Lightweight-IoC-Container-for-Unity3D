using System;

namespace IoC
{
	public interface IContainer
	{
		IBinder<TContractor> 	Bind<TContractor>();
		
		TContractor Build<TContractor>() where TContractor:class;
		void		Release<TContractor>() where TContractor:class;
		
		void 		Inject<TContractor>(TContractor instance);
	}
	
	/// <summary>
	/// This class can be used by the final user, but
	/// it is not advised to. Generally for internal
	/// purposes only.
	/// </summary>
	public interface IInternalContainer
	{
		void Register(System.Type mapping);
		void Register(System.Type mapping, System.Type mapper);
		void Register(System.Type mapping, IProvider provider);
		
		void Map(System.Type mapping, object istance);
	}
}


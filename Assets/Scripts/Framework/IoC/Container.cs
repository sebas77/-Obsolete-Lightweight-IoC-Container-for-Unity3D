using System;
using System.Collections.Generic;
using System.Reflection;
using IoC;

namespace IoC
{
	public class Container: IContainer, IInternalContainer
	{
		public Container()
		{
			_providers = new Dictionary<Type, IProvider>();
			_uniqueInstances = new Dictionary<IProvider, object>();
			_injectLater = new HashSet<object>();
		}
		
		//
		// IContainer interface
		//
		
		public IBinder<TContractor> Bind<TContractor>()
		{
			IBinder<TContractor> binder = BinderProvider<TContractor>();
			
			binder.Bind<TContractor>(this);
			
			return binder;
		}
		
		public TContractor Build<TContractor>() where TContractor:class
		{
			Type contract = typeof(TContractor);
			
			TContractor instance = Get(contract) as TContractor;
			
			DesignByContract.Check.Ensure(instance != null, "IoC.Container instance failed to be built (contractor not found)");
			
			return instance;
		}
		
		public void Release<TContractor>() where TContractor:class
		{
			Type type = typeof(TContractor);
				
			if (_providers.ContainsKey(type))
			{
				IProvider provider = _providers[type];
				
				if (_uniqueInstances.ContainsKey(provider))
					_uniqueInstances.Remove(provider);
				
				_providers.Remove(type);
			}
		}
		
		//
		// IInternalContainer interface
		//

		virtual public void Register(System.Type type, System.Type mapper)
		{
			DesignByContract.Check.Require(type.IsAssignableFrom(mapper));
			
			_providers[type] = new StandardProvider(mapper);
		}
		
		virtual public void Register(System.Type type, IProvider provider)
		{
			DesignByContract.Check.Require(type.IsAssignableFrom(provider.contract));
			
			_providers[type] = provider;
		}
		
		virtual public void Register(System.Type type)
		{
			_providers[type] = new StandardProvider(type);
		}
		
		virtual public void Map(System.Type type, object instance)
		{
			DesignByContract.Check.Require(instance != null);
			DesignByContract.Check.Require(type.IsAssignableFrom(instance.GetType()), "Trying to register an invalid instance");
			
			_injectLater.Add(instance);
			
			_providers[type] = new StandardProvider(type);
						
			_uniqueInstances[_providers[type]] = instance;
		}
		
		public void Inject<TContractor>(TContractor instance)
		{
			if (instance != null)
				InternalInject(instance);
		}
		
		//
		// Private Members
		//
		
		private void InternalInject(object injectable)
		{
			DesignByContract.Check.Require(injectable != null);
			
			Type contract = injectable.GetType();
			
			MemberInfo[] members = contract.FindMembers(MemberTypes.Property,
														BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, 
														null, null);
			
			foreach (MemberInfo member in members)
			{
				object[] attrs = member.GetCustomAttributes(typeof(InjectAttribute), true);
				
				foreach (object attr in attrs)
				{
					if (attr.GetType() == typeof(InjectAttribute))
					{
						PropertyInfo info = member as PropertyInfo;
						
						if (info.PropertyType == typeof(IContainer)) //self inject
							info.SetValue(injectable, this, null);
						else
						{
							object valueObj = Get(info.PropertyType);
							
							//inject in Injectable the valueObj
							if (valueObj != null)
								info.SetValue(injectable, valueObj, null);
						}
					}
				}
			}
			
			if (injectable is IInitialize)
				(injectable as IInitialize).OnInject();
		}
		
		virtual protected object Get(Type contract) 
		{	
			if (_providers.ContainsKey(contract) == true)
			{	//take the provider linked to the contract
				//N.B. several contracts could be linked
				//to the provider of the same class
				IProvider provider = _providers[contract];
				//the contract is actually the provider type
				if (_uniqueInstances.ContainsKey(provider) == false)
					return CreateDependency(provider);
				else
				{
					object instance = _uniqueInstances[provider];
				
					if (_injectLater.Contains(instance))
					{
						InternalInject(instance);
						
						_injectLater.Remove(instance);
					}
						
					return instance;
				}
			}
			
			return null;
		}
		
		virtual protected IBinder<TContractor> BinderProvider<TContractor>()
		{
			return new Binder<TContractor>();
		}

		private object CreateDependency(IProvider provider)
		{
			object obj = provider.Create();
	
			_uniqueInstances[provider] = obj; //seriously, this must be done before obj is injected to avoid circular dependencies
			
			InternalInject(obj);
			
			return obj;
		}
		
		private readonly Dictionary<Type, 	IProvider> 		_providers;
		private readonly Dictionary<IProvider, object> 		_uniqueInstances;
		
		private readonly HashSet<object>					_injectLater;
	}
}
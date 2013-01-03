using System;
using System.Collections.Generic;
using UnityEngine;

namespace IoC
{
	public class UnityContainer:Container
	{
		public UnityContainer():base()
		{
			Bind<IoC.GameObjectFactory>().AsSingle();
			Bind<IoC.IGameObjectFactory>().AsSingle<GameObjectFactory>();
			
			_mbcache = new Dictionary<Type, KeyValuePair<WeakReference, bool>>();
		}
			
		override public void Register(System.Type type, System.Type mapper)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(mapper) == false)
				base.Register(type, mapper);
			else
				throw new Exception("Monobehaviour can be registered only through instance: " + type.FullName);
		}
		
		override public void Register(System.Type type)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(type) == false)
				base.Register(type);
			else
				throw new Exception("Monobehaviour can be registered only through instance: " + type.FullName);
		}
		
		override public void Map(System.Type type, System.Type mapper, object instance)
		{
			if ((instance is MonoBehaviour) == false)
				base.Map(type, mapper, instance);
			else
			{
				DesignByContract.Check.Require(instance != null);
				DesignByContract.Check.Require(type.IsAssignableFrom(instance.GetType()), "Trying to register an invalid instance");
				
				KeyValuePair<WeakReference, bool> valuePair = new KeyValuePair<WeakReference, bool>(new WeakReference(instance), false);
			
				_mbcache[type] = valuePair;
			}
		}
		
		override protected object Get(Type contract)
		{
			if (_mbcache.ContainsKey(contract) == false) //if the contract is not in the cache we can assume it is not a monobehaviour
				return base.Get(contract);
			else
			{
				KeyValuePair<WeakReference, bool> valuePair = _mbcache[contract];
				
				if (valuePair.Key.IsAlive == true)
				{
					MonoBehaviour mb = valuePair.Key.Target as MonoBehaviour;
					
					if (valuePair.Value == false) //has been injected?
					{	//note the cache must be set before the injection to avoid circular dependencies (To improve)
						_mbcache[contract] = new KeyValuePair<WeakReference, bool>(new WeakReference(mb), true);
						
						Inject(mb);
					}
					
					return mb;
				}
				
				return null;
			}
		}
		
		private readonly Dictionary<Type, KeyValuePair<WeakReference, bool>> _mbcache;
	}
}


using System;
using UnityEngine;

namespace IoC
{
	public interface IMonoBehaviourFactory
	{
		M Build<M>(Func<M> constructor) where M:MonoBehaviour;
	}
	
	public class MonoBehaviourFactory: IMonoBehaviourFactory
	{
		[Inject] public IContainer container { set; private get; }
		
		public M Build<M>(Func<M> constructor) where M:MonoBehaviour
		{
			M mb = (M)constructor();
			
			container.Inject(mb);
			
			return mb;
		}
	}
}


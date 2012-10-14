using System;

namespace IoC
{
	public interface IBinder<Contractor>
	{
		void AsSingle();
		void AsSingle<T>(T istance) where T:class, Contractor;
		void AsSingle<T>() where T:Contractor, new();
		void ToFactory<T>(IProvider provider) where T:IProvider, Contractor;
			
		void Bind<ToBind>(IInternalContainer container);
	}

}


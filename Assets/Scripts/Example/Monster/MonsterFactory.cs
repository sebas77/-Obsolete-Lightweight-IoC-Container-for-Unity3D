using System;
using UnityEngine;

public class MonsterFactory:IMonsterFactory, IoC.IInitialize
{
	[IoC.Inject] public IoC.IMonoBehaviourFactory	monoBehaviourFactory	{ set; private get; }
	[IoC.Inject] public IMonsterSystem				monsterSystem 			{ set; private get; }
	
	public void OnInject()
	{
		DesignByContract.Check.Require(monoBehaviourFactory != null);
		DesignByContract.Check.Require(monsterSystem != null);
	}
	
	public void Create()
	{
		if (monsterSystem.monsterCount < 5)
		{
			GameObject monsterGO = CreateMonster();
			
			monoBehaviourFactory.Build(() => { return monsterGO.AddComponent<MonsterPathFollower>(); });
			//note: what about using something like
			//container.Bind<IMonsterPathFollower>().ToSingle<MonsterPathFollower>();
			//container.Build<IMonsterPathFollower>(monstergo); ?
				
			Monster 			monster 		= monoBehaviourFactory.Build(() => { return monsterGO.AddComponent<Monster>(); });
			 
			monsterSystem.Add(monster);
		}
	}
	
	private GameObject CreateMonster()
	{
		return GameObject.Instantiate(Resources.Load("Monster")) as GameObject;
	}
}


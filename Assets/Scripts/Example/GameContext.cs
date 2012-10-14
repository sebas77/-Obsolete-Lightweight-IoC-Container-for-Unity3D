using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Main:IContextRoot
{
	public IoC.IContainer container { get; private set; }
	
	public Main()
	{
		HOTween.Init(false, false, true);
		HOTween.EnableOverwriteManager();
		
		SetupContainer();
		StartGame();
	}
	
	void SetupContainer()
	{
		container = new IoC.UnityContainer();

		container.Bind<IoC.IMonoBehaviourFactory>().AsSingle<IoC.MonoBehaviourFactory>();
		container.Bind<IMonsterFactory>().AsSingle<MonsterFactory>();
		container.Bind<IMonsterSystem>().AsSingle<MonsterSystem>();
		
		container.Bind<PathController>().AsSingle();
		container.Bind<MonsterSpawner>().AsSingle();
	}
	
	void StartGame()
	{
		MonsterSpawner spawner = container.Build<MonsterSpawner>();
		
		//tickEngine could be added in the container as well
		//if needed to other classes!
		TickEngine tickEngine = new TickEngine(); 
		
		tickEngine.Add(spawner);
	}
}

//UnityContext must be executed before than
//anything else use the container itself.
//Exploit the Awake/Start or the Script
//Execution Order to achieve it.

public class GameContext: UnityContext<Main>
{
}

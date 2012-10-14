
public class MonsterSpawner:ITickable, IoC.IInitialize
{
	[IoC.Inject] public IMonsterFactory				monsterFactory 			{ set; private get; }
	
	public MonsterSpawner ()
	{
		_frequency = 3;
		_timeLapsed = _frequency;
	}
	
	public void OnInject()
	{
		DesignByContract.Check.Require(monsterFactory != null);
	}
	
	public void Tick(float delta)
	{
		_timeLapsed += delta;
		
		if (_timeLapsed >= _frequency)
		{
			monsterFactory.Create();
			
			_timeLapsed = 0;
			_frequency = UnityEngine.Random.Range(0.5f, 4.0f);
		}
	}
	
	private float _frequency;
	private float _timeLapsed;
}


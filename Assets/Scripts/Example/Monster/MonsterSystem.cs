using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSystem: IMonsterSystem
{
	public int monsterCount { private set; get; }
	
	public MonsterSystem()
	{
		_monstersGO = new GameObject("Monsters");
		_monsters = new Dictionary<GameObject, IMonster>();
	}
	
	public void Add<M>(M monster) where M:MonoBehaviour, IMonster
	{
		monster.OnKilled += () => {  MonsterKilled(monster.gameObject); };
		
		monsterCount++;
		
		monster.transform.parent = _monstersGO.transform;
		
		_monsters.Add(monster.gameObject, monster);
	}
	
	public IMonster SetUnderFire(GameObject target)
	{
		if (_monsters.ContainsKey(target))
		{
			IMonster monster = _monsters[target];
			
			monster.StartBeingHit(.5f);
			
			return monster;
		}
		
		return null;
	}
	
	public void EscapeFromFire(GameObject target)
	{
		if (_monsters.ContainsKey(target))
		{
			IMonster monster = _monsters[target];
			
			monster.StopBeingHit(.5f);
		}
	}
	
	void MonsterKilled(GameObject go)
	{
		_monsters.Remove(go);
		
		monsterCount--;
	}
	
	private GameObject _monstersGO;
	private Dictionary<GameObject, IMonster> _monsters;
}

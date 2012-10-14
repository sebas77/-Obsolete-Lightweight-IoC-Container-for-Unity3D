using System;
using UnityEngine;

public interface IMonsterSystem
{
	int monsterCount { get; }
	
	void Add<M>(M monster) where M:MonoBehaviour, IMonster;
	
	IMonster 	SetUnderFire(GameObject target);
	void 		EscapeFromFire(GameObject target);
}


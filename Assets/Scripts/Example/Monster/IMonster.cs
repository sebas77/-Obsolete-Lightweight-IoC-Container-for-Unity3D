using System;

public interface IMonster
{
	event Action OnKilled;
	
	void StartBeingHit(float energyPerSecond);
	void StopBeingHit(float energyPerSecond);
}


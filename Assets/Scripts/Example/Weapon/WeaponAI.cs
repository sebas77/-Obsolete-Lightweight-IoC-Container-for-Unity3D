using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum WeaponState
{
	idle,
	fire
}

public class WeaponAI : MonoBehaviour 
{
	[IoC.Inject] public IMonsterSystem 				monsterSystem 			{ set; private get; }
	
	// Use this for initialization
	void Start () 
	{
		this.Inject();
		
		DesignByContract.Check.Require(monsterSystem != null);
		
		_lockedTarget = null;
			
		_currentState = WeaponState.idle;
		
		PlayIdle ();
	}

	void PlayIdle()
	{
		_tweener = HOTween.To(this.transform, 4, new TweenParms() 
		   .Prop("rotation", new Vector3(0, 180, 0)) 
		   .Ease(EaseType.EaseInOutQuad) 
		   .Loops(-1, LoopType.Yoyo) 
		 );
		
		_tweener.Play();
	}
	
	void OnTriggerEnter (Collider other) 
	{
		if (_lockedTarget == null)
		{
	    	Fire();
			
			_lockedTarget = other.gameObject;
			
			IMonster monster = monsterSystem.SetUnderFire(_lockedTarget);
			
			if (monster != null)
				monster.OnKilled += () => { TargetKilledOrEscaped(_lockedTarget); };
		}
	}
	
	void OnTriggerExit (Collider other) 
	{
		TargetKilledOrEscaped(other.gameObject);
	}

	void TargetKilledOrEscaped(GameObject target)
	{
		if (_lockedTarget != null && Object.ReferenceEquals(_lockedTarget, target))
		{
			//Check if there are other targets
			RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, (this.collider as SphereCollider).radius, Vector3.zero);
			
			if (hits.Length > 0)
				_lockedTarget = hits[0].transform.gameObject;
			else
			{
				monsterSystem.EscapeFromFire(_lockedTarget);
				
				_lockedTarget = null;
			
				Idle();
			}
		}
	}
	
	void Update()
	{
		if (_currentState == WeaponState.fire)
		{
			transform.LookAt(_lockedTarget.transform);
		}
	}
	
	void Fire()
	{
		_currentState = WeaponState.fire;
		
		if (_tweener != null)
		{
			_tweener.Kill();
			
			_tweener = null;
		}
	}
	
	void Idle()
	{
		_currentState = WeaponState.idle;
		
		PlayIdle();
	}
	
	WeaponState 	_currentState;
	Tweener			_tweener;
	GameObject		_lockedTarget;
}

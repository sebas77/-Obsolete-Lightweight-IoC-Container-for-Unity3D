using System;
using UnityEngine;
using Holoville.HOTween;

public class MonsterPathFollower:MonoBehaviour
{
	[IoC.Inject] public PathController		pathController 			{ set; private get; }
	
	void Start()
	{
		transform.position = pathController.CheckPoint(currentCheckPoint);
		
		MoveNext();
	}
	
	void MoveNext()
	{
		if (pathController.IsEndReached(currentCheckPoint) == false)
		{
			TweenParms paramaters = new TweenParms().Prop("position", pathController.CheckPoint(currentCheckPoint + 1)).Ease(EaseType.Linear).OnComplete(MoveNext);
			
			Tweener tweener = HOTween.To(this.transform, 2, paramaters);
			
			tweener.Play();
			
			currentCheckPoint++;
		}
		else
		{
			GetComponent<Monster>().CommitSuicide();
		}
		
	}
	
	private int currentCheckPoint = 0;
}


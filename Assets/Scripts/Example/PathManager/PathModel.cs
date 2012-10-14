using System;
using UnityEngine;

public class PathModel:MonoBehaviour
{
	[IoC.Inject] public PathController pathController { private get; set; }
	
	public GameObject[] placeHolders;
	
	void Start()
	{
		this.Inject();
		
		pathController.pathDTO = this.placeHolders;
		
		GameObject.Destroy(this);
	}
}


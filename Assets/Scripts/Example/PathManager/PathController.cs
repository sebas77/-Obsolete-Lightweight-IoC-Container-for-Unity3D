using System;
using UnityEngine;
using Holoville.HOTween;

public class PathController
{
	public GameObject[] pathDTO {private get; set;}
	
	public Vector3 CheckPoint(int index)
	{
		return pathDTO[index].transform.position;
	}
	
	public bool IsEndReached(int check) { return check >= pathDTO.Length - 1; }
}




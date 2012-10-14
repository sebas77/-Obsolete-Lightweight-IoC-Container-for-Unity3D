using System.Collections.Generic;
using UnityEngine;

public class TickBehaviour:MonoBehaviour
{
	void Awake()
	{
		_ticked = new List<ITickable>();
	}
	
	public void Add(ITickable tickable)
	{
		_ticked.Add(tickable);
	}
	
	void Update()
	{
		foreach (ITickable tickable in _ticked)
			tickable.Tick(Time.deltaTime);
	}
	
	private List<ITickable> _ticked;
}

public class TickEngine
{
	public TickEngine ()
	{
		GameObject go = GameObject.Find("Ticker");
		
		if (go == null)
		{
			go = new GameObject("Ticker");
			
			_ticker = go.AddComponent<TickBehaviour>();
		}
		else
			_ticker = go.GetComponent<TickBehaviour>();
	}
	
	public void Add(ITickable tickable)
	{
		_ticker.Add(tickable);
	}
	
	private TickBehaviour 	_ticker;
}
	



 
using UnityEngine;
using System;

public class ContextMessage
{
	static public readonly string ADDED = "OnAdded";
}

public class UnityContext<T>: MonoBehaviour where T:IContextRoot, new()
{
	void Awake()
	{
		Debug.Log("UnityContext Awaked");
		
		_applicationRoot = new T();
	}
	
	//
	// Defining OnEnable as fix for UnityEngine execution order bug
	//
	void OnEnable()
	{	
		Debug.Log("UnityContext Enabled");
	}
	
	void OnAdded(MonoBehaviour component)
	{
		DesignByContract.Check.Require(_applicationRoot != null && _applicationRoot.container != null, "Container not initialized correctly, possible script execution order problem");
				
		_applicationRoot.container.Inject(component);
	}
	
	private T 				_applicationRoot;
}
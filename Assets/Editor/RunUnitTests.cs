using UnityEditor;
using UnityEngine;

public class RunUnitTestsClass: ScriptableObject 
{
	[MenuItem("Assets/Run Unit Tests")]
	static public void RunUnitTests()
	{
		new CSharpTestDriverSimple();
	}
}


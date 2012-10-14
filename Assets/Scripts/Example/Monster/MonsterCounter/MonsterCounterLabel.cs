using UnityEngine;

public class MonsterCounterLabel : MonoBehaviour 
{
	[IoC.Inject] public IMonsterSystem monsterSystem { private get; set; }
	
	void Start()
	{
		this.Inject();
		
		DesignByContract.Check.Assert(monsterSystem != null, "MonsterCounterLabel - monsterSystem non correctly Injected");
	}
	
	void OnGUI () 
	{
		GUIStyle style = new GUIStyle();
		
		style.fontSize = 30;
		
    	GUI.Label (new Rect (10, 10, 100, 30), monsterSystem.monsterCount.ToString(), style);
	}
}

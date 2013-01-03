using UnityEngine;

public static class MonoInjection
{
	public static void Inject(this MonoBehaviour script)
	{
		script.SendMessageUpwards(ContextMessage.ADDED, script, SendMessageOptions.DontRequireReceiver);
	}
}
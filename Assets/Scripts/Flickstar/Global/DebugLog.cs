using UnityEngine;
using UnityEngine.UI;

public class DebugLog : MonoBehaviour
{
	public Text message = null;

	private void Awake()
	{
//		message.text = "";
		Application.logMessageReceived  += HandleLog;
	}

	private void OnDestroy()
	{
		Application.logMessageReceived  += HandleLog;
	}

	private void HandleLog( string logText, string stackTrace, LogType type )
	{
		message.text = logText + "\n" + message.text;
	}
} 
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// あるある１２「Animatorのブレンド中のイベント」の解説スクリプト
/// </summary>
public class AnimationEventReceiver : MonoBehaviour
{
	/// <summary>
	/// ログ
	/// </summary>
	public Queue<string> logs = new Queue<string>();

	/// <summary>
	/// ログ追加
	/// </summary>
	/// <param name="log"></param>
	public void Log(string log)
	{
		ShowLog(log);
	}

	/// <summary>
	/// Animation Event Callback
	/// </summary>
	/// <param name="log"></param>
	private void ShowLog(string log)
	{
		if (logs.Count >= 500)
			logs.Dequeue();

		logs.Enqueue(log);
	}

	/// <summary>
	/// Animation Event Callback
	/// </summary>
	/// <param name="log"></param>
	private void ShowMesh(int number)
	{
		if (logs.Count >= 50)
			logs.Dequeue();

		logs.Enqueue("ShowMesh : " + number);
	}
}

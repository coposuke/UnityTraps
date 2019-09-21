using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// あるある１２「Animatorのブレンド中のイベント」の解説スクリプト
/// </summary>
public class ShowMeshEventReceiver : MonoBehaviour
{
	/// <summary>
	/// 切り替えるメッシュたち
	/// </summary>
	[SerializeField]
	private MeshRenderer[] meshRenderes = null;

	/// <summary>
	/// Animation Event Callback
	/// </summary>
	/// <param name="log"></param>
	private void ShowMesh(int meshNumber)
	{
		for (int i = 0; i < meshRenderes.Length; ++i)
		{
			meshRenderes[i].enabled = (meshNumber - 1) == i;
		}
	}

	/// <summary>
	/// メッシュのリセット
	/// </summary>
	public void ResetMesh()
	{
		var animator = GetComponent<Animator>();

		var state = animator.GetCurrentAnimatorStateInfo(0);
		var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

		var events = clip.events;
		var time = state.normalizedTime * clip.length;

		for (int i = 0; i < events.Length; ++i)
		{
			var e = events[i];
			if (e.time < time)
			{
				if (e.functionName == "ShowMesh")
				{
					SendMessage(e.functionName, e.intParameter);
				}
			}
		}
	}
}

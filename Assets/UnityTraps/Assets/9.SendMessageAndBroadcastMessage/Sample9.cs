using System.Collections;
using System.Diagnostics;
using UnityEngine;


///<summary>
/// あるある９「SendMessageとBroadcastMessage」の解説スクリプト
///</summary>
public class Sample9 : MonoBehaviour
{
	/// <summary>
	/// 子階層に生成する個数
	/// </summary>
	[Tooltip("子階層に生成する個数")]
	public int childCount;

	/// <summary>
	/// 孫階層に生成する深さ
	/// </summary>
	[Tooltip("孫階層に生成する深さ")]
	public int grandChildDepth;

	/// <summary>
	/// StartMessages Methodの呼び出し回数
	/// </summary>
	private int callCount;


#if true
	/// <summary>
	/// Unity Event Start
	/// </summary>
	private IEnumerator Start()
	{
		Setup();

		for (int i = 0; i < 10000; ++i)
		{
			StartMessages();
			yield return new WaitForSecondsRealtime(1.0f);
		}

		yield break;
	}
#else
	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		// Invoke系メソッド上でのBroadcastMessageはSendMessageと効果範囲が同じになってしまう模様
		// つまり子階層以下は検索されない為、SendMessageと同等の速度で処理される
		// ※Unity2018.1.0.f2での検証

		Invoke("StartMessages", 0.0f);
		InvokeRepeating("StartMessages", 1.0f, 1.0f);

		// ちなみに、Invoke系メソッドは非アクティブでも動作します
		// Invoke系メソッドはこのコンポーネント内のメソッドしか呼び出せないので、
		// さほど運用しにくいものではないですが、注意するに越したことはありません。
	}
#endif

	/// <summary>
	/// GameObjectとComponentを大量生産する準備
	/// </summary>
	private void Setup()
	{
		var parent = this.transform;
		var newOriginalObject = new GameObject("ChildObject");
		var newOriginalTransform = newOriginalObject.transform;

		for (int i = 0; i < grandChildDepth; ++i)
		{
			var newChildObject = new GameObject("GrandChildObject");
			var newChildTransform = newChildObject.transform;
			newChildTransform.SetParent(newOriginalTransform);
			newOriginalTransform = newChildTransform;
		}

		newOriginalTransform.gameObject.AddComponent<Child>();

		for (int i = 0; i < childCount; ++i)
		{
			var newChildObject = Instantiate(newOriginalObject);
			newChildObject.transform.SetParent(parent);
		}

		DestroyImmediate(newOriginalObject);
	}

	/// <summary>
	/// SendMessageとBroadcastMessageのタイムを一挙に図る
	/// </summary>
	private void StartMessages()
	{
		var sw = new Stopwatch();
		var child = GetComponent<Child>();
		var second = 0.0;
		var toMicroSec = Mathf.Pow(10, 6);
		var count = callCount++;

		//------------
		sw.Restart();
		child.SetValue(count);
		sw.Stop();

		second = (double)sw.ElapsedTicks / (double)Stopwatch.Frequency;
		UnityEngine.Debug.Log("1.Call SetValue : " + (second * toMicroSec) + "マイクロ秒");

		//------------
		sw.Restart();
		SendMessage("SetValue", count);
		sw.Stop();

		second = (double)sw.ElapsedTicks / (double)Stopwatch.Frequency;
		UnityEngine.Debug.Log("2.SendMessage : " + (second * toMicroSec) + "マイクロ秒");

		//------------
		sw.Restart();
		BroadcastMessage("SetValue", count);
		sw.Stop();

		second = (double)sw.ElapsedTicks / (double)Stopwatch.Frequency;
		UnityEngine.Debug.Log("3.BroadcastMessage : " + (second * toMicroSec) + "マイクロ秒");

		//------------
		UnityEngine.Debug.Log("--------------------");
	}
}

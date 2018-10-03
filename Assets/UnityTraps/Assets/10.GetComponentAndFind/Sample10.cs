#pragma warning disable 0219 // ローカル変数の未使用警告

using UnityEngine;


///<summary>
/// あるある１０「InactiveをGetComponent/Find」の解説スクリプト
///</summary>
public class Sample10 : MonoBehaviour
{
	///<summary>
	/// Unity Event Start
	///</summary>
	private void Start()
	{
		// "Stage"ノードから子の"Enemy**"を取得するコードです。
		// "Enemy01"はアクティブなので問題なく取得できますが、
		// "Enemy02"は非アクティブなので、一部取得できないメソッドがあります。
		// "Enemy03"も非アクティブなので、一部取得できないメソッドがあります。親の"Stage03"も非アクティブです。

		const string Eneny01Name = "Enemy01";
		const string Eneny02Name = "Enemy02(deactive)";
		const string Eneny03Name = "Enemy03(parent-deactive)";

		var world = GameObject.Find("/World").transform;
		var stage01 = world.Find("Stage01").transform;           // OK
		var stage02 = world.Find("Stage02").transform;           // OK
		var stage03 = world.Find("Stage03(deactive)").transform; // OK

		// ①Transform.Find
		var enemy01 = stage01.Find(Eneny01Name); // OK
		var enemy02 = stage02.Find(Eneny02Name); // OK
		var enemy03 = stage03.Find(Eneny03Name); // OK

		// ②GameObject.Find(static関数)
		var enemy01g = GameObject.Find("/World/Stage01/" + Eneny01Name); // OK
		var enemy02g = GameObject.Find("/World/Stage01/" + Eneny02Name); // NG

		// ③Component.GetComponent
		var enemy01c = enemy01.GetComponent<Child>(); // OK
		var enemy02c = enemy02.GetComponent<Child>(); // OK

		// ④Component.GetComponentInChildren
			enemy01c = stage01.GetComponentInChildren<Child>();  // OK
		var enemy01ar= stage01.GetComponentsInChildren<Child>(); // OK
			enemy02c = stage02.GetComponentInChildren<Child>();  // NG
		var enemy02ar= stage02.GetComponentsInChildren<Child>(); // NG

		// ⑤Component.GetComponentInChildren(true)
		enemy01c  = stage01.GetComponentInChildren<Child>(true);  // OK
		enemy01ar = stage01.GetComponentsInChildren<Child>(true); // OK
		enemy02c  = stage02.GetComponentInChildren<Child>(true);  // OK
		enemy02ar = stage02.GetComponentsInChildren<Child>(true); // OK

		// ⑥Component.GetComponentInParent
		var stage01c  = enemy01.GetComponentInParent<Parent>();  // OK
		var stage01ar = enemy01.GetComponentsInParent<Parent>(); // OK
		var stage03c  = enemy03.GetComponentInParent<Parent>();  // NG
		var stage03ar = enemy03.GetComponentsInParent<Parent>(); // NG

		// ⑦Component.GetComponentsInParent(true)
		stage01ar = enemy01.GetComponentsInParent<Parent>(true); // OK
		stage03ar = enemy03.GetComponentsInParent<Parent>(true); // OK

		// ⑧Object.FindObject(s)OfType
		var obj = Object.FindObjectOfType<Child>();  // "Enmey01"
		var objs= Object.FindObjectsOfType<Child>(); // "Enemy01"のみ

		// ⑨Resources.FindObjectsOfTypeAll
		objs = Resources.FindObjectsOfTypeAll<Child>(); // "Enmey01"～"03" 

		// 子階層"のみ"、同じGameObject"のみ"を検索するメソッドはOK。
		// ・①Transform.Find
		// ・③Component.GetComponent

		// 更に深い階層を検索するメソッドはNG。
		// 全階層を検索するメソッドも同様にNG。
		// ・②GameObject.Find
		// ・④Component.GetComponentInChildren (⑤で回避可能)
		// ・⑥Component.GetComponentInParent   (⑦で回避可能)
		// ・⑧Object.FindObject(s)OfType

		// ロードされているオブジェクトから全検索する特殊メソッド。取り扱いは注意が必要です。
		// ・⑨Resources.FindObjectsOfTypeAll
	}
}

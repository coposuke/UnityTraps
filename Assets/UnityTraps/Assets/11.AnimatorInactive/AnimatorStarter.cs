using UnityEngine;


/// <summary>
/// あるある１１「InactiveなAnimatorの記憶喪失」の解説スクリプト
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorStarter : MonoBehaviour
{
	/// <summary>
	/// 型
	/// </summary>
	public enum Type
	{
		Int, Float, Bool, Trigger
	}

	/// <summary>
	/// 設定値
	/// </summary>
	[System.Serializable]
	public class KeyPairValue
	{
		public string name;
		public Type type;
		public string value;
	}

	/// <summary>
	/// Animator Component
	/// </summary>
	private Animator animator;

	/// <summary>
	/// 設定値
	/// </summary>
	[SerializeField]
	private KeyPairValue[] values = null;


	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		animator = GetComponent<Animator>();

		foreach (var pair in values)
		{
			switch (pair.type)
			{
			case Type.Int: animator.SetInteger(pair.name, int.Parse(pair.value)); break;
			case Type.Float: animator.SetFloat(pair.name, float.Parse(pair.value)); break;
			case Type.Bool: animator.SetBool(pair.name, bool.Parse(pair.value)); break;
			case Type.Trigger: animator.SetTrigger(pair.name); break;
			}
		}
	}
}

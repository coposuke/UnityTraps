using UnityEngine;


/// <summary>
/// あるある１１「InactiveなAnimatorの記憶喪失」の解説スクリプト
/// Animatorのモーション変更
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorChanger : MonoBehaviour
{
	/// <summary>
	/// Animator Component
	/// </summary>
	private Animator animator;

	/// <summary>
	/// Animation.Animation01に変更(Posあり, Rot=0.0, Scale=1.0)
	/// </summary>
	[SerializeField, Tooltip("Animation.Animation01に変更(Posあり, Rot=0.0, Scale=1.0)")]
	private bool change1;

	/// <summary>
	/// Animation.Animation02に変更(Posあり, Rot=Random, Scale=Random)
	/// </summary>
	[SerializeField, Tooltip("Animation.Animation02に変更(Posあり, Rot=Random, Scale=Random)")]
	private bool change2;

	/// <summary>
	/// Animation.Animation03に変更(Posのみ)
	/// </summary>
	[SerializeField, Tooltip("Animation.Animation03に変更(Posのみ)")]
	private bool change3;


	/// <summary>
	/// Unity Event Awake
	/// </summary>
	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Unity Event OnEnable
	/// </summary>
	private void OnEnable()
	{
		var dispatcher = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (dispatcher)
			dispatcher.onStateExit += OnStateExit;
	}

	/// <summary>
	/// Unity Event OnDestroy
	/// </summary>
	private void OnDestroy()
	{
		var dispatcher = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (dispatcher)
			dispatcher.onStateExit -= OnStateExit;
	}

	/// <summary>
	/// Unity Event OnValidate
	/// </summary>
	private void OnValidate()
	{
		if (animator == null)
			animator = GetComponent<Animator>();

		if (change1) animator.SetTrigger("Change1");
		else         animator.ResetTrigger("Change1");

		if (change2) animator.SetTrigger("Change2");
		else         animator.ResetTrigger("Change2");

		if (change3) animator.SetTrigger("Change3");
		else         animator.ResetTrigger("Change3");
	}

	/// <summary>
	/// AnimatorStateMachineDispather Callback OnStateExit
	/// </summary>
	private void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		change1 = change2 = change3 = false;
	}

	/// <summary>
	/// 解決策2
	/// </summary>
	[ContextMenu("解決策2")]
	private void Solution2()
	{
		gameObject.SetActive(false);

		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		gameObject.SetActive(true);
	}
}

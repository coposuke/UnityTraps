using UnityEngine;


/// <summary>
/// あるある１１「InactiveなAnimatorの記憶喪失」の解説スクリプト
/// Animationのモーション変更
/// </summary>
[RequireComponent(typeof(Animation))]
public class AnimationChanger : MonoBehaviour
{
	/// <summary>
	/// Animation Component
	/// </summary>
	private new Animation animation;

	/// <summary>
	/// Animation 名前
	/// </summary>
	[SerializeField]
	private string [] animationNames = {
		"Animation.Animation01",
		"Animation.Animation02",
		"Animation.Animation03",
	};

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
	/// CrossFadeかどうか
	/// </summary>
	[SerializeField]
	private bool isCrossFade;

	/// <summary>
	/// CrossFade時間
	/// </summary>
	[SerializeField]
	private float crossFadeTime;


	/// <summary>
	/// Unity Event Awake
	/// </summary>
	private void Awake()
	{
		animation = GetComponent<Animation>();
	}

	/// <summary>
	/// Unity Event OnValidate
	/// </summary>
	private void OnValidate()
	{
		if (animation == null)
			animation = GetComponent<Animation>();

		if (isCrossFade)
		{
			if (change1) animation.CrossFade(animationNames[0], crossFadeTime);
			if (change2) animation.CrossFade(animationNames[1], crossFadeTime);
			if (change3) animation.CrossFade(animationNames[2], crossFadeTime);
		}
		else
		{
			if (change1) animation.Play(animationNames[0]);
			if (change2) animation.Play(animationNames[1]);
			if (change3) animation.Play(animationNames[2]);
		}
		change1 = change2 = change3 = false;
	}
}

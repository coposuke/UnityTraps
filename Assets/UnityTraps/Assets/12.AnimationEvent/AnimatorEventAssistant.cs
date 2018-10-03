using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// あるある１２「Animatorのブレンド中のイベント」の解説スクリプト
/// 本書で解説しきれなかった「解決 その１」の実装例
/// </summary>
/// <remarks>
/// ・SendMessage()はロード時にリフレクションでメソッドをキャッシュしておくとより高速にできます
/// ・tagHashでStateを判断してますが、Editorで事前にStateのリストを作るのも良いですし、Clip名等外部データと連携するのも良いです。
/// </remarks>
[RequireComponent(typeof(Animator))]
public class AnimatorEventAssistant : MonoBehaviour
{
	/// <summary>
	/// Asistant用AnimatorState情報
	/// </summary>
	private class AssistantStateInfo
	{
		public int tagHash;
		public AnimationClip clip;
		public AnimationEvent[] events;
		public float playingTime;
		public float playingTimePrevFrame;
	}

	/// <summary>
	/// Animator Component
	/// </summary>
	private Animator animator;

	/// <summary>
	/// AsistantStateInfoの集合
	/// </summary>
	private Dictionary<int, AssistantStateInfo> assitantDictionary = new Dictionary<int, AssistantStateInfo>();

	/// <summary>
	/// 現在再生しているStateに対応したAsistantStateInfo
	/// </summary>
	private AssistantStateInfo assistantInfo;


	/// <summary>
	/// Unity Event Awake
	/// </summary>
	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.fireEvents = false; // Animatorによるイベント発信を防ぎます
	}

	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		Load();
	}

	/// <summary>
	/// Unity Event OnEnable
	/// </summary>
	private void OnEnable()
	{
		var sm = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (sm != null)
		{
			sm.onStateEnter += OnStateEnter;
			sm.onStateUpdate += OnStateUpdate;
			sm.onStateExit += OnStateExit;
		}
	}

	/// <summary>
	/// Unity Event OnDisable
	/// </summary>
	private void OnDisable()
	{
		var sm = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (sm != null)
		{
			sm.onStateEnter -= OnStateEnter;
			sm.onStateUpdate -= OnStateUpdate;
			sm.onStateExit -= OnStateExit;
		}
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateEnter
	/// </summary>
	private void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		AssistantStateInfo backup = assistantInfo;

		if (!assitantDictionary.TryGetValue(stateInfo.tagHash, out assistantInfo))
		{
			// 動的にClipを挿げ替えたり、初期化タイミングの都合でAssistantInfoがない場合
			// 現在あるいは次回に再生しているクリップ情報を受け取りAssistantInfoを作る
			Load(animator.GetCurrentAnimatorClipInfo(layerIndex));
			Load(animator.GetNextAnimatorClipInfo(layerIndex));

			// StateにClipがついていない場合はAssistantInfoがないのでBackupに戻す
			if (!assitantDictionary.TryGetValue(stateInfo.tagHash, out assistantInfo))
				assistantInfo = backup;
		}

		if (assistantInfo.tagHash == stateInfo.tagHash)
		{
			assistantInfo.playingTimePrevFrame = 0.0f;
			assistantInfo.playingTime = stateInfo.normalizedTime;
			FireEvents(assistantInfo.playingTime, assistantInfo.clip);
		}
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateUpdate
	/// </summary>
	private void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (assistantInfo.tagHash == stateInfo.tagHash)
		{
			var prevTime = assistantInfo.playingTime;
			var nowTime = stateInfo.normalizedTime;
			nowTime -= Mathf.FloorToInt(stateInfo.normalizedTime);

			if (nowTime < prevTime)
			{
				FireEvents(prevTime, 1.0f, assistantInfo);
				FireEvents(nowTime, assistantInfo.clip);
				prevTime = 0.0f;
			}
			else
			{
				FireEvents(prevTime, nowTime, assistantInfo);
			}

			assistantInfo.playingTimePrevFrame = prevTime;
			assistantInfo.playingTime = nowTime;
		}
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateExit
	/// </summary>
	private void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (assistantInfo.tagHash == stateInfo.tagHash)
		{
			var prevTime = assistantInfo.playingTime;
			var nowTime = stateInfo.normalizedTime;
			assistantInfo.playingTimePrevFrame = prevTime;
			assistantInfo.playingTime = nowTime;
			FireEvents(prevTime, nowTime, assistantInfo);
		}
	}

	/// <summary>
	/// 現在設定されているAnimationClipからAnimationEventのロード
	/// </summary>
	public bool Load()
	{
		if (animator == null) { return false; }

		var runtimeAnimCtrl = animator.runtimeAnimatorController;
		if (runtimeAnimCtrl == null) { return false; }

		foreach (var clip in runtimeAnimCtrl.animationClips)
		{
			var tagHash = Animator.StringToHash(clip.name + "Tag");

			// AnimationStateのタグでAnimationStateとAnimationClipの結びつけをする方法です
			// AnimationClipの名前とAnimationStateの名前が同じならタグを使う必要はありません（AnimationState.IsNameで問題なし）
			assitantDictionary[tagHash] = new AssistantStateInfo()
			{
				tagHash = tagHash,
				clip = clip,
				events = clip.events,
			};
		}

		return true;
	}

	/// <summary>
	/// 引数のAnimationClipInfoからAnimationStateInfo生成
	/// </summary>
	/// <param name="info"></param>
	private void Load(AnimatorClipInfo[] infoArray)
	{
		foreach (var info in infoArray)
		{
			var tagHash = Animator.StringToHash(info.clip.name + "Tag");
			AssistantStateInfo stateInfo = null;

			// 不明なtagHashが存在したケースのみStateInfoを追加する
			if (!assitantDictionary.TryGetValue(tagHash, out stateInfo))
			{
				assitantDictionary[tagHash] = new AssistantStateInfo()
				{
					tagHash = tagHash,
					clip = info.clip,
					events = info.clip.events,
				};
			}
		}
	}

	/// <summary>
	/// イベントの送信
	/// </summary>
	/// <param name="prevTime">前フレームの時間(normalizedTime)</param>
	/// <param name="nowTime">今フレームの時間(normalizedTime)</param>
	private void FireEvents(float prevTime, float nowTime, AssistantStateInfo assistantInfo)
	{
		prevTime *= assistantInfo.clip.length;
		nowTime *= assistantInfo.clip.length;

		for (int i = 0; i < assistantInfo.events.Length; ++i)
		{
			var e = assistantInfo.events[i];

			if (prevTime < e.time && e.time <= nowTime)
			{
				SendMessage(e.functionName, e.intParameter);
			}
		}
	}

	/// <summary>
	/// イベントの送信(再生開始時は0.0fのイベントの発信が必要)
	/// </summary>
	/// <param name="nowTime">今フレームの時間</param>
	private void FireEvents(float nowTime, AnimationClip clip)
	{
		for (int i = 0; i < assistantInfo.events.Length; ++i)
		{
			var e = assistantInfo.events[i];

			if (0.0f <= e.time && e.time <= nowTime)
			{
				SendMessage(e.functionName, e.intParameter);
			}
		}
	}
}

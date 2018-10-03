using UnityEngine;


/// <summary>
/// あるある１３「StateMachineBehaviourの呼び出し順」の解説スクリプト
/// </summary>
public class Sample13 : MonoBehaviour
{
	/// <summary>
	/// MonoBehaviour Callbackのログ出力有効/無効
	/// </summary>
	[SerializeField]
	private bool enableMonoBehaviourCallback;

	/// <summary>
	/// StateMachineBehaviour Callbackのログ出力有効/無効
	/// </summary>
	[SerializeField]
	private bool enableStateMachineBehaviourCallback;

	/// <summary>
	/// Update系 Callbackのログ出力有効/無効
	/// </summary>
	[SerializeField]
	private bool enableUpdateCallback;

	/// <summary>
	/// 現在のフレーム
	/// </summary>
	private string frame { get { return Time.frameCount.ToString().PadRight(4); } }

	/// <summary>
	/// AnimationEventReceiver
	/// </summary>
	private AnimationEventReceiver receiver { get { return (cachedReceiver != null) ? cachedReceiver : (cachedReceiver = GetComponent<AnimationEventReceiver>()); } }

	/// <summary>
	/// AnimationEventReceiverのキャッシュ
	/// </summary>
	private AnimationEventReceiver cachedReceiver;


#if UNITY_EDITOR
	/// <summary>
	/// Unity Event Reset
	/// </summary>
	private void Reset()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "Reset");
	}
#endif

	/// <summary>
	/// Unity Event Awake
	/// </summary>
	private void Awake()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "Awake");

		var animator = GetComponent<Animator>();
		var sm = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (sm != null)
		{
			sm.onStateEnter += OnStateEnter;
			sm.onStateMove += OnStateMove;
			sm.onStateSwitch += OnStateSwitch;
			sm.onStateUpdate += OnStateUpdate;
			sm.onStateExit += OnStateExit;
			sm.onStateIK += OnStateIK;
			sm.onStateMachineEnter += OnStateMachineEnter;
			sm.onStateMachineExit += OnStateMachineExit;
		}
	}

	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "Start");
	}

	/// <summary>
	/// Unity Event OnEnable
	/// </summary>
	private void OnEnable()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "OnEnable");
	}

	/// <summary>
	/// Unity Event OnDisable
	/// </summary>
	private void OnDisable()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "OnDisable");
	}

	/// <summary>
	/// Unity Event OnDestroy
	/// </summary>
	private void OnDestroy()
	{
		if (enableMonoBehaviourCallback)
			Log(frame + "OnDisable");

		var animator = GetComponent<Animator>();
		var sm = animator.GetBehaviour<AnimatorStateMachineDispatcher>();
		if (sm != null)
		{
			sm.onStateEnter -= OnStateEnter;
			sm.onStateMove -= OnStateMove;
			sm.onStateSwitch -= OnStateSwitch;
			sm.onStateUpdate -= OnStateUpdate;
			sm.onStateExit -= OnStateExit;
			sm.onStateIK -= OnStateIK;
			sm.onStateMachineEnter -= OnStateMachineEnter;
			sm.onStateMachineExit -= OnStateMachineExit;
		}
	}

	/// <summary>
	/// Unity Event Update
	/// </summary>
	private void Update()
	{
		if(enableMonoBehaviourCallback && enableUpdateCallback)
			Log(frame + "Update");
	}

	/// <summary>
	/// Unity Event LateUpdate
	/// </summary>
	private void LateUpdate()
	{
		if(enableMonoBehaviourCallback && enableUpdateCallback)
			Log(frame + "LateUpdate");
	}

	/// <summary>
	/// Unity Event OnAnimatorMove
	/// </summary>
	private void OnAnimatorMove()
	{
		if(enableMonoBehaviourCallback && enableUpdateCallback)
			Log(frame + "OnAnimatorMove");
	}

	/// <summary>
	/// Unity Event OnAnimatorIK
	/// </summary>
	private void OnAnimatorIK(int layerIndex)
	{
		if(enableMonoBehaviourCallback && enableUpdateCallback)
			Log(frame + "OnAnimatorIK");
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateEnter
	/// </summary>
	private void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(enableStateMachineBehaviourCallback)
			Log(frame + "OnStateEnter : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateUpdate
	/// </summary>
	private void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(enableStateMachineBehaviourCallback && enableUpdateCallback)
			Log(frame + "OnStateUpdate : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateExit
	/// </summary>
	private void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(enableStateMachineBehaviourCallback)
			Log(frame + "OnStateExit : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateSwitch
	/// </summary>
	private void OnStateSwitch(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (enableStateMachineBehaviourCallback)
			Log(frame + "OnStateSwitch : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateMove
	/// </summary>
	private void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(enableStateMachineBehaviourCallback && enableUpdateCallback)
			Log(frame + "OnStateMove : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateIK
	/// </summary>
	private void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(enableStateMachineBehaviourCallback && enableUpdateCallback)
			Log(frame + "OnStateIK : " + GetStateName(stateInfo));
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateMachineEnter
	/// </summary>
	private void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		if(enableStateMachineBehaviourCallback)
			Log(frame + "OnStateMachineEnter");
	}

	/// <summary>
	/// StateMachineBehaviour Callback OnStateMachineExit
	/// </summary>
	private void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		if(enableStateMachineBehaviourCallback)
			Log(frame + "OnStateMachineExit");
	}

	/// <summary>
	/// AnimationStateの名前を取得
	/// </summary>
	/// <param name="stateInfo"></param>
	private string GetStateName(AnimatorStateInfo stateInfo)
	{
		return
			(stateInfo.IsName("Animation01")) ? "Animation01":
			(stateInfo.IsName("Animation02")) ? "Animation02":
			(stateInfo.IsName("Animation03")) ? "Animation03" : "Unknown";
	}

	/// <summary>
	/// ログ出力
	/// </summary>
	/// <param name="log"></param>
	private void Log(string log)
	{
		if (receiver != null)
		{
			receiver.Log(log);
		}
	}
}

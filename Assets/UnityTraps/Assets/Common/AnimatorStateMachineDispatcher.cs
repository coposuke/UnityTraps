using System;
using UnityEngine;


/// <summary>
/// あるある解説用のステートマシンのイベント通知
/// </summary>
public class AnimatorStateMachineDispatcher : StateMachineBehaviour
{
	public event Action<Animator, AnimatorStateInfo, int> onStateEnter;
	public event Action<Animator, AnimatorStateInfo, int> onStateUpdate;
	public event Action<Animator, AnimatorStateInfo, int> onStateSwitch;
	public event Action<Animator, AnimatorStateInfo, int> onStateExit;
	public event Action<Animator, AnimatorStateInfo, int> onStateMove;
	public event Action<Animator, AnimatorStateInfo, int> onStateIK;
	public event Action<Animator, int> onStateMachineEnter;
	public event Action<Animator, int> onStateMachineExit;

	private int switchCounter = -1; // 最初は必ずEnterから始まるので-1しておく


	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onStateEnter?.Invoke(animator, stateInfo, layerIndex);

		if (++switchCounter % 2 == 0)
			onStateSwitch?.Invoke(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onStateUpdate?.Invoke(animator, stateInfo, layerIndex);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onStateExit?.Invoke(animator, stateInfo, layerIndex);

		if (++switchCounter % 2 == 0)
			onStateSwitch?.Invoke(animator, stateInfo, layerIndex);
	}

	public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onStateMove?.Invoke(animator, stateInfo, layerIndex);
	}

	public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		onStateIK?.Invoke(animator, stateInfo, layerIndex);
	}

	public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		onStateMachineEnter?.Invoke(animator, stateMachinePathHash);
	}

	public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		onStateMachineExit?.Invoke(animator, stateMachinePathHash);
	}
}

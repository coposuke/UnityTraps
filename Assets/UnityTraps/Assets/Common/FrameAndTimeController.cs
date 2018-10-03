using UnityEngine;


public class FrameAndTimeController : MonoBehaviour
{
	[SerializeField]
	private int frameRate = -1;


	private void Update()
	{
		Application.targetFrameRate = frameRate;
	}
}

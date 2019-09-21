using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// あるある２２「RendererのBouds」の解説スクリプト
/// </summary>
public class Sample22 : MonoBehaviour
{
	[SerializeField]
	private Transform cameraTransform = null;

	[SerializeField]
	private Transform [] bones = null;

	private float time = 0.0f;

	private Vector3 headPosition;
	private Vector3 trailPosition;

	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		headPosition = bones[0].localPosition;
		trailPosition = bones[bones.Length - 1].localPosition;
	}

	/// <summary>
	/// Unity Event Update
	/// </summary>
	private void Update()
	{
		time += Time.deltaTime;

		float slide = -Mathf.Cos(time) * 0.5f + 0.5f;
		float length = 5f;

		var newTrailPosition = trailPosition;
		newTrailPosition.x   = trailPosition.x + slide * length;

		for (int i = 0; i < bones.Length; ++i)
			bones[i].localPosition = Vector3.Lerp(headPosition, newTrailPosition, i / (float)(bones.Length - 1));

		var cameraPosition = cameraTransform.localPosition;
		cameraPosition.x = newTrailPosition.x;
		cameraPosition.z = Mathf.Lerp(-2, -1, Mathf.Clamp(Mathf.Cos(time * 2.0f), -0.5f, 0.5f) + 0.5f);
		cameraTransform.localPosition = cameraPosition;
	}
}

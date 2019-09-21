using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// あるある２１「RendererのMaterial増殖」の解説スクリプト
/// </summary>
public class Sample21 : MonoBehaviour
{
	private class InstanceInfo
	{
		public Transform transform;
		public MeshRenderer renderer;
	}

	private const int Range = 10;

	[SerializeField]
	private MeshRenderer cubeOriginal = null;

	[SerializeField]
	private Button reloadButton = null;

	[SerializeField]
	private Button recreateButton = null;

	private InstanceInfo[,] cubes = new InstanceInfo[Range,Range];

	private float time = 0.0f;


	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		Create();

		reloadButton.onClick.AddListener(()=>
		{
			SceneManager.LoadSceneAsync("Sample21");
			//Resources.UnloadUnusedAssets(); // こちらでもOK!!
		});

		recreateButton.onClick.AddListener(() =>
		{
			time = 0.0f;
			Create();
		});
	}

	/// <summary>
	/// キューブ生成
	/// </summary>
	private void Create()
	{
		Transform parent = this.transform;

		foreach (var info in cubes)
		{
			if (info != null && info.transform != null)
				DestroyImmediate(info.transform.gameObject);
		}

		for (int x = 0; x < Range; ++x)
		{
			for (int y = 0; y < Range; ++y)
			{
				var cubeObject = Instantiate(cubeOriginal.gameObject, new Vector3(x - Range * 0.5f + 0.5f, 0f, y), Quaternion.identity, parent);
				var cuberenderer = cubeObject.GetComponent<MeshRenderer>();
				Debug.Log(cuberenderer.sharedMaterial);
				cuberenderer.material.color = Color.yellow;
				Debug.Log(cuberenderer.sharedMaterial);
				cubeObject.SetActive(true);

				cubes[x, y] = new InstanceInfo()
				{
					transform = cubeObject.transform,
					renderer = cuberenderer,
				};
			}
		}

	}

	/// <summary>
	/// Unity Event Update
	/// </summary>
	private void Update()
	{
		this.time += Time.deltaTime;

		for (int x = 0; x < Range; ++x)
		{
			for (int y = 0; y < Range; ++y)
			{
				var instance = cubes[x,y];

				var position = instance.transform.localPosition;
				int distance = Mathf.FloorToInt(Vector2.Distance(new Vector2(x, y), new Vector2(Range * 0.5f - 0.5f, Range * 0.5f - 0.5f)));
				position.y = Mathf.Sin(-distance + time * 4.0f) * 0.25f;
				instance.transform.localPosition = position;

				instance.renderer.material.color = Color.HSVToRGB(
					time * 0.25f % 1.0f,
					Mathf.Sin(-distance + time * 4.0f) * 0.5f + 0.5f,
					1f
				);
			}
		}
	}
}

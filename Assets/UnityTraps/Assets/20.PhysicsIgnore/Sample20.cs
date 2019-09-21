using System;
using UnityEngine;


/// <summary>
/// あるある２０「ColliderのOnXXExitが呼ばれない」の解説スクリプト
/// </summary>
public class Sample20 : MonoBehaviour
{
	[SerializeField]
	private TextMesh textOriginal = null;

	[SerializeField]
	private Collider triggerIgnore = null;

	[SerializeField]
	private Collider triggerActive = null;

	[SerializeField]
	private Collider[] colliders = null;

	private float time = -1.0f;

	private bool which = true;


	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		Action<MonoBehaviour, Collider> onEnter = (MonoBehaviour self, Collider other) =>
		{
			FireText(other.transform, "onEnter");
			self.GetComponent<MeshRenderer>().material.color = new Color(1,0,0,0.25f);
		};

		Action<MonoBehaviour, Collider> onExit = (MonoBehaviour self, Collider other) =>
		{
			FireText(other.transform, "onExit");
		};

		var listenerIgnore = triggerIgnore.gameObject.AddComponent<ColliderListener>();
		var listenerActive = triggerActive.gameObject.AddComponent<ColliderListener>();
		listenerIgnore.SetCallback(onEnter, onExit);
		listenerActive.SetCallback(onEnter, onExit);

		// ここでの処理は効果がありません
		// Sleep は ProjectSettings の Physics の sleepThreshould を -1 に設定しています
		triggerIgnore.GetComponent<Rigidbody>().sleepThreshold = -1;
		triggerActive.GetComponent<Rigidbody>().sleepThreshold = -1;
	}

	/// <summary>
	/// Unity Event Update
	/// </summary>
	private void Update()
	{
		this.time += Time.deltaTime;
		if (2.0f < this.time)
		{
			this.which = !which;
			this.time = 0.0f;

			foreach (var other in colliders)
			{
				Physics.IgnoreCollision(triggerIgnore.GetComponent<Collider>(), other, !which);
			}

			triggerActive.gameObject.SetActive(which);
		}
	}

	/// <summary>
	/// テキストを出す
	/// </summary>
	private void FireText(Transform source, string text)
	{
		var position = source.localPosition;
		position.z -= 1.0f;

		var instance = Instantiate(textOriginal.gameObject, position, Quaternion.identity, this.transform);
		var instanceText = instance.GetComponent<TextMesh>();
		instanceText.text = text;
		instance.AddComponent<TextParticle>();
	}

	/// <summary>
	/// コリジョンリスナー
	/// </summary>
	public class ColliderListener : MonoBehaviour
	{
		private Action<MonoBehaviour, Collider> onEnter;
		private Action<MonoBehaviour, Collider> onExit;
		private Material cachedMaterial;

		public void SetCallback(Action<MonoBehaviour, Collider> onEnter, Action<MonoBehaviour, Collider> onExit)
		{
			this.onEnter = onEnter;
			this.onExit = onExit;
			this.cachedMaterial = this.GetComponent<MeshRenderer>().sharedMaterial;
		}

		private void OnTriggerEnter(Collider other)
		{
			this.onEnter(this, other);
		}

		private void OnTriggerExit(Collider other)
		{
			DestroyImmediate(this.GetComponent<MeshRenderer>().material);
			this.GetComponent<MeshRenderer>().sharedMaterial = this.cachedMaterial;
			this.onExit(this, other);
		}

		private void OnCollisionEnter(Collision other)
		{
			this.onEnter(this, other.collider);
		}

		private void OnCollisionExit(Collision other)
		{
			DestroyImmediate(this.GetComponent<MeshRenderer>().material);
			this.GetComponent<MeshRenderer>().sharedMaterial = this.cachedMaterial;
			this.onExit(this, other.collider);
		}
	}

	/// <summary>
	/// テキストパーティクル(挙動と削除)
	/// </summary>
	private class TextParticle : MonoBehaviour
	{
		private const float DestroyTime = 1.0f;
		private float time = 0.0f;
		private Vector3 start;
		private Vector3 goal;

		private void Start()
		{
			float angle = UnityEngine.Random.Range(-50.0f, 50.0f);
			this.start = transform.localPosition;
			this.goal = transform.localPosition + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.back * 2.0f;
		}

		private void Update()
		{
			time += Time.deltaTime;

			var position = transform.localPosition;
			position = Vector3.Lerp(this.start, this.goal, time / DestroyTime);
			position.y = Mathf.Sin(time / DestroyTime * Mathf.PI) * 1.0f;
			transform.localPosition = position;

			if (DestroyTime < time)
				DestroyImmediate(gameObject);
		}
	}
}

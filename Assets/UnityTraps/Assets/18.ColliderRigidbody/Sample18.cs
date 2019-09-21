using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// あるある１８「ColliderとRigidbody」の解説スクリプト
/// </summary>
public class Sample18 : MonoBehaviour
{
	/// <summary>
	/// 衝突検知側の描画用メッシュ
	/// </summary>
	[SerializeField]
	private Mesh pointMesh = null;

	/// <summary>
	/// 衝突検知側の描画用マテリアル
	/// </summary>
	[SerializeField]
	private Material pointMaterial = null;

	/// <summary>
	/// 補間描画用メッシュ
	/// </summary>
	[SerializeField]
	private Mesh normalMesh = null;

	/// <summary>
	/// 補間描画用マテリアル(球面線形補間)
	/// </summary>
	[SerializeField]
	private Material normalMaterial = null;

	/// <summary>
	/// TriggerがONの状態の描画用マテリアル
	/// </summary>
	[SerializeField]
	private Material triggerMaterial = null;

	/// <summary>
	/// リスナーの親
	/// </summary>
	[SerializeField]
	private Transform listenersRoot = null;

	/// <summary>
	/// コライダーの親
	/// </summary>
	[SerializeField]
	private Transform collidersRoot = null;

	/// <summary>
	/// Trigger切り替えボタン
	/// </summary>
	[SerializeField]
	private Button button = null;


	/// <summary>
	/// Unity Callback Start
	/// </summary>
	private void Start()
	{
		Action<MonoBehaviour> onEnter = (MonoBehaviour self) =>
		{
			self.GetComponent<MeshRenderer>().material.color = Color.red;
		};

		for (int i = 0; i < listenersRoot.childCount; ++i)
			listenersRoot.GetChild(i).gameObject
				.AddComponent<ColliderListener>()
				.SetCallback(onEnter);

		for (int i = 0; i < collidersRoot.childCount; ++i)
			collidersRoot.GetChild(i).gameObject
				.AddComponent<ColliderListener>()
				.SetCallback(onEnter);

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(()=>
		{
			for (int i = 0; i < listenersRoot.childCount; ++i)
			{
				var child = listenersRoot.GetChild(i);
				var collider = child.GetComponent<CapsuleCollider>();
				if (child.GetComponent<CharacterController>() != null) continue;
				collider.isTrigger = !collider.isTrigger;

				// アウトライン着色
				if (collider.isTrigger)
				{
					var outlineObject = Instantiate(child);
					outlineObject.name = "outline";
					DestroyImmediate(outlineObject.GetComponent<Collider>());
					DestroyImmediate(outlineObject.GetComponent<Rigidbody>());
					DestroyImmediate(outlineObject.GetComponent<ColliderListener>());
					outlineObject.GetComponent<MeshRenderer>().sharedMaterial = this.triggerMaterial;
					outlineObject.transform.SetParent(child);
					outlineObject.transform.localScale = Vector3.one * 1.05f;
					outlineObject.transform.localPosition = Vector3.zero;
					outlineObject.transform.localRotation = Quaternion.identity;
				}
				else
				{
					child = child.GetChild(0);
					if (child != null) Destroy(child.gameObject);
				}
			}

			for (int i = 0; i < collidersRoot.childCount; ++i)
			{
				var child = collidersRoot.GetChild(i);
				var collider = child.GetComponent<Collider>();
				if (child.GetComponent<CharacterController>() != null) continue;
				collider.isTrigger = !collider.isTrigger;

				// アウトライン着色
				if (collider.isTrigger)
				{
					var outlineObject = Instantiate(child);
					outlineObject.name = "outline";
					DestroyImmediate(outlineObject.GetComponent<Collider>());
					DestroyImmediate(outlineObject.GetComponent<Rigidbody>());
					DestroyImmediate(outlineObject.GetComponent<ColliderListener>());
					outlineObject.GetComponent<MeshRenderer>().sharedMaterial = this.triggerMaterial;
					outlineObject.transform.SetParent(child);
					outlineObject.transform.localScale = Vector3.one * 1.05f;
					outlineObject.transform.localPosition = Vector3.zero;
					outlineObject.transform.localRotation = Quaternion.identity;
				}
				else
				{
					child = child.GetChild(0);
					if (child != null) Destroy(child.gameObject);
				}
			}
		});
	}

	/// <summary>
	/// Unity Callback Update
	/// </summary>
	private void Update()
	{
		collidersRoot.Rotate(0.0f, Time.deltaTime * 4.0f, 0.0f);
		//listenersRoot.localPosition = new Vector3(0.0f, Mathf.Sin(Time.realtimeSinceStartup) * 1e-5f, 0f);
	}

	/// <summary>
	/// コリジョンリスナー
	/// </summary>
	public class ColliderListener : MonoBehaviour
	{
		private Action<MonoBehaviour> onEnter;
		private Material cachedMaterial;

		public void SetCallback(Action<MonoBehaviour> onEnter)
		{
			this.onEnter = onEnter;
			this.cachedMaterial = this.GetComponent<MeshRenderer>().sharedMaterial;
		}

		private void OnTriggerEnter(Collider other)
		{
			onEnter(this);
		}

		private void OnTriggerExit(Collider other)
		{
			DestroyImmediate(this.GetComponent<MeshRenderer>().material);
			this.GetComponent<MeshRenderer>().sharedMaterial = this.cachedMaterial;
		}

		private void OnCollisionEnter(Collision collision)
		{
			onEnter(this);
		}

		private void OnCollisionExit(Collision collision)
		{
			DestroyImmediate(this.GetComponent<MeshRenderer>().material);
			this.GetComponent<MeshRenderer>().sharedMaterial = this.cachedMaterial;
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Debug.Log(hit.collider);
		}
	}


	[ContextMenu("Setup")]
	private void Setup()
	{
		const float length = 10.0f;
		for (int i = 0; i < 18; ++i)
		{
			var rotation = Quaternion.Euler(0.0f, i * 20, 0.0f);
			var position = rotation * Vector3.forward * length;

			var gameObject = new GameObject("Collider" + i);
			var transform = gameObject.transform;
			transform.localPosition = position;
			transform.rotation = rotation;

			var meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = normalMesh;

			var meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = normalMaterial;

			gameObject.AddComponent<BoxCollider>();

			if (i % 2 == 0)
				gameObject.AddComponent<Rigidbody>().isKinematic = true;
		}

		for (int i = 0; i < 2; ++i)
		{
			var rotation = Quaternion.Euler(0.0f, i * 20 + 170, 0.0f);
			var position = rotation * Vector3.forward * length;

			var gameObject = new GameObject("Listener" + i);
			var transform = gameObject.transform;
			transform.localPosition = position;
			transform.rotation = rotation;

			var meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.mesh = pointMesh;

			var meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = pointMaterial;

			gameObject.AddComponent<CapsuleCollider>();

			if (i % 2 == 0)
				gameObject.AddComponent<Rigidbody>().isKinematic = true;
		}
	}
}

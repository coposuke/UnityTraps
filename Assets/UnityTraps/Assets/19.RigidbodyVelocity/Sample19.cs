using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// あるある１９「Rigidbody.velocityとCharacterCollider.Move」の解説スクリプト
/// </summary>
public class Sample19 : MonoBehaviour
{
	private readonly Vector3 CharaCtrlCameraPos = new Vector3(-2, 0, -5);
	private readonly Vector3 RigidbodyCameraPos = new Vector3(2, 0, 5);
	private readonly Vector3 LookATCameraPos = new Vector3(0, -1, 0);

	[SerializeField]
	private Transform cameraTransform;

	[SerializeField]
	private CharacterController characterController;

	[SerializeField]
	private Transform characterTransform;

	[SerializeField]
	private Rigidbody objectRigidbody;

	[SerializeField]
	private Transform objectTransform;

	[SerializeField]
	private Text charaText;

	[SerializeField]
	private Text enterText;

	private bool isCharacter = true;

	private bool isRightHowToUse = false;


	/// <summary>
	/// Unity Callback Update
	/// </summary>
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			isCharacter = !isCharacter;
			charaText.text = "切替" + (isCharacter ? "（CharaController）" : "（Rigidbody）");
		}

		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
		{
			isRightHowToUse = !isRightHowToUse;
			enterText.text = "切替" + (isRightHowToUse ? "（正しい動かし方）" : "（悪い動かし方）");
		}

		UpdateCamera();
		UpdateCapsule();
	}

	/// <summary>
	/// カメラの移動
	/// </summary>
	private void UpdateCamera()
	{
		if (isCharacter)
		{
			var pos = cameraTransform.localPosition;
			pos = Vector3.Slerp(pos, CharaCtrlCameraPos, 0.1f);
			cameraTransform.localPosition = pos;
		}
		else
		{
			var pos = cameraTransform.localPosition;
			pos = Vector3.Slerp(pos, RigidbodyCameraPos, 0.1f);
			cameraTransform.localPosition = pos;
		}

		cameraTransform.LookAt(LookATCameraPos);
	}

	/// <summary>
	/// カプセルの移動
	/// </summary>
	private void UpdateCapsule()
	{
		const float Speed = 20.0f;

		// CharacterController
		if (isCharacter)
		{
			// 正しい動かし方
			if (isRightHowToUse)
			{
				Vector3 direction = Vector3.zero;

				if (Input.GetKey(KeyCode.RightArrow))
					direction = Vector3.right;
				if (Input.GetKey(KeyCode.LeftArrow))
					direction = Vector3.left;

				characterController.SimpleMove(direction * Speed);
			}
			// 悪い動かし方
			else
			{
				if (Input.GetKey(KeyCode.RightArrow))
					characterTransform.position += Vector3.right * Speed * Time.deltaTime;
				if (Input.GetKey(KeyCode.LeftArrow))
					characterTransform.position += Vector3.left * Speed * Time.deltaTime;

				// 場外に出れちゃうので防ぐ
				var pos = characterTransform.position;
				pos.x = Mathf.Clamp(pos.x, -5, 5);
				characterTransform.position = pos;
			}
		}
		// Rigidbody
		else
		{
			// 正しい動かし方
			if (isRightHowToUse)
			{
				Vector3 direction = Vector3.zero;

				if (Input.GetKey(KeyCode.LeftArrow))
					direction = Vector3.right;
				if (Input.GetKey(KeyCode.RightArrow))
					direction = Vector3.left;

				objectRigidbody.velocity = direction * Speed;
			}
			// 悪い動かし方
			else
			{
				//if (Input.GetKey(KeyCode.LeftArrow))
				//	objectTransform.position += Vector3.right * Speed * Time.deltaTime;
				//if (Input.GetKey(KeyCode.RightArrow))
				//	objectTransform.position += Vector3.left * Speed * Time.deltaTime;

				if (Input.GetKey(KeyCode.LeftArrow))
					objectRigidbody.MovePosition(objectTransform.position += Vector3.right * Speed * Time.deltaTime);
				if (Input.GetKey(KeyCode.RightArrow))
					objectRigidbody.MovePosition(objectTransform.position += Vector3.left * Speed * Time.deltaTime);
			}
		}
	}


	[ContextMenu("Setup")]
	private void Setup()
	{
		var cameraObj = GameObject.Find("Camera");
		this.cameraTransform = cameraObj.transform;

		var capsuleCharaCtrlObj = GameObject.Find("Capsule - CharacterController");
		var capsuleRigidbodyObj = GameObject.Find("Capsule - Rigidbody");
		this.characterController = capsuleCharaCtrlObj.GetComponent<CharacterController>();
		this.characterTransform = capsuleCharaCtrlObj.transform;
		this.objectRigidbody = capsuleRigidbodyObj.GetComponent<Rigidbody>();
		this.objectTransform = capsuleRigidbodyObj.transform;

		this.enterText = GameObject.Find("Message - RightMethod").GetComponent<Text>();
		this.charaText = GameObject.Find("Message - Control").GetComponent<Text>();
	}
}

using UnityEngine;


/// <summary>
/// あるある解説用のコンポーネント
/// 一画面に収まらなかった時のページング機能
/// </summary>
public class Pager : MonoBehaviour
{
	/// <summary>
	/// ページとして動かすオブジェクト
	/// </summary>
	[System.Serializable]
	public class PageTransform
	{
		[System.NonSerialized]
		public Vector3 original;
		public Transform transform;
	}

	/// <summary>
	/// ページとしての座標
	/// </summary>
	[System.Serializable]
	public class PagePosition
	{
		public Vector3 position;
	}

	/// <summary>
	/// 次へボタン
	/// </summary>
	[SerializeField]
	private UnityEngine.UI.Button nextButton = null;

	/// <summary>
	/// 前へボタン
	/// </summary>
	[SerializeField]
	private UnityEngine.UI.Button prevButton = null;

	/// <summary>
	/// ページとして動かすオブジェクト
	/// </summary>
	[SerializeField]
	private PageTransform[] pageTransforms = null;

	/// <summary>
	/// ページとしての座標
	/// </summary>
	[SerializeField]
	private PagePosition[] pagePositions = null;

	/// <summary>
	/// ページ番号
	/// </summary>
	private int page;

	/// <summary>
	/// 移動目的座標
	/// </summary>
	private Vector3 target;

	/// <summary>
	/// 移動現在座標
	/// </summary>
	private Vector3 current;

	///<summary>
	/// Unity Event Start
	///</summary>
	private void Start()
	{
		foreach (var page in pageTransforms)
		{
			page.original = page.transform.position;
		}

		if (pagePositions != null)
		{
			current = pagePositions[0].position;
		}

		RefreshButton();
	}

	///<summary>
	/// Unity Event Update
	///</summary>
	private void Update()
	{
		current = Vector3.Lerp(current, target, 10.0f * Time.deltaTime);

		for (int i = 0; i < pageTransforms.Length; ++i)
		{
			pageTransforms[i].transform.position = pageTransforms[i].original + current;
		}
	}

	/// <summary>
	/// 次のページへ
	/// </summary>
	public void NextPage()
	{
		if (pagePositions == null) { return; }
		page = Mathf.Min(page + 1, pagePositions.Length - 1);
		target = pagePositions[page].position;
		RefreshButton();
	}

	/// <summary>
	/// 前のページへ
	/// </summary>
	public void PrevPage()
	{
		if (pagePositions == null) { return; }
		page = Mathf.Max(page - 1, 0);
		target = pagePositions[page].position;
		RefreshButton();
	}

	/// <summary>
	/// ボタンの表示リフレッシュ
	/// </summary>
	private void RefreshButton()
	{
		if (pagePositions == null) { return; }
		nextButton.gameObject.SetActive(page != pagePositions.Length - 1);
		prevButton.gameObject.SetActive(page != 0);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Linq;


/// <summary>
/// あるある１６「Random.Rangeの範囲」の解説スクリプト
/// </summary>
public class Sample16 : MonoBehaviour
{
	/// <summary>
	/// テクスチャの一辺のピクセル数
	/// </summary>
	private const int SampleTexturePixel = 256;

	/// <summary>
	/// Random.Rangeの最小値
	/// </summary>
	[SerializeField, Tooltip("Random.Rangeの最小値")]
	private int RangeMin = 0;

	/// <summary>
	/// Random.Rangeの最大値
	/// </summary>
	[SerializeField, Tooltip("Random.Rangeの最大値")]
	private int RangeMax = 5;

	/// <summary>
	/// Dropdown Content
	/// </summary>
	private enum DropdownList
	{
		UnityEngine_RandomRangeInt,
		UnityEngine_RandomRangeFloat,
		System_RandomRangeInt,
		System_RandomRangeDouble,
	}


	/// <summary>
	/// Unity Callback Start
	/// </summary>
	private void Start()
	{
		OnChangedDropdownList(0);
	}

	/// <summary>
	/// Unity Callback OnDestroy
	/// </summary>
	private void OnDestroy()
	{
		// Get Display Content
		var diplayPanel = GameObject.Find("Panel - Texture").transform;
		var contentImage = diplayPanel.Find("ContentImage").GetComponent<RawImage>();

		// Cleanup
		if (contentImage != null)
			DestroyImmediate(contentImage.texture);
	}

	/// <summary>
	/// Dropdown変更時(Inspectorにて設定)
	/// </summary>
	public void OnChangedDropdownList(int selectIndex)
	{
		switch ((DropdownList)selectIndex)
		{
		case DropdownList.UnityEngine_RandomRangeInt:	OnChanged_UnityEngine_RandomRangeInt();break;
		case DropdownList.UnityEngine_RandomRangeFloat:	OnChanged_UnityEngine_RandomRangeFloat();break;
		case DropdownList.System_RandomRangeInt:		OnChanged_System_RandomRangeInt(); break;
		case DropdownList.System_RandomRangeDouble:		OnChanged_System_RandomRangeDouble(); break;
		default: break;
		}
	}

	/// <summary>
	/// Dropdown UnityEngine.Random.Range(Int,Int)に変更時
	/// </summary>
	private void OnChanged_UnityEngine_RandomRangeInt()
	{
		float[] values = new float[SampleTexturePixel * SampleTexturePixel];
		for (int i = 0; i < SampleTexturePixel; ++i)
		{
			UnityEngine.Random.InitState(i);

			for (int k = 0; k < SampleTexturePixel; ++k)
				values[i * SampleTexturePixel + k] = UnityEngine.Random.Range(RangeMin, RangeMax);
		}

		DisplayGraph(values, RangeMin, RangeMax);
		DisplayTexture(values);
	}

	/// <summary>
	/// Dropdown UnityEngine.Random.Range(Int,Int)に変更時
	/// </summary>
	private void OnChanged_UnityEngine_RandomRangeFloat()
	{
		float[] values = new float[SampleTexturePixel * SampleTexturePixel];
		for (int i = 0; i < SampleTexturePixel; ++i)
		{
			UnityEngine.Random.InitState(i);

			for (int k = 0; k < SampleTexturePixel; ++k)
				values[i * SampleTexturePixel + k] = UnityEngine.Random.Range((float)RangeMin, (float)RangeMax);
		}

		DisplayGraph(values, RangeMin, RangeMax, 8);
		DisplayTexture(values);
	}

	/// <summary>
	/// Dropdown System.Random.Next(Int,Int)に変更時
	/// </summary>
	private void OnChanged_System_RandomRangeInt()
	{
		float[] values = new float[SampleTexturePixel * SampleTexturePixel];
		for (int i = 0; i < SampleTexturePixel; ++i)
		{
			var random = new System.Random(i);

			for (int k = 0; k < SampleTexturePixel; ++k)
				values[i * SampleTexturePixel + k] = random.Next(RangeMin, RangeMax);
		}

		DisplayGraph(values, RangeMin, RangeMax);
		DisplayTexture(values);
	}

	/// <summary>
	/// Dropdown System.Random.NextDouble()に変更時
	/// </summary>
	private void OnChanged_System_RandomRangeDouble()
	{
		int range = Mathf.Abs(RangeMin) + Mathf.Abs(RangeMax);

		float[] values = new float[SampleTexturePixel * SampleTexturePixel];
		for (int i = 0; i < SampleTexturePixel; ++i)
		{
			var random = new System.Random(i);

			for (int k = 0; k < SampleTexturePixel; ++k)
				values[i * SampleTexturePixel + k] = (float)(random.NextDouble() * range + RangeMin);
		}

		DisplayGraph(values, RangeMin, RangeMax, 8);
		DisplayTexture(values);
	}

	/// <summary>
	/// グラフ化
	/// </summary>
	private void DisplayGraph(float[] values, int min, int max, int split = 1)
	{
		// Non support params
		split = Mathf.Max(split, 1);
		if (min == 0 && max == 0) return;
		if (values.Length == 0) return;

		// Get Display Content
		var diplayPanel = GameObject.Find("Panel - Display").transform;
		var contentRoot        = diplayPanel.Find("Content").GetComponent<RectTransform>();
		var contentImageOrigin = diplayPanel.Find("ContentImage").gameObject;
		var layoutRoot         = diplayPanel.Find("LayoutGroup");
		var layoutTextOrigin   = diplayPanel.Find("LayoutGroupText").gameObject;

		// Cleanup
		for (int i = layoutRoot.childCount - 1; i >= 0; --i)
			DestroyImmediate(layoutRoot.GetChild(i).gameObject);
		for (int i = contentRoot.childCount - 1; i >= 0; --i)
			DestroyImmediate(contentRoot.GetChild(i).gameObject);

		// Display - Rank
		for (int i = min; i <= max; ++i)
		{
			var textGameObject = Instantiate(layoutTextOrigin);
			textGameObject.transform.SetParent(layoutRoot, false);
			textGameObject.GetComponent<Text>().text = i.ToString() + (split == 1 ? "" : ".0");
			textGameObject.SetActive(true);
		}

		// Aggregate values
		int contentNumber = Mathf.Abs(min) + Mathf.Abs(max) + 1;						// 乱数範囲
		int contentCountSize = Mathf.CeilToInt(contentNumber * split) - (split - 1);	// 集計個数(丸める)
		int[] contentCounts = new int[contentCountSize];
		for (int i = 0; i < values.Length; ++i)
		{
			float fNumber = values[i] - min;
			int iNumber = Mathf.FloorToInt(fNumber * split);
			contentCounts[iNumber]++;
		}

		// Display - Content
		float contentWidthRatio = contentRoot.rect.width / contentCountSize;
		float contentHeightRatio = contentRoot.rect.height / contentCounts.Max();
		for (int i = 0; i < contentCounts.Length; ++i)
		{
			float x = i * contentWidthRatio;
			float height = contentCounts[i] * contentHeightRatio;

			var contentGameObject = Instantiate(contentImageOrigin);
			contentGameObject.SetActive(true);

			var contentRectTransform = contentGameObject.GetComponent<RectTransform>();
			contentRectTransform.SetParent(contentRoot, false);
			contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, height);

			if (split != 1 && i % split == 0)
			{
				contentGameObject.GetComponent<Image>().color = Color.red;
			}
		}
	}

	/// <summary>
	/// テクスチャ化
	/// </summary>
	private void DisplayTexture(float[] values)
	{
		// Non support params
		if (values.Length == 0) return;

		// Get Display Content
		var diplayPanel = GameObject.Find("Panel - Texture").transform;
		var contentImage = diplayPanel.Find("ContentImage").GetComponent<RawImage>();

		// Cleanup
		if (contentImage != null)
			DestroyImmediate(contentImage.texture);

		// Create Texture
		var colors = new Color[values.Length];
		for (int i = 0; i < values.Length; ++i)
			colors[i] = Color.white * values[i];

		var texture = new Texture2D(SampleTexturePixel, SampleTexturePixel, TextureFormat.ARGB32, false);
		texture.SetPixels(0, 0, SampleTexturePixel, SampleTexturePixel, colors, 0);
		texture.Apply(false);

		// Set Texture
		contentImage.texture = texture;
	}
}

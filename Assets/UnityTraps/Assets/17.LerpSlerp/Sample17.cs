using UnityEngine;
using System;


/// <summary>
/// あるある１７「LerpとSlerpの使い方」の解説スクリプト
/// </summary>
public class Sample17 : MonoBehaviour
{
	/// <summary>
	/// From, Toのポイント描画用メッシュ
	/// </summary>
	[SerializeField]
	private Mesh pointMesh = null;

	/// <summary>
	/// From, Toのポイント描画用マテリアル
	/// </summary>
	[SerializeField]
	private Material pointMaterial = null;

	/// <summary>
	/// From, Toのポイント描画用マトリックス
	/// </summary>
	private Matrix4x4 pointMatrix = Matrix4x4.identity;

	/// <summary>
	/// 補間描画用メッシュ
	/// </summary>
	[SerializeField]
	private Mesh mesh = null;

	/// <summary>
	/// 補間描画用マテリアル(線形補間)
	/// </summary>
	[SerializeField]
	private Material lerpMaterial = null;

	/// <summary>
	/// 補間描画用マテリアル(球面線形補間)
	/// </summary>
	[SerializeField]
	private Material slerpMaterial = null;

	/// <summary>
	/// ポイント開始点
	/// </summary>
	[SerializeField]
	private Vector3 from;

	/// <summary>
	/// ポイント終了点
	/// </summary>
	[SerializeField]
	private Vector3 to;


	/// <summary>
	/// Unity Callback Update
	/// </summary>
	private void Update()
	{
		from = Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(0.25f * Time.time) * 360.0f) * Vector3.right * 10 * Mathf.Cos(0.52f * Time.time);
		to   = Quaternion.Euler(0.0f, 0.0f, Mathf.Cos(0.52f * Time.time) * 360.0f) * Vector3.right * 10 * Mathf.Sin(0.25f * Time.time);

		pointMatrix *= Matrix4x4.Rotate( Quaternion.Euler(
			3 * Mathf.Sin(1 * Time.time) * Time.deltaTime * 40,
			2 * Mathf.Cos(2 * Time.time) * Time.deltaTime * 40,
			1 * Mathf.Sin(3 * Time.time) * Time.deltaTime * 40
		) );
	}

	/// <summary>
	/// Unity Callback OnDrawGizmos
	/// </summary>
	private void OnDrawGizmos()
	{
		DrawInterpolation(slerpMaterial, Slerp);
		//DrawInterpolation(slerpMaterial, Vector3.Slerp);
		DrawInterpolation(lerpMaterial, Vector3.Lerp);

		Graphics.DrawMesh(pointMesh, from, pointMatrix.rotation, pointMaterial, 0, Camera.main);
		Graphics.DrawMesh(pointMesh, to, pointMatrix.rotation, pointMaterial, 0, Camera.main);
	}

	/// <summary>
	/// 補間中のポイントを描画
	/// </summary>
	private void DrawInterpolation(Material material, Func<Vector3, Vector3, float, Vector3> interpolateFunc)
	{
		int Max = 21;
		Matrix4x4 matrix = Matrix4x4.identity;

		for (int i = 1; i < Max; i++)
		{
			float ratio = i / (float)Max;
			var pos = interpolateFunc(from, to, ratio);

			matrix.SetTRS(pos, Quaternion.identity, Vector3.one * 0.5f);
			Graphics.DrawMesh(mesh, matrix, material, 0, Camera.main);
		}
	}

	/// <summary>
	/// 試しに実装したSlerp
	/// </summary>
	private Vector3 Slerp(Vector3 from, Vector3 to, float t)
	{
		// 正規化されたベクトルを生成
		var start = from.normalized;
		var end = to.normalized;

		// 2ベクトル間の角度を算出(radian角)
		float dot = Vector3.Dot(start, end);
		if (Mathf.Abs(dot) > 0.9995f)
			dot = 0.5f;
		float totalRadian = Mathf.Acos(dot);

		// 球面に補間されるようにSine(radian = 0 ～ PI)を使う
		float totalAngle = Mathf.Sin(totalRadian);
		float startAngle = Mathf.Sin(totalRadian * (1.0f - t));
		float endAngle   = Mathf.Sin(totalRadian * t);
		var vec = (start * startAngle + end * endAngle) / totalAngle;

		// 距離は線形補間
		vec = vec * (from.magnitude * (1.0f - t) + to.magnitude * t);
		return vec;
	}
}

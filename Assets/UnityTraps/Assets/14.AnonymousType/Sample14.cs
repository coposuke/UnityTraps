using System.Linq;
using UnityEngine;


/// <summary>
/// あるある１４「匿名型で便利に情報整理」の解説スクリプト
/// </summary>
public class Sample14 : MonoBehaviour
{
	/// <summary>
	/// Unity Event Start
	/// </summary>
	private void Start()
	{
		CreateWall();
	}

	/// <summary>
	/// Planeの周りに壁を生成
	/// </summary>
	private void CreateWall()
	{
		var plane = GameObject.Find("/3D/Plane");
		var planeTransform = plane.transform;

		var wallScale = planeTransform.localScale * 10.0f;		// PlaneはCubeの10倍の大きさ
		var wallDistance = planeTransform.localScale * 5.0f;	// Planeと壁の距離は半分の5倍の大きさ
		wallDistance.x -= 0.5f; // Cubeの半径分中心に近づける
		wallDistance.z -= 0.5f; // Cubeの半径分中心に近づける

		var walls = new[]
		{
			new { pos = new Vector3(0.0f, 0.5f, -wallDistance.z), scale = new Vector3(wallScale.x, 1.0f, 1.0f) }, // 手前
			new { pos = new Vector3(0.0f, 0.5f,  wallDistance.z), scale = new Vector3(wallScale.x, 1.0f, 1.0f) }, // 奥
			new { pos = new Vector3(-wallDistance.x, 0.5f, 0.0f), scale = new Vector3(1.0f, 1.0f, wallScale.z) }, // 左
			new { pos = new Vector3( wallDistance.x, 0.5f, 0.0f), scale = new Vector3(1.0f, 1.0f, wallScale.z) }, // 右
		};

		foreach (var wall in walls.Select((info, idx) => new { info, idx }))
		//for(int idx = 0; idx < walls.Length; ++idx) // 通常は無理せずこちらを。上は必要に応じて。
		{
			var wallObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var wallTransform = wallObject.transform;
			var wallCollider = wallObject.AddComponent<BoxCollider>();

			wallCollider.size = Vector3.one;
			wallCollider.center = Vector3.zero;
			wallTransform.SetParent(planeTransform);
			wallTransform.localPosition = wall.info.pos;
			wallTransform.localScale = wall.info.scale;
			wallObject.name = "Wall" + wall.idx;
		}
	}
}

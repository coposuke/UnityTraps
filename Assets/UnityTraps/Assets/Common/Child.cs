#pragma warning disable 0414 // メンバ変数の未使用警告

using UnityEngine;


/// <summary>
/// あるある解説用のコンポーネント
/// </summary>
public class Child : MonoBehaviour
{
	/// <summary>
	/// Int型変数
	/// </summary>
	private int intValue = 0;

	/// <summary>
	/// SendMessage Event Method(検証の為Publicですが、本来はPrivateであるべき)
	/// </summary>
	public void SetValue(int value)
	{
		this.intValue = value;
	}
}

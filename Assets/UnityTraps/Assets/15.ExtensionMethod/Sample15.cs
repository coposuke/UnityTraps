using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// あるある１５「拡張メソッドは認知されやすい」の解説スクリプト
/// </summary>
public class Sample15 : MonoBehaviour
{
	/// <summary>
	/// Unity Event Start
	/// </summary>
	void Start ()
	{
		var paramDic = new Dictionary<string, string>
		{
			{"hp", "123"},
			{"mp", "45"},
			{"rate", "6.78"},
		};

		var self = this.GetComponent<Sample15>();
		Debug.Log("1. " + self);

		self = this.GetOrAddComponent<Sample15>();
		Debug.Log("2. " + self);

		var child = this.GetOrAddComponent<Child>();
		Debug.Log("3. " + child);

		Debug.Log("4. " + paramDic.TryGetValueOrDefault("hp"));

		var mpValue = paramDic.TryGetValueOrDefault("mp").TryParse<int>(int.TryParse);
		Debug.Log("5. " + mpValue);

		var rateValue = paramDic.TryGetValueOrDefault("rate").TryParse<float>(float.TryParse);
		Debug.Log("6. " + rateValue);
	}
}


/// <summary>
/// 拡張メソッド群
/// </summary>
/// <remarks>namespaceで囲った場合はusingしないと利用できません</remarks>
public static class ExtensionMethods
{
	/// <summary>
	/// UnityEngine.GameObjectからGetComponent。なければAddComponent。
	/// </summary>
	public static TComponent GetOrAddComponent<TComponent>(this GameObject self) where TComponent : Component
	{
		var result = self.GetComponent<TComponent>();
		if (result == null)
			result = self.AddComponent<TComponent>();
		return result;
	}

	/// <summary>
	/// UnityEngine.ComponentからGetComponent。なければAddComponent。
	/// </summary>
	static public TComponent GetOrAddComponent<TComponent>(this Component self) where TComponent : Component
	{
		var result = self.GetComponent<TComponent>();
		if (result == null)
			result = self.gameObject.AddComponent<TComponent>();
		return result;
	}

	/// <summary>
	/// int, float, doubleのようなプリミティブ型のTryParse
	/// </summary>
	public delegate bool TryParser<TType>(string from, out TType to);

	/// <summary>
	/// Dictionaryから値の取得
	/// </summary>
	static public Nullable<TValue> TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key) where TValue : struct
	{
		TValue obj = default(TValue);
		self.TryGetValue(key, out obj);
		return obj;
	}

	/// <summary>
	/// Dictionaryから値の取得
	/// </summary>
	static public TValue TryGetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key) where TValue : class
	{
		TValue obj = default(TValue);
		self.TryGetValue(key, out obj);
		return obj;
	}

	/// <summary>
	/// stringからパースされた値の取得
	/// </summary>
	static public TType TryParse<TType>(this string self, TryParser<TType> parser)
	{
		TType value = default(TType);
		parser(self, out value);
		return value;
	}

	/// <summary>
	/// 同名メソッドが存在する為利用できないメソッド
	/// </summary>
	static public TComponent GetComponent<TComponent>(this Component self)
	{
		return default(TComponent);
	}
}

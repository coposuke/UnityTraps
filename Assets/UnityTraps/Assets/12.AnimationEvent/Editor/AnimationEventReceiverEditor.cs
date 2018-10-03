using UnityEngine;
using UnityEditor;


/// <summary>
/// Inspectorで綺麗に表示するためのエディタ
/// </summary>
[CustomEditor(typeof(AnimationEventReceiver))]
public class AnimationEventReceiverEditor : Editor
{
	private Vector2 scrollPosition;

	/// <summary>
	/// Unity Event OnInspectorGUI
	/// </summary>
	public override void OnInspectorGUI()
	{
		var component = (AnimationEventReceiver)target;
		var logs = component.logs;

		using (new EditorGUILayout.HorizontalScope())
		{
			if (GUILayout.Button("Clear"))
				logs.Clear();

			EditorGUILayout.LabelField("件数：" + logs.Count);
		}

		if (0 < logs.Count)
		{
			var scrollHeight = 20 * EditorStyles.label.lineHeight;

			using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, false, true, GUILayout.Height(scrollHeight)))
			{
				scrollPosition = scrollView.scrollPosition;

				foreach (var log in logs)
				{
					EditorGUILayout.LabelField(log);
				}
			}
		}
	}
}

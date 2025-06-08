using System;
using UnityEngine;

public class IdentifyConsoleCommand : ConsoleCommand {
	public override void Run() {
		if (IdentifyConsoleCommand.labelStyle == null) {
			IdentifyConsoleCommand.labelStyle = new GUIStyle();
			IdentifyConsoleCommand.labelStyle.alignment = TextAnchor.MiddleCenter;
			IdentifyConsoleCommand.labelStyle.fontSize = 18;
		}
		bool flag = base.OnOffArgument();
		IdentifyConsoleCommand.isRunning = flag;
	}

	public static Entity NearbyEntity(Vector2 position, float radius = 0.25f) {
		Collider2D[] array = Physics2D.OverlapCircleAll(position, radius, 1 << Ecosystem.entityLayer);
		foreach (Collider2D collider2D in array) {
			Entity component = collider2D.GetComponent<Entity>();
			return component;
		}
		return null;
	}

	public override bool RequiresAdmin() {
		return false;
	}

	public static bool isRunning = false;
	public static GUIStyle labelStyle;
}
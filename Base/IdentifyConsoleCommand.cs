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

	public override bool RequiresAdmin() {
		return false;
	}

	public static bool isRunning = false;
	public static GUIStyle labelStyle;
}
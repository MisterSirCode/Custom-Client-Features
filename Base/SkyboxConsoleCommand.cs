using System;
using System.Linq;
using UnityEngine;

public class SkyboxConsoleCommand : ConsoleCommand {
	public override void Run() {
		if (base.arguments.Count() == 4) {
			SkyboxConsoleCommand.customColor = new Color((float)Double.Parse(base.arguments[0]), (float)Double.Parse(base.arguments[1]), (float)Double.Parse(base.arguments[2]), (float)Double.Parse(base.arguments[3]));
			SkyboxConsoleCommand.activeWithColor = true;
		} else {
			bool toggle = base.OnOffArgument();
			if (toggle == false) SkyboxConsoleCommand.activeWithColor = false;
			foreach (object obj in GameObject.Find("/Sky Camera").transform) {
				Transform child = (Transform)obj;
				string name = child.gameObject.name;
				if (name != "Sky" && name != "Stars") {
					child.gameObject.SetActive(toggle);
				}
				if (name == "Stars" && ReplaceableSingleton<Zone>.main.biome == "space") {
					child.gameObject.SetActive(toggle);
				}
				if (name == "Terrain") {
					child.gameObject.SetActiveRecursively(toggle);
				}
			}
		}
	}

	public static bool activeWithColor = false;
	public static Color customColor;

	public override bool RequiresAdmin() {
		return false;
	}
}
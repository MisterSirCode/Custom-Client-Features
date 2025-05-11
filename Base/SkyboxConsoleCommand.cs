using System;
using UnityEngine;

public class SkyboxConsoleCommand : ConsoleCommand {
	public override void Run() {
		bool toggle = base.OnOffArgument();
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

	// Token: 0x06004A37 RID: 18999
	public override bool RequiresAdmin() {
		return false;
	}
}
using System;
using UnityEngine;

public class SkyboxConsoleCommand : ConsoleCommand {
	public override void Run() {
		bool flag = base.OnOffArgument();
		GameObject sky = GameObject.Find("/Sky Camera");
		sky.SetActive(flag);
	}

	// Token: 0x06004A37 RID: 18999
	public override bool RequiresAdmin() {
		return false;
	}
}
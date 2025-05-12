using System;
using UnityEngine;

public class BldoverConsoleCommand : ConsoleCommand {
	public override void Run() {
		BldoverConsoleCommand.buildOverride = !BldoverConsoleCommand.buildOverride;
		PlayerPrefs.SetInt("buildOverride", BldoverConsoleCommand.buildOverride == true ? 1 : 0);
		PlayerPrefs.Save();
	}

	// Token: 0x06004A37 RID: 18999
	public override bool RequiresAdmin() {
		return false;
	}

    public static bool buildOverride;
}
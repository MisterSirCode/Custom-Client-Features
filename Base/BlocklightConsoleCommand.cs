using System;
using UnityEngine;

public class BlocklightConsoleCommand : ConsoleCommand {
	public override void Run() {
		if (base.arguments.Count() == 4) {
			BlocklightConsoleCommand.globalColorOverride = new Color((float)Double.Parse(base.arguments[0]), (float)Double.Parse(base.arguments[1]), (float)Double.Parse(base.arguments[2]), (float)Double.Parse(base.arguments[3]));
		} else {
			bool flag = base.OnOffArgument();
            if (flag) {
                BlocklightConsoleCommand.globalColorOverride = new Color(1f, 1f, 1f, 1f);
            } if (!flag) {
			    BlocklightConsoleCommand.globalColorOverride = new Color(0f, 0f, 0f, 1f);
            }
        }
	}

	// Token: 0x06004A37 RID: 18999
	public override bool RequiresAdmin() {
		return false;
	}

    public static Color globalColorOverride;
}
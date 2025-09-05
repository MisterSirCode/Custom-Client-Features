using System;
using UnityEngine;

public class MoodyConsoleCommand : ConsoleCommand {
	public override void Run() {
		MeshRenderer meshie = GameObject.Find("/Lighting Camera").GetComponent<LightingRenderer>().lightingOverlayTransform.GetComponent<MeshRenderer>();
		if (base.arguments.Count() == 4) {
			Player player = ReplaceableSingleton<Player>.main;
			if (player == null) {
				return;
			}
			double alpha = Double.Parse(base.arguments[3]);
			if (alpha > 0.45 && !player.admin) {
				Notification.Create("You can set alpha to 0.45 maximum.", 1);
				return;
			}
			meshie.material.color = new Color((float)Double.Parse(base.arguments[0]), (float)Double.Parse(base.arguments[1]), (float)Double.Parse(base.arguments[2]), (float)alpha);
		} else {
			bool flag = base.OnOffArgument();
            if (flag) {
			meshie.material.color = new Color(0f, 0f, 0f, 0.1f);
            } if (!flag) {
                meshie.material.color = new Color(0f, 0f, 0f, 0.4f);
            }
        }
	}

	public override bool RequiresAdmin() {
		return false;
	}
}
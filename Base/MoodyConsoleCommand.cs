using System;
using UnityEngine;

public class MoodyConsoleCommand : ConsoleCommand {
	public override void Run() {
		MeshRenderer meshie = GameObject.Find("/Lighting Camera").GetComponent<LightingRenderer>().lightingOverlayTransform.GetComponent<MeshRenderer>();
		if (base.arguments.Count() == 4) {
			meshie.material.color = new Color((float)Double.Parse(base.arguments[0]), (float)Double.Parse(base.arguments[1]), (float)Double.Parse(base.arguments[2]), (float)Double.Parse(base.arguments[3]));
		} else {
			bool flag = base.OnOffArgument();
            if (flag) {
			meshie.material.color = new Color(0f, 0f, 0f, 0.1f);
            } if (!flag) {
                meshie.material.color = new Color(0f, 0f, 0f, 0.4f);
            }
        }
	}

	// Token: 0x06004A37 RID: 18999
	public override bool RequiresAdmin() {
		return false;
	}
}
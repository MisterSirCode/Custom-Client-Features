using System;
using System.Linq;
using UnityEngine;

public class SupergayConsoleCommand : ConsoleCommand {
	public override void Run() {
		bool toggle = base.OnOffArgument();
        SupergayConsoleCommand.active = toggle;
	}

	public static bool active = false;

	public override bool RequiresAdmin() {
		return false;
	}
}
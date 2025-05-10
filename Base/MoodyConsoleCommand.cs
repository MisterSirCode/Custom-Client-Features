using System;

public class MoodyConsoleCommand : ConsoleCommand {
	public override void Run() {
		Messenger.Broadcast<bool>("moodyCommandUsed", false);
	}

	public override bool RequiresAdmin() {
		return false;
	}
}

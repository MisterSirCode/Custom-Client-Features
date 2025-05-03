	public void OnGUI() {
		if (ExternalConsole.GetInstance() != null && ExternalConsole.GetInstance().enabled) {
			ExternalConsole.GetInstance().Draw();
		}
	}
	public void SessionChanged() {
		string @string = PlayerPrefs.GetString("grap_username");
		Messenger.Broadcast<string>("sessionChanged", (@string == null || @string.Length <= 0) ? null : @string);
	}
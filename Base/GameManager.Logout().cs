	public void Logout() {
		PlayerPrefs.DeleteKey("grap_username");
		PlayerPrefs.DeleteKey("grap_authToken");
		if (GameManager.IsGame()) {
			this.LoadMainMenu();
		}
		this.SessionChanged();
	}
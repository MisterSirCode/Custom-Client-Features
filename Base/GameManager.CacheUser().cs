	public void CacheUser(string username, string authToken) {
		PlayerPrefs.SetString("grap_username", username);
		PlayerPrefs.SetString("grap_authToken", authToken);
	}
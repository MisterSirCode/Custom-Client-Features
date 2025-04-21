	public Hashtable CurrentUser()
	{
		string @string = PlayerPrefs.GetString("grap_username");
		string string2 = PlayerPrefs.GetString("grap_authToken");
		if (@string != null && @string.Length > 0 && string2 != null && string2.Length > 0) {
			return new Hashtable {
				{
					"name",
					@string
				},
				{
					"token",
					string2
				}
			};
		}
		return null;
	}
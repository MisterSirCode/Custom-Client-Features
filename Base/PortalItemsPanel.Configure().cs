public void Configure(List<object> optionConfigs) {
	foreach (object obj in optionConfigs) {
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
		PortalSearchOption component = this.optionPrefab;
		if (this.options.Count > 0) {
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.optionPrefab.gameObject);
			gameObject.transform.SetParent(this.optionPrefab.transform.parent, false);
			component = gameObject.GetComponent<PortalSearchOption>();
		}
		this.options.Add(component);
		component.label.text = dictionary.GetString("name");
		component.parameters = dictionary.GetDictionary("params");
	}

	if (transform.Find("Parchment(Clone)/Scroll Rect").GetComponent<ScrollSpeedSetting>() == null) {
		transform.Find("Parchment(Clone)/Scroll Rect").gameObject.AddComponent<ScrollSpeedSetting>();
	}
}

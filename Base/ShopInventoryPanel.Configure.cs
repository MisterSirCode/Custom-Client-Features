public void Configure(Dictionary<string, object> config, Dictionary<string, object> section) {
	this.config = config;
	this.LoadItems(section.GetList("items"), this.itemsPanel);
	this.LoadItems(section.GetList("top_items"), this.topItemsPanel);
	if (base.transform.Find("Parchment(Clone)/Scroll Rect").GetComponent<ScrollSpeedSetting>() == null) {
		base.transform.Find("Parchment(Clone)/Scroll Rect").gameObject.AddComponent<ScrollSpeedSetting>();
	}
}

public override void ConfigureInternal(Dictionary<string, object> cfg) {
	base.ConfigureInternal(cfg);
	object obj = cfg.Get("list");
	object[] array = ((!(obj is object[])) ? ((List<object>)obj).ToArray() : ((object[])obj));
	this.listItemPrefab.gameObject.SetActive(array.Length != 0);
	for (int i = 0; i < array.Length; i++) {
		Dictionary<string, object> dictionary = (Dictionary<string, object>)array[i];
		Transform transform = this.listItemPrefab;
		if (i > 0) {
			transform = global::UnityEngine.Object.Instantiate<RectTransform>(this.listItemPrefab);
			transform.SetParent(this.panel, false);
		}
		DialogListItem component = transform.GetComponent<DialogListItem>();
		component.text.text = dictionary.GetString("text");
		component.text.supportRichText = dictionary.GetBool("supportRichText", component.text.supportRichText);
		component.text.color = this.dialog.colors[this.textColorType];
		if (dictionary.ContainsKey("item")) {
			if (GameManager.IsGame()) {
				Item item = Item.Get(Convert.ToInt32(dictionary["item"]));
				if (item != null) {
					string text = item.InventorySpriteName();
					tk2dSpriteDefinition tk2dSpriteDefinition = Singleton<AtlasManager>.main.Sprite(text, false);
					if (tk2dSpriteDefinition != null) {
						component.rawImage.texture = Singleton<AtlasManager>.main.inventoryTexture;
						component.rawImage.SetTk2dSprite(tk2dSpriteDefinition);
					}
				}
			}
			component.image.gameObject.SetActive(false);
		} else if (dictionary.ContainsKey("image")) {
			component.image.sprite = GameGui.GetSprite(dictionary.GetString("image"));
			component.rawImage.gameObject.SetActive(false);
		} else {
			component.image.gameObject.SetActive(false);
			component.rawImage.gameObject.SetActive(false);
		}
	}
}

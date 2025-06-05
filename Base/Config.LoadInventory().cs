private void LoadInventory(List<object> cfg) {
	this._itemsByInventoryCategory = new Dictionary<string, List<Item>>();
	this._inventoryCategories = new List<InventoryCategory>();
	List<Item> list = this.AllItems();
	bool useDebug = false;
	foreach (object obj3 in cfg) {
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj3;
		InventoryCategory inventoryCategory = new InventoryCategory {
			name = (string)dictionary["name"],
			icon = (string)dictionary["icon"]
		};
		if (dictionary.Get("crafting") != null) {
			inventoryCategory.crafting = (bool)dictionary.Get("crafting");
		}
		this._inventoryCategories.Add(inventoryCategory);
		List<Item> list2 = new List<Item>();
		string text = (string)dictionary["name"];
		foreach (object obj2 in ((List<object>)dictionary["items"])) {
			Item item = this.itemsByName.Get((string)obj2);
			if (item != null) {
				list2.Add(item);
				if (list.Contains(item)) {
					list.Remove(item);
				}
				item.inventoryCategory = text;
			} else {
				global::UnityEngine.Debug.Log("Can't find item " + ((obj2 != null) ? obj2.ToString() : null));
			}
		}
		this._itemsByInventoryCategory[text] = list2;
	}
	if (useDebug) {
		this._inventoryCategories.Add(new InventoryCategory {
			name = "Other",
			crafting = true
		});
		this._itemsByInventoryCategory["Other"] = list;
	}
}
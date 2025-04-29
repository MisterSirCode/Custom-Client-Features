	private void LoadInventory(List<object> cfg)
	{
		try
		{
			this._itemsByInventoryCategory = new Dictionary<string, List<Item>>();
			this._inventoryCategories = new List<InventoryCategory>();
			bool debug = true;
			if (debug)
			{
				this._inventoryCategories.Add(new InventoryCategory
				{
					name = "Other",
					crafting = false
				});
			}
			foreach (object obj3 in cfg)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj3;
				InventoryCategory inventoryCategory = new InventoryCategory
				{
					name = (string)dictionary["name"],
					icon = (string)dictionary["icon"]
				};
				if (dictionary.Get("crafting") != null)
				{
					inventoryCategory.crafting = (bool)dictionary.Get("crafting");
				}
				this._inventoryCategories.Add(inventoryCategory);
				List<Item> list = new List<Item>();
				string text = (string)dictionary["name"];
				List<Item> list2 = this.AllItems();
				foreach (object obj2 in ((List<object>)dictionary["items"]))
				{
					Item item = this.itemsByName.Get((string)obj2);
					if (item != null)
					{
						if (debug && list2.Contains(item))
						{
							list2.Remove(item);
						}
						list.Add(item);
						item.inventoryCategory = text;
					}
					else
					{
						Debug.Log("Can't find item " + ((obj2 != null) ? obj2.ToString() : null));
					}
				}
				if (debug && inventoryCategory.name == "Other")
				{
					this._itemsByInventoryCategory[text] = list2;
				}
				this._itemsByInventoryCategory[text] = list;
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Loader Error - " + ex.ToString());
		}
	}
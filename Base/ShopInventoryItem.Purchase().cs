public void Purchase() {
	if (this.CanPurchase()) {
		string text = Locale.Text("purchase.confirm", null, null, null);
		string text2 = Locale.Text("purchase.confirm.detail", this.item.GetString("name"), this.cost.ToString() + string.Empty, null);
		List<object> list = this.item.GetList("inventory");
		if (list == null) {
			list = new List<object>();
		}
		object[] items = new object[list.Count];
		for (int i = 0; i < list.Count; i++) {
			List<object> list2 = (List<object>)list[i];
			Item item = Item.Get((string)list2[0]);
			object amount = list2[1];
			Dictionary<string, object> dialogListItem = new Dictionary<string, object>() {
				{ "item", item.code },
				{ "text", ((amount != null) ? amount.ToString() : null) + " x " + item.title },
				{ "supportRichText", true },
			};
			items[i] = dialogListItem;
		}
		ConfigurableDialog.ActionHandler actionHandler = delegate(Dictionary<string, object> values) {
			Command.Send(Command.Identity.Transaction, new object[] { this.item.GetString("key") });
			ReplaceableSingleton<Player>.main.crowns -= this.cost;
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object> {
			{ "title", text },
			{
				"sections",
				new List<object> {
					new Dictionary<string, object> {
						{ "text", text2 },
						{ "list", items }
					}
				}
			},
			{
				"actions",
				new List<object> {
					new Dictionary<string, object> {
						{ "title", "Cancel" },
						{ "close", true }
					},
					new Dictionary<string, object> {
						{ "title", "Okay" },
						{ "handler", actionHandler }
					}
				}
			},
			{ "id", "shop_drawer" }
		};
		base.transform.OpenConfigurableDrawerInWindow(dictionary, null, null);
	} else {
		base.transform.OpenConfigurableDrawerInWindow(Locale.Text("error.oops", null, null, null), Locale.Text("purchase.not_enough_crowns", null, null, null), null, null);
	}
	GameGui.Sfx(GameGui.Sound.Click);
}

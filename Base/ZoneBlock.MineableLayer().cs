	public Item.Layer MineableLayer()
	{
		if (this.frontItem != null && this.frontItem.code > 0)
		{
			return Item.Layer.Front;
		}
		if (this.backItem != null && this.backItem.code > 0)
		{
			return Item.Layer.Back;
		}
		return Item.Layer.None;
	}
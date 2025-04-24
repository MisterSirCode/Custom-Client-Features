public Item.Layer ScrubbableLayer()
{
	if (this.baseItem != null && this.baseItem.code > 0) {
		return Item.Layer.Base;
	}
	return Item.Layer.None;
}
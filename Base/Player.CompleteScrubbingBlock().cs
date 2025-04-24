public bool CompleteScrubbingBlock(ZoneBlock block)
{
	Item.Layer layer = block.ScrubbableLayer();
	Item item = block.ItemByLayer(layer).RootItem();
	Item primaryItem = this.primaryItem;
	int num = block.ModByLayer(layer);
	int num2 = ((item.parentItem == null) ? item.code : item.parentItem.code);
	Command.Send(Command.Identity.BlockMine, new object[] {
		block.position.x,
		block.position.y,
		(int)layer,
		num2,
		0
	});
	block.SetLayer(layer, 0, 0);
    Item item2 = ((item.mod != Item.Mod.Decay || num <= 0 || item.decayInventoryItem == null) ? item.inventoryItem : item.decayInventoryItem);
    if (item2 != null) {
        int num4 = ((item.mod != Item.Mod.Stack || num <= 0) ? 1 : num);
        this.inventory.Add(item2.code, num4);
    }
	int itemsMined = this.itemsMined;
	this.itemsMined = itemsMined + 1;
	block.Place(true);
	Messenger.Broadcast<Item>("playerMinedItem", item);
	Messenger.Broadcast<ZoneBlock>("playerMinedBlock", block);
	return true;
}

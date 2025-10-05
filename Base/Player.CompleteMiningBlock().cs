public bool CompleteMiningBlock(ZoneBlock block) {
    Item.Layer layer = block.MineableLayer();
    Item item = block.ItemByLayer(layer).RootItem();
    Item primaryItem = this.primaryItem;
    int layerMod = block.ModByLayer(layer);
    int serverItemCode = (item.parentItem == null) ? item.code : item.parentItem.code;
    Command.Send(Command.Identity.BlockMine, new object[] {
        block.position.x,
        block.position.y,
        (int)layer,
        serverItemCode,
        0
    });
    bool digging = primaryItem.action == Item.Action.Dig && item.diggable;
    int layerReplacementItemCode = (!digging) ? 0 : Item.GetCode("ground/earth-dug");
    block.SetLayer(layer, layerReplacementItemCode, 0);
    if (!digging) {
        Item inventoryItem = (item.mod != Item.Mod.Decay || layerMod <= 0 || item.decayInventoryItem == null) ? item.inventoryItem : item.decayInventoryItem;
        if (inventoryItem != null) {
            int qty = 1;
            object pile = item.Usability(Item.Use.Pile);
            if (item.mod == Item.Mod.Stack && layerMod > 0) {
                qty = layerMod;
            }
            if (pile != null) {
                qty = (int)pile * layerMod;
            }
            this.inventory.Add(inventoryItem.code, qty);
        }
    }
    int itemsMined = this.itemsMined;
    this.itemsMined = itemsMined + 1;
    block.Place(true);
    Messenger.Broadcast<Item>("playerMinedItem", item);
    Messenger.Broadcast<ZoneBlock>("playerMinedBlock", block);
    return true;
}

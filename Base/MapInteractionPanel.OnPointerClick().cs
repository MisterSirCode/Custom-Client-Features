public void OnPointerClick(PointerEventData eventData) {
    Vector3 v = this.WorldClickPosition(eventData.pressPosition);
    MetaBlock metaBlock = ReplaceableSingleton<Zone>.main.meta.ClosestBlockInRange(v, 25f, Item.Use.Teleportable, "global");
    if (metaBlock != null) {
        int[] targetPosition = new int[]{ metaBlock.x, metaBlock.y };
        if (this.teleporterOriginBlock != null) {
            new All(this.teleporterOriginBlock.frontItem).SendCommand(this.teleporterOriginBlock, targetPosition);
        } else {
            Item consumableTeleporter = Items.Teleport.BestItem();
            if (consumableTeleporter != null && consumableTeleporter.code != 0) {
                Command.Send(Command.Identity.InventoryUse, new object[]{ 0, consumableTeleporter.name, 1, targetPosition });
                if (consumableTeleporter.consumable) {
                    ReplaceableSingleton<Player>.main.inventory.Remove(consumableTeleporter.code, 1);
                }
            }
        }
        Messenger.Broadcast<bool>("mapShow", false);
    }
}
public bool CanPlaceBlock(Vector2 blockPosition, out string failureReason) {
    failureReason = null;
    if ((new Vector2(blockPosition.x, -blockPosition.y) - (Vector2)this.avatarTransform.position + this.avatarBoxCollider.offset).magnitude > this.MaxPlacingDistance()) {
        failureReason = Locale.Text("block.too_far", null, null, null);
        return false;
    }
    if (this.inventory.Quantity(this.primaryItem) < 1) {
        failureReason = Locale.Text("inventory.not_enough", null, null, null);
        return false;
    }
    ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)blockPosition.x, (int)blockPosition.y, false);
    if (zoneBlock != null) {
        if ((this.primaryItem.fieldable == Item.Fieldable.Always || this.primaryItem.fieldable == Item.Fieldable.Placed) && zoneBlock.IsProtectedAgainstPlayer(Player.Action.Place, out failureReason)) {
            return false;
        }
        if (this.primaryItem.layer != Item.Layer.Base) {
            Item item = zoneBlock.ItemByLayer(this.primaryItem.layer);
            if (item != null && item.code > 0 && !item.placeover) {
                failureReason = Locale.Text("block.occupied", null, null, null);
                return false;
            }
        }
        if (this.primaryItem.mounted && (zoneBlock.baseItem == null || zoneBlock.baseItem.code < 2) && (zoneBlock.backItem == null || zoneBlock.backItem.code == 0)) {
            return false;
        }
    }
    return true;
}
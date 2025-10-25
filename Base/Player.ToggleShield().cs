public void ToggleShield(bool on) {
    if (on) {
        if (this.activeShield == null) {
            this.activeShield = this.ActivateMomentaryAccessory(Item.Action.Shield);
        }
    } else {
        this.activeShield = null;
        this.avatar.shield.color = GraphicsHelper.HexColor("b744ff");
    }
    this.avatar.shieldItem = this.activeShield;
    Command.Identity identity = Command.Identity.InventoryUse;
    object[] array = new object[4];
    array[0] = 1;
    array[1] = (this.activeShield == null) ? 0 : this.activeShield.code;
    array[2] = (this.activeShield != null) ? 1 : 0;
    Command.Send(identity, array);
    Messenger.Broadcast<bool>("playerShieldActivated", on);
}

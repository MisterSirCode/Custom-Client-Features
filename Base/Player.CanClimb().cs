private bool CanClimb() {
    ZoneBlock accessibleBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)position.x, (int)position.y);
    return accessibleBlock != null && (this.CanClimb(accessibleBlock) || this.CanClimb(accessibleBlock.Top()));
}

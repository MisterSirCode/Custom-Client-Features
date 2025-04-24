public bool ScrubBlock(Vector2 blockPosition, float miningDuration)
{
	if (!this.avatar.alive) {
		return false;
	}
	ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
	if (zoneBlock != null) {
		Item item = zoneBlock.ScrubbableItem();
		if (item != null) {
			float num = Mathf.Clamp(miningDuration / this.MiningDuration(item), 0f, 1f);
			Messenger.Broadcast<ZoneBlock, Item, float>("miningDamage", zoneBlock, item, num);
			if (num >= 1f) {
				return this.CompleteScrubbingBlock(zoneBlock);
			}
		} else {
			this.avatar.interactionItem = null;
		}
	}
	this.AnimateTool();
	return false;
}
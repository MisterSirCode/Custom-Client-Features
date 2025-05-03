public bool CanScrubBlock(Vector2 blockPosition, bool allowIneffectuality, out string failureReason, bool debug = false) {
	failureReason = null;
	if (this.awesomeMode) {
		return true;
	}
	if (this.primaryItem == null) {
		return false;
	}
	if ((new Vector2(blockPosition.x, -blockPosition.y) - ((Vector2)this.avatarTransform.position + this.avatarBoxCollider.offset)).magnitude > this.MaxMiningDistance()) {
		failureReason = Locale.Text("block.too_far", null, null, null);
		return false;
	}
	if (allowIneffectuality) {
		return true;
	}
	ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
	if (zoneBlock == null || zoneBlock.sceneBlock == null) {
		if (debug) {
			failureReason = Locale.Text("block.no_accessible_block", null, null, null);
		}
		return false;
	}
	if (!this.CanReachBlock(blockPosition)) {
		if (debug) {
			failureReason = Locale.Text("block.cannot_reach", null, null, null);
		}
		return false;
	}
	Item item = zoneBlock.ScrubbableItem();
	if (item == null) {
		if (debug) {
			failureReason = Locale.Text("block.no_mineable_item", null, null, null);
		}
		return false;
	}
	if (item.invulnerable && !this.activeAdmin) {
		failureReason = Locale.Text("block.invulnerable", null, null, null);
		return false;
	}
	if (zoneBlock.IsProtectedAgainstPlayer(Player.Action.Mine, out failureReason)) {
		return false;
	}
	if (!this.IsSkilledToMineItem(item)) {
		string text = string.Concat(new object[] {
			Locale.Text("block.not_skilled", null, null, null),
			" (",
			item.miningSkill.Capitalize(),
			" lv ",
			item.miningSkillLevel,
			")"
		});
		failureReason = text;
		return false;
	}
	return false;
}
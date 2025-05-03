	public bool TryToScrubBlock(Vector2 blockPosition) {
		if (this.primaryItem == null || !this.primaryItem.IsScrubbingTool()) {
			return false;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		if (zoneBlock != null) {
			this.avatar.interactionItem = zoneBlock.ScrubbableItem();
			this.avatar.interactionPosition = blockPosition;
			if (this.miningBlockPositionChangedAt == 0f || this.miningBlockPosition != blockPosition) {
				this.miningBlockPositionChangedAt = Time.time;
				this.miningBlockPosition = blockPosition;
			}
			float num = Time.time - this.miningBlockPositionChangedAt;
			string text;
			if (this.CanScrubBlock(blockPosition, false, out text, false)) {
				if (this.ScrubBlock(blockPosition, num)) {
					this.miningBlockPositionChangedAt = 0f;
				}
				return true;
			}
			if (text == Locale.Text("mine.protected", null, null, null) || text == Locale.Text("mine.protected.diggable", null, null, null)) {
				this.lastMinedProtectedAt = Time.time;
			}
			if (num > 1.5f && text != null) {
				Notification.Create(text, 1);
			}
		}
		else {
			this.avatar.interactionItem = null;
		}
		return false;
	}
	public void TryToUseTool(Vector2 blockPosition, Vector2 worldPosition) {
		if (this.IsPrimaryItemGun()) {
			this.TryToShoot(worldPosition);
			return;
		}
        if (this.IsPrimaryItemScrubbingTool()) {
            if (!this.TryToScrubBlock(blockPosition)) {
			    this.SwingTool(worldPosition);
            }
        }
		if (!this.TryToMineBlock(blockPosition)) {
			this.SwingTool(worldPosition);
		}
	}
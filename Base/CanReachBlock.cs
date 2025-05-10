public bool CanReachBlock(Vector2 blockPosition) {
	if (this.awesomeMode) {
		return true;
	}
	if (this.canReachBlockCache.ContainsKey(blockPosition)) {
		return this.canReachBlockCache[blockPosition];
	}
	Vector2 vector = new Vector2(blockPosition.x + 0.5f, -blockPosition.y - 0.5f);
	ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)blockPosition.x, (int)blockPosition.y, false);
	if (zoneBlock == null || zoneBlock.sceneBlock == null) {
		this.canReachBlockCache[blockPosition] = false;
		return false;
	}
	ZoneBlock zoneBlock2 = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
	int index = zoneBlock.index;
	int num = ((zoneBlock2 == null) ? (-1) : zoneBlock2.index);
	foreach (Vector2 vector2 in this.RaycastOrigins()) {
		Vector2 vector3 = vector - vector2;
		float num2 = Mathf.Min(new float[] { vector3.magnitude });
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, vector3, num2, this.foregroundLayerMask);
		if (!(raycastHit2D.collider != null)) {
			this.canReachBlockCache[blockPosition] = true;
			return true;
		}
		BlockCollider blockCollider = raycastHit2D.transform.GetComponent<BlockCollider>();
		if (blockCollider == null) {
			blockCollider = raycastHit2D.transform.parent.GetComponent<BlockCollider>();
		}
		if (blockCollider != null && (blockCollider.blockIndex == index || blockCollider.blockIndex == num)) {
			this.canReachBlockCache[blockPosition] = true;
			return true;
		}
	}
	this.canReachBlockCache[blockPosition] = false;
	return false;
}

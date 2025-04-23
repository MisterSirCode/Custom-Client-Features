	public ZoneBlock AccessibleBlock(int x, int y)
	{
		ZoneBlock zoneBlock = this.Block(x, y, false);
		if (zoneBlock != null) {
			if (zoneBlock.IsAccessible(Item.Layer.Front, 0, 0)) {
				return zoneBlock;
			}
			for (int i = 0; i < this.adjacentMining.Length; i++) {
				int[] array = this.adjacentMining[i];
				ZoneBlock zoneBlock2 = this.Block(x + array[0], y + array[1], false);
				if (zoneBlock2 != null && zoneBlock2.IsAccessible(Item.Layer.Front, -array[0], array[1])) {
					return zoneBlock2;
				}
			}
			if (zoneBlock.IsAccessible(Item.Layer.Back, 0, 0)) {
				return zoneBlock;
			}
		}
		return null;
	}
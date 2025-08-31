public bool TryToDropItem(Vector2 worldPosition, Item item) {
	EntityAvatar entityAvatar = ReplaceableSingleton<Ecosystem>.main.NearbyPeer(worldPosition, 0.25f);
	// If the entity is a player, send a "trade" entity use command
	if (entityAvatar != null) {
		Command.Send(Command.Identity.EntityUse, new object[] {
			entityAvatar.entityId,
			new object[] { "trade", item.code },
		});
		return true;
	}

	// If the entity is a NPC, send an "item" entity use command
	Entity entity = ReplaceableSingleton<Ecosystem>.main.NearbyUsableEntity(worldPosition, 0.25f);
	if (entity != null && entity.config.usable) {
		Command.Send(Command.Identity.EntityUse, new object[] {
			entity.entityId,
			new object[] { "item", item.code },
		});
		return true;
	}

	// If there is a block that has the command use type, use the block with the item code
	Zone main = ReplaceableSingleton<Zone>.main;
	Vector2 vector = main.WorldToBlockPosition(worldPosition);
	ZoneBlock zoneBlock = main.AccessibleBlock((int)vector.x, (int)vector.y);
	if (zoneBlock != null && zoneBlock.frontItem.useTypes.Contains(Item.Use.Command)) {
		new All(zoneBlock.frontItem).SendCommand(
			zoneBlock,
			new object[] { "item", item.code }
		);
		return true;
	}
	return false;
}

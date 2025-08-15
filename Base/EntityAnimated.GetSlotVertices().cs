protected float[] GetSlotVertices(string slotName) {
	Slot slot = this.skeleton.skeleton.FindSlot(slotName);
	float[] array = new float[8];
	if (slot != null) {
		RegionAttachment regionAttachment = (RegionAttachment)slot.Attachment;
		if (regionAttachment != null) {
			regionAttachment.ComputeWorldVertices(0f, 0f, slot.bone, array);
			Quaternion rotation = this.skeletonTransform.localRotation;
			// There seems to be an offset in the skeleton position relative to the entity.
			// This is more prominent for players other than the currently playing player.
			// Skeleton is 0.09997559 too high for the current player and 0.9885864 for other players.
			// For simplicity we consider the current player to be properly aligned so we use the difference.
			float yAdjustment = this.isPlayer ? 0.0f : -0.88861081f;
			for (int i = 0; i < 8; i += 2) {
				float offsetX = array[i];
				float offsetY = array[i + 1];
				Vector3 offset = rotation * new Vector3(offsetX, offsetY);
				array[i] = base.transform.position.x + offset.x;
				array[i + 1] = base.transform.position.y + yAdjustment + offset.y;
			}
		}
	}
	return array;
}

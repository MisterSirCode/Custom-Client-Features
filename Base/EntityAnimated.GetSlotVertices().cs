protected float[] GetSlotVertices(string slotName) {
	Slot slot = this.skeleton.skeleton.FindSlot(slotName);
	float[] array = new float[8];
	if (slot != null) {
		RegionAttachment regionAttachment = (RegionAttachment)slot.Attachment;
		if (regionAttachment != null) {
			regionAttachment.ComputeWorldVertices(0f, 0f, slot.bone, array);
			Quaternion rotation = this.skeletonTransform.localRotation;
			for (int i = 0; i < 8; i += 2) {
				float offsetX = array[i];
				float offsetY = array[i + 1];
				Vector3 offset = rotation * new Vector3(offsetX, offsetY);
				array[i] = base.transform.position.x + offset.x;
				array[i + 1] = base.transform.position.y + offset.y;
			}
		}
	}
	return array;
}

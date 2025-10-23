public bool MomentaryAccessoryEnded(Item item) {
	return item != null && item.firingDuration > 0f && this.lastAccessoryActivatedAt.GetFloat(item.action, -100f) + item.firingDuration < Time.time;
}

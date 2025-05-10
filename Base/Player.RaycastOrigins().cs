public Vector2[] RaycastOrigins() {
	if (this.avatarTransform == null || this.avatarBoxCollider == null) {
		return new Vector2[0];
	}
	return new Vector2[] {
		new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y + this.avatarBoxCollider.size.y / 2f),
		new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y - this.avatarBoxCollider.size.y / 2f),
		new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x - this.avatarBoxCollider.size.x / 2f, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y),
		new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x + this.avatarBoxCollider.size.x / 2f, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y)
	};
}
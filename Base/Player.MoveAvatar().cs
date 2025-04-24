private void MoveAvatar()
{
    Vector3 position = this.avatarTransform.position;
    this._velocity.z = 0f;
    this.avatarController2D.move((this._velocity + this.externalVelocity) * Time.deltaTime);
    this._velocity = (this.avatarTransform.position - position) / Time.deltaTime - this.externalVelocity;
    if (this.IsGrounded()) {
        this.lastGroundedAt = Time.time;
    }
}
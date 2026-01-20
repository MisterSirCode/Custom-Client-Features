private void StepMovement() {
    if (this.isTeleporting) {
        return;
    }
    if (Time.time > this.lastMovedAt + 0.5f) {
        this.inputVelocity = Vector2.Lerp(this.inputVelocity, Vector2.zero, Time.deltaTime * 5f);
    }
    if (Time.time < this.lastJumpedAt + this.JumpMinDuration()) {
        this.inputVelocity.y = 1f;
    }
    bool flag = this.avatar.currentAnimationName != "walk" && this.avatar.currentAnimationName != "run";
    bool wasAirborne = this.movement == Player.Movement.Hovering || this.movement == Player.Movement.Flying || this.movement == Player.Movement.Falling;
    bool flag2 = this.IsAlive() && Mathf.Abs(this.inputVelocity.x) > 0.1f;
    bool flag3 = this.IsAlive() && this.inputVelocity.y > ((!this.IsGrounded()) ? 0.1f : 0.5f);
    bool flag4 = this.IsAlive() && this.inputVelocity.y < -0.2f;
    bool flag5 = this.IsAlive() && this.flyAccessory != null && this.HasSteam() && !this.suppressed && flag3;
    bool flag6 = this.IsAlive() && this.HasSteam() && this.stompAccessory != null && Time.time > this.lastStompedAt + this.stompAccessory.AttackInterval() && flag4;
    Player.Movement movement = this.movement;
    this.movement = Player.Movement.Idle;
    this.rotation = 0;
    if (this.collidingBlock == null) {
        this.externalVelocity.x = Mathf.Lerp(this.externalVelocity.x, 0f, Time.deltaTime);
    } else if (!this.collidingBlock.frontItem.IsUsableType(Item.Use.Move)) {
        this.externalVelocity.x = 0f;
    }
    if (this.CheckOverlayPercentage(0.5f)) {
        if (!this.movementLocked) {
            this.movementLocked = true;
            Messenger.Broadcast("playerMovementLocked");
        }
        if (this._clip) {
            return;
        }
    } else if (this.movementLocked) {
        this.movementLocked = false;
        Messenger.Broadcast("playerMovementUnlocked");
    }
    if (this.submerged) {
        float gravity = 0.333f;
        if (!flag2 && !flag3 && !flag4) {
            this.Move(0f, 0f, 0f, gravity);
            return;
        }
        if (!flag5 || !this.flyAccessory.IsUsableType(Item.Use.Propel)) {
            this.movement = Player.Movement.Swimming;
            this.Move(0.333f, 0.333f, 0.333f, gravity);
            return;
        }
        this.movement = Player.Movement.Propelling;
        if (this.UseFlyAccessory()) {
            this.Move(0.667f, 0.667f, 0.667f, gravity);
            return;
        }
    } else {
        if (this.CanClimb() && !this.IsGrounded() && this.inputVelocity.y > 0f) {
            this.movement = Player.Movement.Climbing;
            this.Move(0.5f, 0.7f, 0f, 1f);
            return;
        }
        if (Time.time < this.lastJumpedAt + this.JumpMaxDuration()) {
            this.movement = Player.Movement.Jumping;
            this.Move(1f, 1f, 0f, 1f);
            return;
        }
        if (this.IsGrounded()) {
            this.avatar.WalkOn(this.collidingBlock, wasAirborne);
            if (movement == Player.Movement.Stomping) {
                this.StompComplete();
            }
            if (movement == Player.Movement.Flailing) {
                this.currentEmoteAnimation = new PlayerAnimation("land", false);
            }
            if (flag3 && Time.time > this.lastJumpedAt) {
                this.movement = Player.Movement.Jumping;
                this.lastJumpedAt = (this.lastPropelledUpwardAt = Time.time);
                this.Move(1f, 1f, 0f, 1f);
                return;
            }
            if (flag2) {
                if (flag) {
                    this.startedRunningAt = Time.time;
                }
                this.movement = ((Mathf.Abs(this._velocity.x) <= 4f || Time.time <= this.startedRunningAt + Player.walkToRunDelay) ? Player.Movement.Walking : Player.Movement.Running);
                this.Move((this.movement != Player.Movement.Running) ? 0.8f : 1f, 0f, 0f, 1f);
            } else {
                this._velocity.x = this._velocity.x * 0.6f;
                this.Move(0f, 0f, 0f, 1f);
            }
            if (this.collidingBlock != null && this.collidingBlock.frontItem.IsUsableType(Item.Use.Move)) {
                int direction = (this.collidingBlock.frontMod == 0) ? 1 : -1;
                this.externalVelocity.x = Mathf.Lerp(this.externalVelocity.x, (float)direction * 5f, Time.deltaTime * 5f);
            }
            return;
        } else if (flag5 && this.flyAccessory.IsUsableType(Item.Use.Fly)) {
            this.movement = Player.Movement.Flying;
            this.rotation = (int)Mathf.Round(30f * this._velocity.x / this.MaxHorizontalSpeed());
            this.lastPropelledUpwardAt = Time.time;
            if (this.UseFlyAccessory()) {
                this.Move(1f, 0.75f, 0f, 1f);
                return;
            }
        } else {
            if (flag6) {
                this.movement = Player.Movement.Stomping;
                this.Move(0.3f, 0f, 3f, 1f);
                this.UseStompAccessory();
                return;
            }
            if (this._velocity.y == 0f) {
                this.movement = Player.Movement.Balancing;
                if (!flag2) {
                    ZoneBlock zoneBlock = this.BelowBlock();
                    if (zoneBlock != null && !zoneBlock.IsObstacle()) {
                        ZoneBlock zoneBlock2 = this.LeftBlock();
                        ZoneBlock zoneBlock3 = this.RightBlock();
                        if (zoneBlock2 != null && !zoneBlock2.IsObstacle() && zoneBlock3 != null && !zoneBlock3.IsObstacle()) {
                            float num = (Mathf.Repeat(this.position.x, 1f) <= 0.5f) ? 1f : -1f;
                            this._velocity.x = 0.6f * num;
                        }
                    }
                }
            } else if (Time.time > this.lastGroundedAt + 3f && Time.time > this.lastPropelledUpwardAt + 3.333f && this.flyAccessory != null && !this.flyAccessory.IsUsableType(Item.Use.Hover) && this.currentLiquidLevel <= 4) {
                this.movement = Player.Movement.Flailing;
            } else {
                this.movement = Player.Movement.Falling;
            }
            this.Move(1f, 0f, 0f, 1f);
        }
    }
}
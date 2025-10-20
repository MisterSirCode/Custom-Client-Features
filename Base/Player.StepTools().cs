private void StepTools() {
    if (this.activeShield != null) {
        this.UseSteam(Mathf.Max(this.activeShield.rate, 1f), true, false);
        if (this.activeShield.rate > 0.0f && this.steam <= 0f) {
            this.ToggleShield(false);
        }
        if (this.activeShield.firingDuration > 0.0f && Time.time > this.lastActivatedShieldAt + this.activeShield.firingDuration) {
            this.ToggleShield(false);
        }
    }
    if (this.wasShooting && Time.time > this.lastWasShootingAt + 0.2f) {
        this.StopShooting();
    }
}

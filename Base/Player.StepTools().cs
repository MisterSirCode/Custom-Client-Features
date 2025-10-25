private void StepTools() {
    if (this.activeShield != null) {
        this.UseSteam(this.activeShield.rate, true, false);
        if (this.activeShield.rate > 0f && this.steam <= 0f) {
            this.ToggleShield(false);
        }
        if (this.activeShield.firingDuration > 0f && this.MomentaryAccessoryEnded(this.activeShield)) {
            StartCoroutine("MomentaryAccessoryFollowUp", new object[] { this.activeShield });
            this.ToggleShield(false);
        }
    }
    if (this.wasShooting && Time.time > this.lastWasShootingAt + 0.2f) {
        this.StopShooting();
    }
}

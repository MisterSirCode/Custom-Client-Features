private void OnShieldOff() {
    if (this.activeShield == null || this.activeShield.firingDuration <= 0f) {
        this.ToggleShield(false);
    }
}

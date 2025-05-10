private bool ShouldBeEnabled() {
    return GameManager.IsMobile() || GameManager.IsSteamdeck();
}
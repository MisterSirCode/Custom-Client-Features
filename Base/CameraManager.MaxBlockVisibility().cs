private ObscuredInt MaxBlockVisibility() {
    if (GameManager.IsTiny() && !GameManager.IsSteamdeck()) {
        return 2000;
    }
    return 3000;
}
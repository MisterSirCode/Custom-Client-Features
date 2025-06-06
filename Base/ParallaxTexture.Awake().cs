private void Awake() {
    this.mainCameraTransform = ReplaceableSingleton<CameraManager>.main.mainCameraTransform;
    this.cMaterial = base.GetComponent<Renderer>().material;
    this.defaultScale = this.container.localScale;
    this.defaultZoom = ReplaceableSingleton<CameraManager>.main.mainCamera.orthographicSize;
    if (base.gameObject.name == "Cavern Front") {
        this.offsetFactor = 0.0025f;
        return;
    }
    if (base.gameObject.name == "Cavern") {
        this.offsetFactor = 0.01f;
        this.transform.GetChild(1).localPosition = new Vector3(0f, 0f, 1f);
    }
}
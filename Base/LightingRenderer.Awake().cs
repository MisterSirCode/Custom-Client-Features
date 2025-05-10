private void Awake() {
    Messenger.AddListener<bool>("lightingVisibilityChanged", new Callback<bool>(this.OnLightingVisibilityChanged));
    Messenger.AddListener<bool>("moodyCommandUsed", new Callback<bool>(this.OnMoodyCommandUsed));
    this.renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
    this.lightingCamera.targetTexture = this.renderTexture;
    MeshRenderer meshRenderer = (MeshRenderer)this.lightingOverlayTransform.GetComponent(typeof(MeshRenderer));
    meshRenderer.material.mainTexture = this.renderTexture;
    meshRenderer.material.color = new Color(0f, 0f, 0f, 0.4f);
    Mesh mesh = ((MeshFilter)this.lightingOverlayTransform.GetComponent(typeof(MeshFilter))).mesh;
    Vector2[] uv = mesh.uv;
    for (int i = 0; i < uv.Length; i++) {
        uv[i] = new Vector2(uv[i].x, (uv[i].y != 0f) ? 0f : 1f);
    }
    mesh.uv = uv;
    Blur component = base.GetComponent<Blur>();
    if (component != null) {
        component.enabled = true;
        component.iterations = ((!GameManager.IsMobile()) ? 5 : 3);
        component.blurSpread = 1.5f;
    }
}
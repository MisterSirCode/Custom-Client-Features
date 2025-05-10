private void OnMoodyCommandUsed(bool any) {
    if (this.moodyEnabled) {
        this.moodyEnabled = false;
        MeshRenderer meshRenderer = (MeshRenderer)this.lightingOverlayTransform.GetComponent(typeof(MeshRenderer));
        meshRenderer.material.color = new Color(0f, 0f, 0f, 0.4f);
    } else {
        this.moodyEnabled = true;
        MeshRenderer meshRenderer = (MeshRenderer)this.lightingOverlayTransform.GetComponent(typeof(MeshRenderer));
        meshRenderer.material.color = new Color(0f, 0f, 0f, 0.7f);
    }
}
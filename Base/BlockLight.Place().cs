public void Place(Vector2 blockPosition, Item item) {
    Vector3[] vertices = this.lightMesh.vertices;
    Color[] array = new Color[vertices.Length];
    for (int i = 0; i < vertices.Length; i++) {
        vertices[i] = vertices[i].normalized * item.light * 1.5f;
        if (BlocklightConsoleCommand.globalColorOverride != null)
            array[i] = item.lightColor * BlocklightConsoleCommand.globalColorOverride;
        else
            array[i] = item.lightColor;
    }
    this.lightMesh.vertices = vertices;
    this.lightMesh.colors = array;
    this.lightMesh.RecalculateBounds();
    base.transform.position = new Vector3(blockPosition.x + item.lightPosition.x, blockPosition.y - item.lightPosition.y, 0f);
}
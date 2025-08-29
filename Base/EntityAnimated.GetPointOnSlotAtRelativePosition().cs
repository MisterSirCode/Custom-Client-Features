public Vector2 GetPointOnSlotAtRelativePosition(string slotName, Vector2 relativePosition) {
    float[] slotVertices = this.GetSlotVertices(slotName);
    Vector2 p0 = new Vector2(slotVertices[6], slotVertices[7]);
    Vector2 px = new Vector2(slotVertices[0], slotVertices[1]);
    Vector2 py = new Vector2(slotVertices[4], slotVertices[5]);
    return p0 + relativePosition.x * (px - p0) + relativePosition.y * (py - p0);
}

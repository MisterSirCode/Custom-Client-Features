public IEnumerator AnimateJiggle(object[] parameters) {
    float jiggle = (float)parameters[0];
    float blockWidth = (float)parameters[1];
    float blockHeight = (float)parameters[2];
    float magnitudePeriod = 0.4f;
    float jigglePeriod = 0.05f;
    // We aim for the tip of the item to move roughly proportionally to the jiggle amount.
    float jiggleRangeDegrees = 20.0f * jiggle / blockHeight;
    float desyncDelay = magnitudePeriod * (1067.0f * transform.position.x + 723.0f * transform.position.y) / 1982.0f;
    // float desyncDelay = magnitudePeriod * transform.position.GetHashCode() / 1e9f;
    Vector3 localPosition = LocalPosition();
    // Block is normally pivoted around halfway through the block, and y increases upwards.
    Vector3 pivot = new Vector3(blockWidth / 2.0f - 0.5f, -0.5f, 0.0f);

    for (;;) {
        float time = UnityEngine.Time.time + desyncDelay;
        float angle = (float)(0.5f * jiggleRangeDegrees * Math.Max(0.0, Math.Sin(time / magnitudePeriod)) * Math.Sin(time / jigglePeriod));
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.SetPosition(rotation * (localPosition - pivot) + pivot);
        this._transform.rotation = rotation;
        // Sync to frames
        yield return null;
    }
}

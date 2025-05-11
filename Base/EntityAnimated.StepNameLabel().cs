private void StepNameLabel(Rect screenRect) {
    if (this.nameLabel != null) {
        float num = ReplaceableSingleton<CameraManager>.main.lightingCamera.orthographicSize / 6f;
        this.nameLabel._gameObject.transform.localScale = new Vector3(num, num, 1f);
        if (this.IsVisible()) {
            BoxCollider2D boxCollider2D = (BoxCollider2D)this._collider2D;
            this.nameLabel.position = new Vector3(boxCollider2D.offset.x + base.cTransform.position.x, boxCollider2D.offset.y + boxCollider2D.size.y / 2f + 0.1f + base.cTransform.position.y, 0f);
            this.nameLabel.offscreen = false;
            this.nameLabel.SetActive(true);
            return;
        }
        if (this.IsVisibleOffscreen()) {
            Vector3 vector = ReplaceableSingleton<Zone>.main.WorldToScreenPosition(this._transform.position);
            Intersection intersection = screenRect.SegmentIntersection(new Segment(screenRect.center, vector));
            this.nameLabel.position = ReplaceableSingleton<Zone>.main.ScreenToWorldPosition(intersection.point);
            this.distance = new Vector2(ReplaceableSingleton<Zone>.main.cameraRect.center.x, -ReplaceableSingleton<Zone>.main.cameraRect.center.y) - this._transform.position;
            this.nameArrowAngle = Mathf.LerpAngle(this.nameArrowAngle, Mathf.Atan2(this.distance.y, this.distance.x) * 57.29578f, 0.2f);
            this.nameLabel.arrowRotation = this.nameArrowAngle;
            this.nameLabel.offscreen = true;
            this.nameLabel.SetActive(true);
            return;
        }
        this.nameLabel.SetActive(false);
    }
}
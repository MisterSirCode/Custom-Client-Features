private void CheckGrounding()
{
    this.grounded = true;
    this.partiallyGrounded = false;
    this.collidingBlock = null;
    float num = 0.2f;
    float num2 = 6f;
    int num3 = 0;
    while ((float)num3 < num2) {
        float x = Mathf.Lerp(-num, num, (float)num3 / (num2 - 1f));
        RaycastHit2D raycastHit2D = Physics2D.Raycast(this.avatarTransform.position + new Vector3(this.avatarBoxCollider.offset.x, this.avatarBoxCollider.offset.y, 0f) + new Vector3(x, 0f, 0f), new Vector3(0f, -1f, 0f), this.avatarBoxCollider.size.y * 0.5f + 0.1f, this.groundedLayerMask);
        if (raycastHit2D.collider != null) {
            BlockCollider component = raycastHit2D.transform.GetComponent<BlockCollider>();
            if (component == null) {
                component = raycastHit2D.collider.gameObject.GetComponentInParent<BlockCollider>();
            }
            if (component != null) {
                Zone zone = ReplaceableSingleton<Zone>.main;
                ZoneBlock zoneBlock = zone.Block(component.blockIndex % zone.blockSize.width, component.blockIndex / zone.blockSize.width, false);
                this.collidingBlock = zoneBlock;
            }
            this.partiallyGrounded = true;
        } else {
            this.grounded = false;
        }
        num3++;
    } 
}
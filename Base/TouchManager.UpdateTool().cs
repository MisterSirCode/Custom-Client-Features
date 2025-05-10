private void UpdateTool() {
    if (ReplaceableSingleton<Player>.main.primaryItem == null) {
        return;
    }
    InputDevice activeDevice = InputManager.ActiveDevice;
    bool flag = ReplaceableSingleton<Player>.main.primaryItem.IsSwingableTool();
    bool flag2 = ReplaceableSingleton<Player>.main.primaryItem.IsMiningTool();
    bool flag3 = ReplaceableSingleton<Player>.main.primaryItem.IsWeapon();
    float value = activeDevice.GetControl(InputControlType.RightStickX).Value;
    float value2 = activeDevice.GetControl(InputControlType.RightStickY).Value;
    Vector2 vector = new Vector2(value, -value2);
    Vector2 vector2 = ReplaceableSingleton<Player>.main.position + vector.normalized + this.miningOffset;
    this.lastActiveToolPosition = vector2;
    this.lastActiveToolRotation = Mathf.Atan2(value2, value) * 57.29578f;
    if (vector.magnitude > 0.05f && (flag || flag3)) {
        Vector2 vector3 = vector2;
        if (flag2) {
            ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)vector3.x, (int)vector3.y, false);
            if (zoneBlock == null || ((zoneBlock.frontItem.code == 0 || zoneBlock.frontItem.code == 519) && zoneBlock.backItem.code == 0)) {
                vector3 += vector.normalized;
            }
            vector3 = new Vector2(Mathf.Floor(vector3.x), Mathf.Floor(vector3.y));
        } else if (!flag) {
            float num = 5f;
            vector2.x += vector.normalized.x * num;
            vector2.y += vector.normalized.y * num;
        }
        vector2.y *= -1f;
        if (!this.triedToInteract) {
            if (ReplaceableSingleton<Player>.main.CanReachBlock(vector3)) {
                ReplaceableSingleton<Player>.main.TryToUseAtPosition(vector2);
            }
            this.triedToInteract = true;
        }
        ReplaceableSingleton<Player>.main.TryToUseTool(vector3, vector2);
    }
    if ((double)Mathf.Abs(value) <= 0.1 && (double)Mathf.Abs(value2) <= 0.1) {
        this.triedToInteract = false;
    }
    if (flag) {
        this.miningSpike.enabled = Time.time > ReplaceableSingleton<Player>.main.avatar.animateToolBeganAt + 0.05f;
        this.miningSpike.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.9f, 0f, (Time.time - ReplaceableSingleton<Player>.main.avatar.animateToolBeganAt) * 3f));
        this.miningSpikeTransform.rotation = Quaternion.Euler(0f, 0f, this.lastActiveToolRotation);
        Vector2 vector4 = new Vector2(this.lastActiveToolPosition.x, -this.lastActiveToolPosition.y) + this.miningSpikeOffset;
        this.miningSpikeTransform.anchoredPosition = ReplaceableSingleton<Zone>.main.WorldToScreenPosition(vector4) / CanvasScalerHelper.ScaleFactor();
        return;
    }
    this.miningSpike.enabled = false;
}
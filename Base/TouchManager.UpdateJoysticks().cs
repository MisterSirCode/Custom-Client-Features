private void UpdateJoysticks() {
    if (GameManager.IsSteamdeck()) {
        this.leftJoystick.gameObject.SetActive(false);
        this.rightJoystick.gameObject.SetActive(false);
        return;
    }
    bool flag = !ReplaceableSingleton<GameGui>.main.IsPrimaryWindowGroupVisible("game");
    bool flag2 = flag && ReplaceableSingleton<Player>.main.primaryItem != null && ReplaceableSingleton<Player>.main.primaryItem.IsTool();
    float num = 0.5f;
    float num2 = Screen.dpi * 0.5f;
    float num3 = Screen.dpi * 1.25f;
    float num4 = Mathf.Lerp(num2, num3, num);
    float num5 = num4 / 2f;
    float num6 = num4 / 2f + Screen.dpi * 0.175f;
    TouchSprite ring = this.leftJoystick.ring;
    Vector2 vector = new Vector2(num4, num4);
    this.rightJoystick.ring.Size = vector;
    ring.Size = vector;
    TouchSprite knob = this.leftJoystick.knob;
    vector = new Vector2(num4 / 3f, num4 / 3f);
    this.rightJoystick.knob.Size = vector;
    knob.Size = vector;
    this.leftJoystick.Offset = new Vector2(num5 + Screen.dpi * 0.3f, (!flag) ? (-9999f) : num6);
    this.rightJoystick.Offset = new Vector2(-num5 - Screen.dpi * 0.4f, (!flag2) ? (-9999f) : num6);
    this.leftJoystick.knobRange = (this.rightJoystick.knobRange = num4 / 3f * 0.9f);
}
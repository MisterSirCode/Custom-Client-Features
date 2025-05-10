private void UpdateMovement() {
    InputDevice activeDevice = InputManager.ActiveDevice;
    float value = activeDevice.GetControl(InputControlType.LeftStickX).Value;
    float value2 = activeDevice.GetControl(InputControlType.LeftStickY).Value;
    value = Mathf.Clamp(value * 1.414f, -1f, 1f);
    value2 = Mathf.Clamp(value2 * 1.414f, -1f, 1f);
    ReplaceableSingleton<Player>.main.InputMovement(value, value2);
}
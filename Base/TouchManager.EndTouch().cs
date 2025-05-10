private void EndTouch(global::InControl.Touch touch) {
    if (touch == this.rightJoystick.currentTouch) {
        this.pointerManager.OnPointerUp(touch.position);
    }
    if (this.sceneTouches.ContainsKey(touch.fingerId)) {
        this.sceneTouches.Remove(touch.fingerId);
    }
}
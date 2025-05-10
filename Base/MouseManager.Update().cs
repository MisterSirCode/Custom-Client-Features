private void Update() {
    if (GameManager.IsMobile()) {
        return;
    }
    if (Zone.IsActive() && !ReplaceableSingleton<GameGui>.main.IsModal()) {
        this.pointerManager.OnPointerMove(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) || Singleton<KeyboardManager>.main.IsKeyDown("Interact")) {
            this.pointerManager.OnPointerDown(Input.mousePosition);
        } else if (Input.GetMouseButton(0) || Singleton<KeyboardManager>.main.IsKeyHeld("Interact")) {
            this.pointerManager.OnPointerHeld(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0) || Singleton<KeyboardManager>.main.IsKeyUp("Interact")) {
            this.pointerManager.OnPointerUp(Input.mousePosition);
        }
        if (Input.GetAxis("Mouse Wheel") != 0f && !this.pointerManager.IsPointerBlockedByGui()) {
            float axis = Input.GetAxis("Mouse Wheel");
            int num = ((axis <= 0f) ? ((int)Mathf.Floor(axis)) : ((int)Mathf.Ceil(axis)));
            Messenger.Broadcast<int>("activeHotbarSlotIncremented", num);
        }
    }
}
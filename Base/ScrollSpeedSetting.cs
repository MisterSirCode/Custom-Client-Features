using UnityEngine;
using UnityEngine.UI;

class ScrollSpeedSetting : MonoBehaviour {
    private void Awake() {
        scrollRect = this.GetComponent<ScrollRect>();
        Messenger.AddListener("settingChanged", new Callback<string, object>(OnSettingChanged));
        if (!PlayerPrefs.HasKey("scrollSpeed")) {
            PlayerPrefs.SetFloat("scrollSpeed", 18f);
        }
    }

    private void OnSettingChanged(string setting, object value) {
        if (setting != null && scrollRect != null && setting == "scrollSpeed")
        {
            scrollRect.scrollSensitivity = factor * (float)value;
        }
    }

    public ScrollRect scrollRect {
        get {
            return _scrollRect;
        }
        set {
            _scrollRect = value;
            if (_scrollRect != null) {
                _scrollRect.scrollSensitivity = PlayerPrefs.GetFloat("scrollSpeed", _scrollRect.scrollSensitivity);
            }
        }
    }

    private ScrollRect _scrollRect;
    public float factor = 1f;
}

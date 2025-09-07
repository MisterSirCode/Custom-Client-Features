using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EmotesPanel : MonoBehaviour {
	public EmotesPanel() { }

	public void Awake() {
		Messenger.AddListener("wardrobeInitialized", new Callback(this.OnWardrobeInitialized));
	}

	private void OnWardrobeInitialized() {
		foreach (Item item in Config.main.itemsByCategory["emotes"]) {
			if (item.isBase) this.IndexEmote(item);
		}
		this.UpdateEmotes();
	}

	private void IndexEmote(Item item) {
		if (item == null) {
			return;
		}
		if (item.category != "emotes") {
			return;
		}
		if (this.currentButtons.ContainsKey(item.code)) {
			return;
		}
		GameObject gameObject = this.CreateEmoteButton(item);
		gameObject.transform.SetParent(base.gameObject.transform, false);
		this.currentButtons.Add(item.code, gameObject);
	}

	private void UnindexEmote(Item item) {
		if (this.currentButtons.ContainsKey(item.code)) {
			global::UnityEngine.Object.Destroy(this.currentButtons.Get(item.code));
			this.currentButtons.Remove(item.code);
		}
	}

    private GameObject CreateEmoteButton(Item item) {
        GameObject button = new GameObject(item.title + " Button", typeof(Button), typeof(RectTransform), typeof(EmoteButton));
        GameObject image = new GameObject(item.title + " Button Image", typeof(RectTransform), typeof(RawImage));
        RawImage rawImage = image.GetComponent<RawImage>();
        string spriteName;
        if (item.InventorySpriteName().StartsWith("inventory/")) {
            spriteName = item.InventorySpriteName();
        } else{
            spriteName = "inventory/" + item.InventorySpriteName();
        }
        rawImage.texture = Singleton<AtlasManager>.main.SpriteTexture(spriteName, true);
		tk2dSpriteDefinition tk2DSpriteDefinition = Singleton<AtlasManager>.main.Sprite(spriteName);
        GuiExtensions.SetTk2dSprite(rawImage, tk2DSpriteDefinition);
        image.transform.SetParent(button.transform, false);
        image.transform.localScale *= EmoteButton.NORMAL_SCALE_FACTOR;
        ((RectTransform)image.transform).sizeDelta = GuiExtensions.AspectConstrainedSize(tk2DSpriteDefinition.rect.size, ((RectTransform)button.transform).sizeDelta, 4f);
		EmoteButton emoteButtonBehaviour = button.GetComponent<EmoteButton>();
        emoteButtonBehaviour.emote = item;
        emoteButtonBehaviour.image = image;
        return button;
    }

	public void UpdateEmotes() {
		Player player = ReplaceableSingleton<Player>.main;
		foreach (int x in currentButtons.Keys) {
			Item item = Item.Get(x);
			if(!item.isBase && !player.inventory.wardrobe.Contains(item)) {
			    UnindexEmote(item);
			}
		}
		foreach (Item item in player.inventory.wardrobe) {
		    IndexEmote(item);
		}
	}

	private Dictionary<int, GameObject> currentButtons = new Dictionary<int, GameObject>();
}

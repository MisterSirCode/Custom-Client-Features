using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalSpriteLoader : MonoBehaviour {
	public void Start() {
		if (ExternalSpriteLoader.instance != null) {
			return;
		}
		ExternalSpriteLoader.instance = this;
		this.assets = ExternalAssetManager.GetInstance().GetAssetsOfType("sprite");
        this.LoadAllSprites();
	}

	public void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	public IEnumerator LoadSpriteFile(ExternalAsset asset) {
		WWW www = new WWW(asset.path);
		yield return www;
		try {
            Texture2D texture = www.texture;
			//ExternalConsole.Log("Texture Size (KB)", Mathf.Round((float)www.bytesDownloaded / 1000f).ToString());
            asset.loaded = true;
			asset.SetData(Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f)));
            this.sprites.Add(asset);
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
		}
	}

    public void LoadAllSprites() {
        this.sprites = new List<ExternalAsset>();
        foreach (ExternalAsset asset in this.assets) {
            base.StartCoroutine(this.LoadSpriteFile(asset));
        }
    }

    public static Sprite GetSprite(string name) {
        foreach (ExternalAsset asset in ExternalSpriteLoader.instance.assets) {
            if (asset.name == name) {
                return (Sprite)asset.GetData();
            }
        }
        return null;
    }

	public static ExternalSpriteLoader instance;
    public List<ExternalAsset> assets;
    public List<ExternalAsset> sprites;
}
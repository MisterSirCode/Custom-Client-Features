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

	public IEnumerator LoadAssetMetadata(ExternalAsset asset) {
		string metaPath = asset.path + ".meta";
		if (!File.Exists(metaPath)) yield break;
		WWW www = new WWW(metaPath);
		yield return www;
		try {
			Dictionary<string, object> result = new Dictionary<string, object>();
			using (StringReader reader = new StringReader(www.text)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					int sep = line.IndexOf("=");
					if (sep != -1) result.Add(line.Substring(0, sep), line.Substring(sep + 1));
				}
			}
			asset.SetMeta(result);
			www.Dispose();
			yield break;
		} catch (Exception ex) {
			ExternalConsole.HandleException(asset.name + " Metadata Loader", ex);
			yield break;
		}
		yield break;
	}

	public IEnumerator LoadSpriteFile(ExternalAsset asset) {
		yield return base.StartCoroutine(ExternalSpriteLoader.instance.LoadAssetMetadata(asset));
		WWW www = new WWW(asset.path);
		yield return www;
		try {
            Texture2D texture = www.texture;
			//ExternalConsole.Log("Texture Size (KB)", Mathf.Round((float)www.bytesDownloaded / 1000f).ToString());
			asset.loaded = true;
			float top = 0.0f, bottom = 0.0f, left = 0.0f, right = 0.0f, pixelsPerUnit = 1.0f;
			if (asset.HasMeta()) {
				Dictionary<string, object> meta = asset.GetMeta();
				top = float.Parse(meta.GetString("top", "0").Trim());
				bottom = float.Parse(meta.GetString("bottom", "0").Trim());
				left = float.Parse(meta.GetString("left", "0").Trim());
				right = float.Parse(meta.GetString("right", "0").Trim());
                pixelsPerUnit = float.Parse(meta.GetString("pixelsPerUnit", "1").Trim());
			}
			Sprite sprite;
			if (top != 0f || bottom != 0f || left != 0f || right != 0f) {
				sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f), pixelsPerUnit, 0U, SpriteMeshType.FullRect, new Vector4(left, bottom, right, top));
			} else {
				sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f));
			}
			asset.SetData(sprite);
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
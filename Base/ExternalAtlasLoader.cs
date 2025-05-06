using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalAtlasLoader : MonoBehaviour {
	public struct ExternalAtlas {
        public ExternalAtlas(string title, ExternalAsset image, ExternalAsset text) {
			name = title;
            sprite = image;
            atlas = text;
        }
		public string name { get; set; }
        public ExternalAsset sprite { get; }
        public ExternalAsset atlas { get; }
        public override string ToString() => $"Atlas ${name} (Image {sprite.name}, Text {atlas.name})";
    }

	public void Start() {
		if (ExternalAtlasLoader.instance != null) {
			return;
		}
		ExternalAtlasLoader.instance = this;
		this.assets = ExternalAssetManager.GetInstance().GetAssetsOfType("atlas");
        this.atlasImages = new List<ExternalAsset>();
        this.atlasTexts = new List<ExternalAsset>();
        this.LoadAllAtlases();
	}

	public void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	public IEnumerator WaitForSomeCoroutines(IEnumerator[] ienumerators) {
        if (ienumerators != null & ienumerators.Length > 0) {
            Coroutine[] coroutines = new Coroutine[ienumerators.Length];
            for (int i = 0; i < ienumerators.Length; i++)
                coroutines[i] = StartCoroutine(ienumerators[i]);
            for (int i = 0; i < coroutines.Length; i++)
                yield return coroutines[i];
        } else yield return null;
    }

	public IEnumerator LoadAtlasFile(ExternalAsset asset) {
		WWW www = new WWW(asset.path);
		yield return www;
		try {
            if (asset.path.Contains(".txt")) {
                asset.loaded = true;
                asset.SetData(new TextAsset(www.text));
                this.atlasTexts.Add(asset);
            } else {
                Texture2D texture = www.texture;
                //ExternalConsole.Log("Texture Size (KB)", Mathf.Round((float)www.bytesDownloaded / 1000f).ToString());
                asset.loaded = true;
                asset.SetData(Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f)));
                this.atlasImages.Add(asset);
            }
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Atlas Failed", e.ToString()); 
		}
	}

	public IEnumerator GenerateAtlases(IEnumerator[] ienumerators) {
		yield return base.StartCoroutine(this.WaitForSomeCoroutines(ienumerators));
		foreach (ExternalAsset text in this.atlasTexts) {
			ExternalAsset match = this.atlasImages.Find(image => image.name == text.name);
			ExternalAtlas newAtlas = new ExternalAtlas(text.name, match, text);
			this.atlases.Add(newAtlas);
		}
	}

    public void LoadAllAtlases() {
        this.atlasImages = new List<ExternalAsset>();
        this.atlasTexts = new List<ExternalAsset>();
		List<IEnumerator> list = new List<IEnumerator>();
        foreach (ExternalAsset asset in this.assets) {
			list.Add(this.LoadAtlasFile(asset));
        }
		base.StartCoroutine(this.GenerateAtlases(list.ToArray()));
    }

	public static ExternalAtlasLoader instance;
    public List<ExternalAsset> assets;
    public List<ExternalAsset> atlasImages;
    public List<ExternalAsset> atlasTexts;
}
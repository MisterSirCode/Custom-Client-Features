using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalAtlasLoader : MonoBehaviour {
	public void Start() {
		if (ExternalAtlasLoader.instance != null) {
			return;
		}
		ExternalAtlasLoader.instance = this;
		this.assets = ExternalAssetManager.GetInstance().GetAssetsOfType("atlas");
        this.atlasImages = new List<ExternalAsset>();
        this.atlasTexts = new List<ExternalAsset>();
		this.atlases = new List<ExternalAtlas>();
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
            if (asset.path.Contains(".tk2d")) {
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
            try {
				ExternalAsset match = this.atlasImages.Find((ExternalAsset image) => image.name == text.name);
                if (match != null) {
                    ExternalAtlas newAtlas = new ExternalAtlas(text.name, match, text);
                    this.atlases.Add(newAtlas);
                }
            } catch(Exception err) {
                ExternalConsole.HandleException("Atlas Loader Error", err);
            }
		}
		ExternalConsole.Log("Atlases Loaded", this.atlases.Count.ToString());
		Singleton<AtlasManager>.main.LoadAtlases();
	}

    public void LoadAllAtlases() {
        this.atlasImages = new List<ExternalAsset>();
        this.atlasTexts = new List<ExternalAsset>();
		this.atlases = new List<ExternalAtlas>();
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
	public List<ExternalAtlas> atlases;
}
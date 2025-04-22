using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalSpriteLoader : MonoBehaviour
{
	public void Start()
	{
		if (ExternalSpriteLoader.instance != null)
		{
			return;
		}
		ExternalSpriteLoader.instance = this;
		this.assets = ExternalAssetManager.GetInstance().loaded;
	}

	public IEnumerator LoadSpriteFile(string path)
	{
		WWW www = new WWW(path);
		yield return www;
		try {
            Texture2D texture = www.texture;
			this.sprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0,0)));
			ExternalConsole.Log("Texture Size (KB)", Mathf.Round((float)www.bytesDownloaded / 1000f).ToString());
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
		}
	}

	public static ExternalSpriteLoader instance;
    public List<ExternalAsset> assets;
	public List<Sprite> sprites;
}
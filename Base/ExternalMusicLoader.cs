using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalMusicLoader : MonoBehaviour
{
	public void Start()
	{
		AudioSource newSource = Camera.current.gameObject.AddComponent<AudioSource>();
		this.controller = newSource;
		ExternalAsset testSong = ExternalAssetManager.instance.loaded.First<ExternalAsset>();
		ExternalConsole.Log("Loading " + testSong.name, testSong.GetRectifiedPath());
		base.StartCoroutine(this.LoadAudioFile(testSong.GetRectifiedPath()));
	}

	public IEnumerator LoadAudioFile(string path)
	{
		WWW www = new WWW(path);
		yield return www;
		try {
			testClip = www.GetAudioClip(false, false, AudioType.WAV);
			ExternalConsole.Log("Song Size (MB)", Mathf.Round((float)www.bytesDownloaded / 1000000f).ToString());
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
		}
	}

	public void PlayTestClip()
	{
		this.controller.volume = 1f;
		this.controller.clip = this.testClip;
		this.controller.Play();
	}

    public List<ExternalAsset> assets;
    public List<AudioClip> clips;
	public AudioSource controller;
	public AudioClip testClip;
}
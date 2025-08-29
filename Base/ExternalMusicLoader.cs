using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalMusicLoader : MonoBehaviour {
	public struct NamedClip {
		public NamedClip(string name, AudioClip clip, bool exists) {
			this.name = name;
			this.clip = clip;
			this.exists = exists;
		}

		public string name { get; }
		public AudioClip clip { get; }
		public bool exists { get; }
	};

	public void Start() {
		if (ExternalMusicLoader.instance != null) {
			return;
		}
		ExternalMusicLoader.instance = this;
		ExternalMusicLoader.clips = new List<NamedClip>();
		AudioSource newSource = Camera.current.gameObject.AddComponent<AudioSource>();
		this.controller = newSource;
		if (PlayerPrefs.HasKey("musicVolume"))
			this.controller.volume = PlayerPrefs.GetFloat("musicVolume");
		else
			this.controller.volume = 1f;
        this.waitingForNext = true;
        this.waitingForSong = true;
        this.playLoop = true;
		this.assets = ExternalAssetManager.GetInstance().GetAssetsOfType("music");
        this.queue = new List<ExternalAsset>();
	}

	public void Update() {
		if (this.playLoop && GameManager.IsGame()) {
            if (this.waitingForNext) {
				base.StartCoroutine(PlayNextInQueueWithDelay());
                this.waitingForNext = false;
            }
            if (!this.waitingForSong) {
                if (!this.controller.isPlaying) {
					this.waitingForSong = true;
					this.waitingForNext = true;
				}
            }
		}
        if (!GameManager.IsGame()) this.controller.Stop();
	}

	NamedClip GetNamedClip(string path) {
		foreach (NamedClip clip in ExternalMusicLoader.clips) {
			if (clip.name == path) return clip;
			else continue; 
		}
		return new NamedClip("", null, false);
	}

	public IEnumerator LoadAudioFile(string path) {
		NamedClip namedClip = GetNamedClip(path);
		if (namedClip.exists) {
			this.controller.clip = namedClip.clip;
			this.controller.Play();
			this.waitingForSong = false;
		} else {
			WWW www = new WWW(path);
			yield return www;
			try {
				NamedClip toWrite = new NamedClip(path, www.GetAudioClip(false, false, AudioType.WAV), true);
				ExternalMusicLoader.clips.Add(toWrite);
				//ExternalConsole.Log("Song Size (MB)", Mathf.Round((float)www.bytesDownloaded / 1000000f).ToString());
				this.controller.clip = toWrite.clip;
				this.controller.Play();
				this.waitingForSong = false;
				www.Dispose();
				yield break;
			} catch(Exception e) {
				ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
			}
		}
	}

    public void GenerateRandomQueue() {
        this.queue = new List<ExternalAsset>(this.assets);
        this.queue.Shuffle();
    }

    public void PlayNextInQueue()  {
        if (this.queue.Count > 0) {
		    base.StartCoroutine(this.LoadAudioFile(this.queue.First().GetRectifiedPath()));
            this.queue.RemoveAt(0);
        } else {
            this.GenerateRandomQueue();
            this.PlayNextInQueue();
        }
    }

    public IEnumerator PlayNextInQueueWithDelay() {
        yield return new WaitForSeconds(10);
        PlayNextInQueue();
        yield break;
    }

	public static ExternalMusicLoader instance;
	public static List<NamedClip> clips;
    public List<ExternalAsset> queue;
    public List<ExternalAsset> assets;
	public AudioSource controller;
    public bool playLoop;
    public bool waitingForNext;
    public bool waitingForSong;
}
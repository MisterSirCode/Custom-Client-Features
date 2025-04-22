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
		if (ExternalMusicLoader.instance != null)
		{
			return;
		}
		ExternalMusicLoader.instance = this;
		AudioSource newSource = Camera.current.gameObject.AddComponent<AudioSource>();
		this.controller = newSource;
        this.waitingForNext = true;
        this.waitingForSong = true;
        this.playLoop = false;
        this.queue = new List<ExternalAsset>();
	}

	public void Update()
	{
		if (this.playLoop && Zone.IsActive()) {
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
        if (!Zone.IsActive()) this.controller.Stop();
	}

	public IEnumerator LoadAudioFile(string path)
	{
		WWW www = new WWW(path);
		yield return www;
		try {
			this.clip = www.GetAudioClip(false, false, AudioType.WAV);
			ExternalConsole.Log("Song Size (MB)", Mathf.Round((float)www.bytesDownloaded / 1000000f).ToString());
            this.controller.clip = this.clip;
            this.controller.Play();
            this.waitingForSong = false;
			www.Dispose();
			yield break;
		} catch(Exception e) {
			ExternalConsole.Log("Loading Asset Failed", e.ToString()); 
		}
	}

    public void GenerateRandomQueue()
    {
        this.queue = new List<ExternalAsset>(this.assets);
        this.queue.Shuffle();
    }

    public void PlayNextInQueue() 
    {
        if (this.queue.Count > 0) {
		    base.StartCoroutine(this.LoadAudioFile(this.queue.First().GetRectifiedPath()));
            this.queue.RemoveAt(0);
        } else {
            this.GenerateRandomQueue();
            this.PlayNextInQueue();
        }
    }

    public IEnumerator PlayNextInQueueWithDelay()
    {
        yield return new WaitForSeconds(10);
        PlayNextInQueue();
        yield break;
    }

	public static ExternalMusicLoader instance;
    public List<ExternalAsset> queue;
    public List<ExternalAsset> assets;
	public AudioSource controller;
	public AudioClip clip;
    public bool playLoop;
    public bool waitingForNext;
    public bool waitingForSong;
}
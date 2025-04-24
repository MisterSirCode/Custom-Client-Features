using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CodeStage.AntiCheat.ObscuredTypes;
using Items;
using Spine;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
public class Player : ReplaceableSingleton<Player>
{
	// Token: 0x06003666 RID: 13926
	public Player()
	{
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06003667 RID: 13927
	public GlobalSoundSource soundSource
	{
		get
		{
			return Singleton<AudioManager>.main.soundSource;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x06003668 RID: 13928
	public Vector3 velocity
	{
		get
		{
			return this._velocity;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06003669 RID: 13929
	public float lastTeleportedAt
	{
		get
		{
			return this._lastTeleportedAt;
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x0600366A RID: 13930
	// (set) Token: 0x0600366B RID: 13931
	public ObscuredBool suppressed
	{
		get
		{
			return this._suppressed;
		}
		set
		{
			if (this._suppressed != value)
			{
				this._suppressed = value;
				Messenger.Broadcast<bool>("flightSuppressionChanged", value);
			}
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x0600366C RID: 13932
	public float lastUsedToolAt
	{
		get
		{
			return this._lastUsedToolAt;
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x0600366D RID: 13933
	// (set) Token: 0x0600366E RID: 13934
	public float lastMinedProtectedAt { get; private set; }

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x0600366F RID: 13935
	// (set) Token: 0x06003670 RID: 13936
	public ObscuredBool awesomeMode
	{
		get
		{
			return this._awesomeMode;
		}
		set
		{
			if (this.admin)
			{
				this._awesomeMode = value;
				return;
			}
			this._awesomeMode = false;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06003671 RID: 13937
	public ObscuredBool clip
	{
		get
		{
			return this._clip;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06003672 RID: 13938
	public Item primaryItem
	{
		get
		{
			return this._primaryItem;
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06003673 RID: 13939
	public bool socialInfoProcessed
	{
		get
		{
			return this._socialInfoProcessed;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06003674 RID: 13940
	// (set) Token: 0x06003675 RID: 13941
	public new string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
			Messenger.Broadcast<string>("playerNameChanged", value);
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06003676 RID: 13942
	// (set) Token: 0x06003677 RID: 13943
	public int level
	{
		get
		{
			return this._level;
		}
		set
		{
			this._level = value;
			this.xpForCurrentLevel = this.XpForLevel(this.level);
			this.xpForNextLevel = this.XpForLevel(this.level + 1);
			Messenger.Broadcast<int>("playerLevelChanged", this.level);
			Messenger.Broadcast<int, int>("playerXpChanged", this.xp, this.xpForNextLevel);
		}
	}

	// Token: 0x06003678 RID: 13944
	public void LevelUp(int newLevel)
	{
		this.level = newLevel;
		Notification.Create("LEVEL " + this.level.ToString(), 21);
		this.currentEmoteAnimation = new PlayerAnimation("cheer", false);
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06003679 RID: 13945
	// (set) Token: 0x0600367A RID: 13946
	public int xp
	{
		get
		{
			return this._xp;
		}
		set
		{
			this._xp = value;
			Messenger.Broadcast<int, int>("playerXpChanged", this.xp, this.xpForNextLevel);
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x0600367B RID: 13947
	// (set) Token: 0x0600367C RID: 13948
	public ObscuredFloat health
	{
		get
		{
			return this._health;
		}
		set
		{
			this._health = Mathf.Clamp(value, 0f, this.MaxHealth());
			Messenger.Broadcast<float>("playerHealthChanged", this._health);
			if (!this.avatar.alive && this._health > 0f)
			{
				this.avatar.Revive(null);
			}
			if (this.avatar.alive && this._health == 0f)
			{
				this.avatar.Die(null);
			}
			if (!this.avatar.alive)
			{
				string text = ((!(this.lastAttackedBy != null) || this.lastAttackedBy.name.Length <= 0) ? (Locale.Text("player.died", null, null, null) + " " + ReplaceableSingleton<GameGui>.main.RespawnMessage()) : (this.lastAttackedBy.name + Locale.Text("player.killed", null, null, null) + ((!GameManager.IsTiny()) ? " " : "\n") + ReplaceableSingleton<GameGui>.main.RespawnMessage()));
				Messenger.Broadcast<string>("deathMessageChanged", text);
				int deaths = this.deaths;
				this.deaths = deaths + 1;
				ReplaceableSingleton<Zone>.main.AddDeathPosition(this.position);
				Messenger.Broadcast("playerDied");
			}
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x0600367D RID: 13949
	// (set) Token: 0x0600367E RID: 13950
	public ObscuredFloat breath
	{
		get
		{
			return this._breath;
		}
		set
		{
			this._breath = value;
			Messenger.Broadcast<float>("playerBreathChanged", this._breath);
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x0600367F RID: 13951
	// (set) Token: 0x06003680 RID: 13952
	public ObscuredFloat freeze
	{
		get
		{
			return this._freeze;
		}
		set
		{
			this._freeze = value;
			Messenger.Broadcast<float>("playerFreezeChanged", this._freeze);
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x06003681 RID: 13953
	// (set) Token: 0x06003682 RID: 13954
	public ObscuredFloat thirst
	{
		get
		{
			return this._thirst;
		}
		set
		{
			this._thirst = value;
			Messenger.Broadcast<float>("playerThirstChanged", this._thirst);
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06003683 RID: 13955
	// (set) Token: 0x06003684 RID: 13956
	public ObscuredFloat steam
	{
		get
		{
			return this._steam;
		}
		set
		{
			this._steam = Mathf.Clamp(value, 0f, this.MaxSteam());
			Messenger.Broadcast<float>("playerSteamChanged", this._steam);
			if (this._steam == 0f)
			{
				this.cooldownStartedAt = Time.time;
				Messenger.Broadcast<float>("playerCooldownStarted", this.CooldownTime());
			}
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06003685 RID: 13957
	// (set) Token: 0x06003686 RID: 13958
	public int playTime
	{
		get
		{
			return this._playTime;
		}
		set
		{
			this._playTime = value;
			Messenger.Broadcast<int>("playerPlayTimeChanged", value);
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06003687 RID: 13959
	// (set) Token: 0x06003688 RID: 13960
	public int itemsMined
	{
		get
		{
			return this._itemsMined;
		}
		set
		{
			this._itemsMined = value;
			Messenger.Broadcast<int>("playerItemsMinedChanged", value);
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06003689 RID: 13961
	// (set) Token: 0x0600368A RID: 13962
	public int itemsPlaced
	{
		get
		{
			return this._itemsPlaced;
		}
		set
		{
			this._itemsPlaced = value;
			Messenger.Broadcast<int>("playerItemsPlacedChanged", value);
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x0600368B RID: 13963
	// (set) Token: 0x0600368C RID: 13964
	public int itemsCrafted
	{
		get
		{
			return this._itemsCrafted;
		}
		set
		{
			this._itemsCrafted = value;
			Messenger.Broadcast<int>("playerItemsCraftedChanged", value);
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x0600368D RID: 13965
	// (set) Token: 0x0600368E RID: 13966
	public int deaths
	{
		get
		{
			return this._deaths;
		}
		set
		{
			this._deaths = value;
			Messenger.Broadcast<int>("playerDeathsChanged", value);
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x0600368F RID: 13967
	// (set) Token: 0x06003690 RID: 13968
	public bool premium
	{
		get
		{
			return this._premium;
		}
		set
		{
			this._premium = value;
			Messenger.Broadcast<bool>("playerPremiumChanged", value);
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06003691 RID: 13969
	// (set) Token: 0x06003692 RID: 13970
	public int crowns
	{
		get
		{
			return this._crowns;
		}
		set
		{
			this._crowns = value;
			Messenger.Broadcast<int>("playerCrownsChanged", value);
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06003693 RID: 13971
	// (set) Token: 0x06003694 RID: 13972
	public int skillPoints
	{
		get
		{
			return this._skillPoints;
		}
		set
		{
			this._skillPoints = value;
			Messenger.Broadcast<int>("playerSkillPointsChanged", value);
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06003695 RID: 13973
	// (set) Token: 0x06003696 RID: 13974
	public ObscuredVector2 position
	{
		get
		{
			return this._position;
		}
		set
		{
			if (base.enabled)
			{
				this._position = value;
				this.avatarTransform.position = new Vector2(value.x, -value.y);
			}
		}
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06003697 RID: 13975
	public Vector2 worldPosition
	{
		get
		{
			return new Vector2(this._position.x, -this._position.y);
		}
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06003698 RID: 13976
	public Vector2 worldCenter
	{
		get
		{
			return this.worldPosition - new Vector2(0f, 1f);
		}
	}

	// Token: 0x06003699 RID: 13977
	private void Start()
	{
		this.inventory = new Inventory();
		this.skills = new Dictionary<string, int>();
		this.skillBonuses = new Dictionary<string, int>();
		this.achievements = new List<string>();
		this.achievementProgress = new Dictionary<string, int>();
		this.followees = new Dictionary<string, string>();
		this.followers = new Dictionary<string, string>();
		Messenger.AddListener<Item>("activeHotbarItemChanged", new Callback<Item>(this.SetPrimaryItem));
		Messenger.AddListener<Item>("primaryItemChanged", new Callback<Item>(this.OnPrimaryItemChanged));
		Messenger.AddListener<object>("socialInfoReady", new Callback<object>(this.OnSocialInfoReady));
		Messenger.AddListener<List<Item>>("accessoriesChanged", new Callback<List<Item>>(this.OnAccessoriesChanged));
		Messenger.AddListener("shieldOn", new Callback(this.OnShieldOn));
		Messenger.AddListener("shieldOff", new Callback(this.OnShieldOff));
		Messenger.AddListener("shieldToggle", new Callback(this.OnShieldToggle));
		this.foregroundLayerMask = 1 << LayerMask.NameToLayer("Foreground");
		this.groundedLayerMask = this.foregroundLayerMask;
		this.entityLayerMask = 1 << LayerMask.NameToLayer("Entities");
		this.SetControllerPlatformMask();
		base.gameObject.AddComponent(typeof(HintManager));
	}

	// Token: 0x0600369A RID: 13978
	public void Ready()
	{
		Messenger.Broadcast("playerReady");
		this.steam = this.MaxSteam();
		this.suppressed = false;
		this.targetEntities.Clear();
		this.SetLightColor(Color.black);
		Messenger.Broadcast<int>("activeHotbarSlotChanged", 0);
		ReplaceableSingleton<CameraManager>.main.UpdateCameraPositionForPlayer();
		base.StartCoroutine(this.CheckAutoUseBlocks());
		base.StartCoroutine(this.CheckForSuppressors());
	}

	// Token: 0x0600369B RID: 13979
	private void SetupAvatar(Dictionary<string, object> appearance)
	{
		GameObject gameObject = new GameObject("Entity");
		this.avatar = (EntityAvatar)gameObject.AddComponent(typeof(EntityAvatar));
		this.avatar.transform.parent = this.avatarTransform;
		this.avatar.transform.localPosition = new Vector3(0f, 0f, Entity.ZIndex);
		this.avatar.gameObject.layer = LayerMask.NameToLayer("Player");
		this.avatar.AddSkeleton("player").localPosition = new Vector3(0f, -0.1f, 0f);
		this.avatar.isPlayer = true;
		this.avatar.rotationSpeed = 3f;
		this.avatar.name = this._name;
		this.avatar.AddShield(true);
		this.avatar.Change(appearance);
	}

	// Token: 0x0600369C RID: 13980
	public void Preconfigure(int entityId, Dictionary<string, object> config)
	{
		this.entityId = entityId;
		this.admin = (bool)config.Get("admin", false);
	}

	// Token: 0x0600369D RID: 13981
	public void Configure(Dictionary<string, object> config)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		this.documentId = config.GetString("id");
		this.apiToken = config.GetString("api_token");
		this.name = config.GetString("name");
		base.enabled = true;
		this.xp = config.GetInt("xp", 0);
		this.level = Mathf.Max(config.GetInt("level", 0), 1);
		this.playTime = config.GetInt("play_time", 0);
		this.itemsMined = config.GetInt("items_mined", 0);
		this.itemsPlaced = config.GetInt("items_placed", 0);
		this.itemsCrafted = config.GetInt("items_crafted", 0);
		this.deaths = config.GetInt("deaths", 0);
		this.premium = config.GetBool("premium", false);
		this.karma = config.GetString("karma");
		this.karmaIsPoor = !Regex.IsMatch(this.karma, "Godly|Angelic|Great|Good|Neutral|Fair");
		this.crowns = config.GetInt("crowns", 0);
		this.skillPoints = config.GetInt("points", 0);
		this.settings = config.GetDictionary("settings");
		Config.PlayerSettings.quality = this.settings.GetInt("quality", 0);
		Config.PlayerSettings.LoadHotbarPresets((List<object>)this.settings.Get("hotbar_presets"));
		this.mutings = config.GetDictionary("mutings");
		if (this.mutings != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in this.mutings)
			{
				Messenger.Broadcast<string>("playerMuted", keyValuePair.Key);
			}
		}
		this.entityConfig = Config.main.EntityByCode(0);
		Messenger.Broadcast<int>("playerVisibilityChanged", this.settings.GetInt("visibility", 0));
		this.avatarBoxCollider = this.avatarTransform.gameObject.GetComponent<BoxCollider2D>();
		if (this.avatar == null)
		{
			this.SetupAvatar(config.GetDictionary("appearance"));
		}
		stopwatch.Stop();
	}

	// Token: 0x0600369E RID: 13982
	public object GetSetting(string store, string key, Type type)
	{
		if (store == "prefs")
		{
			if (type == typeof(int))
			{
				int @int = PlayerPrefs.GetInt(key, -1);
				if (@int != -1)
				{
					return @int;
				}
			}
			else if (type == typeof(float))
			{
				float @float = PlayerPrefs.GetFloat(key, -1f);
				if (@float != -1f)
				{
					return @float;
				}
			}
			else if (type == typeof(string))
			{
				return PlayerPrefs.GetString(key);
			}
		}
		if (store == "account")
		{
			return this.settings.Get(key);
		}
		return null;
	}

	// Token: 0x0600369F RID: 13983
	public void UpdateSetting(string store, string key, Type type, object value)
	{
		if (value.GetType() == typeof(bool))
		{
			value = (((bool)value) ? 1 : 0);
		}
		if (store == "prefs")
		{
			if (type == typeof(int))
			{
				PlayerPrefs.SetInt(key, Convert.ToInt32(value));
			}
			if (type == typeof(float))
			{
				PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
			}
			if (type == typeof(string))
			{
				PlayerPrefs.SetString(key, Convert.ToString(value));
			}
		}
		else if (store == "account")
		{
			this.settings[key] = value;
			Command.Send(Command.Identity.Setting, new object[] { key, value });
		}
		Messenger.Broadcast<string, object>("settingChanged", key, value);
	}

	// Token: 0x060036A0 RID: 13984
	public void ToggleAwesome()
	{
		this.awesomeMode = !this.awesomeMode;
		this.avatar.SetAura(this.awesomeMode);
		Messenger.Broadcast<bool>("awesomeModeChanged", this.awesomeMode);
	}

	// Token: 0x060036A1 RID: 13985
	public void ToggleClipping()
	{
		if (this.admin)
		{
			this._clip = !this._clip;
			this.SetControllerPlatformMask();
		}
	}

	// Token: 0x060036A2 RID: 13986
	private void SetControllerPlatformMask()
	{
		this.avatarController2D.platformMask = ((!this._clip) ? 0 : this.groundedLayerMask);
		this.avatarController2D.avoidOverlapMask = ((!this._clip) ? 0 : this.groundedLayerMask);
	}

	// Token: 0x060036A3 RID: 13987
	public void Step()
	{
		this.canReachBlockCache.Clear();
		this._position = new Vector2(this.avatarTransform.position.x, -this.avatarTransform.position.y);
		if (Time.time > this.nextMovementCommandTime || (int)this._position.x != this.lastSentMovementPosition.x || (int)this._position.y != this.lastSentMovementPosition.y)
		{
			this.SendMovementCommand();
		}
		this.CheckSubmersion();
		this.CheckGrounding();
		this.StepMovement();
		this.ExploreChunk();
		this.avatar.Step();
		if (this.CurrentBlock() != null)
		{
			this.StepSubmersion(this.CurrentBlock().liquidItem, this.wasSubmerged);
		}
		if (this.avatar.alive)
		{
			PlayerAnimation playerAnimation = this.CurrentAnimation();
			if (playerAnimation != null)
			{
				this.avatar.Animate(playerAnimation.name, playerAnimation.loop, null);
				this.animationIndex = this.entityConfig.AnimationIndex(playerAnimation.name);
			}
		}
		this.SnapToGrid();
		this.ClampVelocity();
		this.StepTargeting();
		this.StepTools();
		this.SendStatus();
		this.UseNearbyItems();
		this.RecoverSteam();
		Messenger.Broadcast<string>("zonePositionChanged", ReplaceableSingleton<Zone>.main.PositionDescription(this.position));
		if (Time.frameCount % 600 == 0)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "s", this.steam },
				{
					"sm",
					this.MaxSteam()
				},
				{ "h", this.health },
				{
					"hm",
					this.MaxHealth()
				},
				{
					"skm",
					this.AdjustedSkill("mining")
				},
				{
					"skb",
					this.AdjustedSkill("building")
				},
				{
					"ske",
					this.AdjustedSkill("engineering")
				},
				{
					"a",
					(!this.awesomeMode) ? "false" : "true"
				},
				{
					"cl",
					(!this.clip) ? "false" : "true"
				},
				{
					"cl2",
					(!this._clip) ? "false" : "true"
				},
				{ "cr", this.crowns },
				{
					"rsp",
					this.MaxHorizontalSpeed() / 8.5f * 1.6f
				},
				{
					"msp",
					this.MiningSpeed() * 0.8f
				},
				{
					"dll",
					(!GameManager.splinesReticulated) ? "false" : "true"
				}
			};
			Command.Send(Command.Identity.Status, new object[] { dictionary });
		}
	}

	// Token: 0x060036A4 RID: 13988
	public void PostStep()
	{
		if (this.lastToolCommandUse == Player.InventoryUse.Start && Time.time > this._lastUsedToolAt + 0.1f)
		{
			this.SendInventoryUseCommand(Player.InventoryUse.End, false);
			this.lastToolCommandUse = Player.InventoryUse.End;
		}
	}

	// Token: 0x060036A5 RID: 13989
	public ZoneBlock CurrentBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x, (int)this.position.y, false);
	}

	// Token: 0x060036A6 RID: 13990
	private ZoneBlock AboveBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x, (int)this.position.y - 1, false);
	}

	// Token: 0x060036A7 RID: 13991
	private ZoneBlock BelowBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x, (int)this.position.y + 1, false);
	}

	// Token: 0x060036A8 RID: 13992
	private ZoneBlock BelowLeftBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x - 1, (int)this.position.y + 1, false);
	}

	// Token: 0x060036A9 RID: 13993
	private ZoneBlock BelowRightBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x + 1, (int)this.position.y + 1, false);
	}

	// Token: 0x060036AA RID: 13994
	private ZoneBlock LeftBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x - 1, (int)this.position.y, false);
	}

	// Token: 0x060036AB RID: 13995
	private ZoneBlock RightBlock()
	{
		return ReplaceableSingleton<Zone>.main.Block((int)this.position.x + 1, (int)this.position.y, false);
	}

	// Token: 0x060036AC RID: 13996
	public void SetPrimaryItem(Item item)
	{
		this._primaryItem = item;
		this.avatar.toolItem = item;
		this.avatar.StopShooting(false);
		Messenger.Broadcast<Item>("primaryItemChanged", item);
	}

	// Token: 0x060036AD RID: 13997
	private void OnPrimaryItemChanged(Item item)
	{
		this.SendInventoryUseCommand(Player.InventoryUse.Select, false);
	}

	// Token: 0x060036AE RID: 13998
	public bool IsPrimaryItemPlaceable()
	{
		return this._primaryItem != null && this._primaryItem.placeable;
	}

	// Token: 0x060036AF RID: 13999
	public bool IsPrimaryItemTool()
	{
		return this._primaryItem != null && this._primaryItem.IsTool();
	}

	// Token: 0x060036B0 RID: 14000
	public bool IsPrimaryItemGun()
	{
		return this._primaryItem != null && this._primaryItem.IsGun();
	}

	// Token: 0x060036B1 RID: 14001
	public bool CanConsume()
	{
		return Time.time > this.canConsumeAt;
	}

	// Token: 0x060036B2 RID: 14002
	public void OnConsume()
	{
		this.canConsumeAt = Time.time + 1f;
	}

	// Token: 0x060036B3 RID: 14003
	private void SendInventoryUseCommand(Player.InventoryUse use, bool onlyIfAllowed)
	{
		if (this.primaryItem == null)
		{
			return;
		}
		if (onlyIfAllowed && this.primaryItem.IsGun() && !this.HasSteam())
		{
			return;
		}
		int num = 0;
		object[] array2;
		if (use != Player.InventoryUse.Start)
		{
			if (use != Player.InventoryUse.End)
			{
				object[] array4 = new object[4];
				array4[0] = num;
				array4[1] = this.primaryItem.code;
				array4[2] = 0;
				array2 = array4;
			}
			else
			{
				object[] array5 = new object[4];
				array5[0] = num;
				array5[1] = this.primaryItem.code;
				array5[2] = 2;
				array2 = array5;
			}
		}
		else
		{
			int[] array3 = this.targetEntities.Keys.ToList<int>().ToArray();
			array2 = new object[]
			{
				num,
				this.primaryItem.code,
				1,
				array3
			};
		}
		Command.Send(Command.Identity.InventoryUse, array2);
		this.lastInventoryUseMessageAt = Time.time;
	}

	// Token: 0x060036B4 RID: 14004
	private float MaxHorizontalSpeed()
	{
		return Mathf.Lerp(6.375f, 8.5f, (float)this.AdjustedSkill("agility") / 10f);
	}

	// Token: 0x060036B5 RID: 14005
	private float ClimbingSpeed()
	{
		return Mathf.Lerp(6.5f, 8.5f, (float)this.AdjustedSkill("agility") / 10f);
	}

	// Token: 0x060036B6 RID: 14006
	private float JumpMaxDuration()
	{
		return 0.25f;
	}

	// Token: 0x060036B7 RID: 14007
	private float JumpMinDuration()
	{
		return 0.021875f;
	}

	// Token: 0x060036B8 RID: 14008
	private IEnumerator CheckAutoUseBlocks()
	{
		int y = -5;
		for (;;)
		{
			if (ReplaceableSingleton<Zone>.main.state == Zone.State.Active)
			{
				ZoneBlock zoneBlock = this.CurrentBlock();
				if (zoneBlock != null)
				{
					for (int i = -5; i < 5; i++)
					{
						ZoneBlock zoneBlock2 = ReplaceableSingleton<Zone>.main.Block(zoneBlock.x + i, zoneBlock.y + y, false);
						if (zoneBlock2 != null)
						{
							Vector2 vector = new Vector2((float)zoneBlock2.x, (float)zoneBlock2.y);
							float magnitude = (vector - new Vector2((float)zoneBlock.x, (float)zoneBlock.y)).magnitude;
							if (magnitude <= 5f && this.CanReachBlock(vector))
							{
								Item frontItem = zoneBlock2.frontItem;
								if (frontItem != null && magnitude < frontItem.useProximity && frontItem.useProximity != 0f)
								{
									zoneBlock2.Use();
								}
							}
						}
					}
				}
			}
			int num = y;
			y = num + 1;
			if (y > 5)
			{
				y = -5;
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield break;
	}

	// Token: 0x060036B9 RID: 14009
	private IEnumerator CheckForSuppressors()
	{
		for (;;)
		{
			bool shouldBeSuppressed = false;
			foreach (KeyValuePair<int, MetaBlock> keyValuePair in ReplaceableSingleton<Zone>.main.meta.effectMetaBlocks)
			{
				MetaBlock value = keyValuePair.Value;
				if (!shouldBeSuppressed && !this.awesomeMode && value.item.IsUsableType(Item.Use.Suppress))
				{
					Vector2 vector = new Vector2((float)value.x, (float)value.y);
					ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block(value.x, value.y, false);
					if (zoneBlock != null && zoneBlock.frontMod > 0)
					{
						shouldBeSuppressed = Vector2.Distance(new Vector2(vector.x, -vector.y), this.avatarTransform.position) < value.item.power;
					}
				}
			}
			this.suppressed = shouldBeSuppressed;
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	// Token: 0x060036BA RID: 14010
	private bool CheckOverlayPercentage(float percentage)
	{
		Vector2 vector = (Vector2)this.avatarTransform.position + this.avatarBoxCollider.offset;
		Vector2 size = this.avatarBoxCollider.size;
		Vector2[] array = new Vector2[]
		{
			new Vector2(vector.x - size.x / 2f, vector.y - size.y / 2f),
			new Vector2(vector.x, vector.y - size.y / 2f),
			new Vector2(vector.x + size.x / 2f, vector.y - size.y / 2f),
			new Vector2(vector.x - size.x / 2f, vector.y),
			new Vector2(vector.x, vector.y),
			new Vector2(vector.x + size.x / 2f, vector.y),
			new Vector2(vector.x - size.x / 2f, vector.y + size.y / 2f),
			new Vector2(vector.x, vector.y + size.y / 2f),
			new Vector2(vector.x + size.x / 2f, vector.y + size.y / 2f)
		};
		int num = 0;
		Vector2[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Collider2D collider2D = Physics2D.OverlapPoint(array2[i], this.foregroundLayerMask);
			if (collider2D != null && collider2D.GetType() != typeof(PolygonCollider2D))
			{
				num++;
			}
			if ((float)(num / 9) >= percentage)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060036BB RID: 14011
	private void CheckSubmersion()
	{
		this.wasSubmerged = this.submerged;
		this.currentLiquidLevel = (int)(((this.CurrentBlock() == null) ? 0 : this.CurrentBlock().liquidMod) + ((this.AboveBlock() == null) ? 0 : this.AboveBlock().liquidMod));
		this.submerged = this.currentLiquidLevel > 4;
	}

	// Token: 0x060036BC RID: 14012
	private void CheckGrounding()
	{
		this.grounded = true;
		this.partiallyGrounded = false;
		this.collidingBlock = null;
		float num = 0.2f;
		float num2 = 6f;
		int num3 = 0;
		while ((float)num3 < num2)
		{
			float num4 = Mathf.Lerp(-num, num, (float)num3 / (num2 - 1f));
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.avatarTransform.position + new Vector3(this.avatarBoxCollider.offset.x, this.avatarBoxCollider.offset.y, 0f) + new Vector3(num4, 0f, 0f), new Vector3(0f, -1f, 0f), this.avatarBoxCollider.size.y * 0.5f + 0.1f, this.groundedLayerMask);
			if (raycastHit2D.collider != null)
			{
				BlockCollider blockCollider = raycastHit2D.transform.GetComponent<BlockCollider>();
				if (blockCollider == null)
				{
					blockCollider = raycastHit2D.collider.gameObject.GetComponentInParent<BlockCollider>();
				}
				if (blockCollider != null)
				{
					Zone main = ReplaceableSingleton<Zone>.main;
					ZoneBlock zoneBlock = main.Block(blockCollider.blockIndex % main.blockSize.width, blockCollider.blockIndex / main.blockSize.width, false);
					this.collidingBlock = zoneBlock;
				}
				this.partiallyGrounded = true;
			}
			else
			{
				this.grounded = false;
			}
			num3++;
		}
	}

	// Token: 0x060036BD RID: 14013
	public bool IsGrounded()
	{
		return this._clip && this.grounded;
	}

	// Token: 0x060036BE RID: 14014
	private void StepMovement()
	{
		if (this.isTeleporting)
		{
			return;
		}
		if (Time.time > this.lastMovedAt + 0.5f)
		{
			this.inputVelocity = Vector2.Lerp(this.inputVelocity, Vector2.zero, Time.deltaTime * 5f);
		}
		if (Time.time < this.lastJumpedAt + this.JumpMinDuration())
		{
			this.inputVelocity.y = 1f;
		}
		bool flag = this.avatar.currentAnimationName != "walk" && this.avatar.currentAnimationName != "run";
		bool flag2 = this.movement == Player.Movement.Hovering || this.movement == Player.Movement.Flying || this.movement == Player.Movement.Falling;
		bool flag3 = this.IsAlive() && Mathf.Abs(this.inputVelocity.x) > 0.1f;
		bool flag4 = this.IsAlive() && this.inputVelocity.y > ((!this.IsGrounded()) ? 0.1f : 0.5f);
		bool flag5 = this.IsAlive() && this.inputVelocity.y < -0.2f;
		bool flag6 = this.IsAlive() && this.flyAccessory != null && this.HasSteam() && !this.suppressed && flag4;
		bool flag7 = this.IsAlive() && this.HasSteam() && this.stompAccessory != null && Time.time > this.lastStompedAt + this.stompAccessory.AttackInterval() && flag5;
		Player.Movement movement = this.movement;
		this.movement = Player.Movement.Idle;
		this.rotation = 0;
		if (this.collidingBlock == null)
		{
			this.externalVelocity.x = Mathf.Lerp(this.externalVelocity.x, 0f, Time.deltaTime);
		}
		else if (!this.collidingBlock.frontItem.IsUsableType(Item.Use.Move))
		{
			this.externalVelocity.x = 0f;
		}
		if (this.CheckOverlayPercentage(0.5f))
		{
			if (!this.movementLocked)
			{
				this.movementLocked = true;
				Messenger.Broadcast("playerMovementLocked");
			}
			if (this._clip)
			{
				return;
			}
		}
		else if (this.movementLocked)
		{
			this.movementLocked = false;
			Messenger.Broadcast("playerMovementUnlocked");
		}
		if (this.submerged)
		{
			float num = 0.333f;
			if (!flag3 && !flag4 && !flag5)
			{
				this.Move(0f, 0f, 0f, num);
				return;
			}
			if (!flag6 || !this.flyAccessory.IsUsableType(Item.Use.Propel))
			{
				this.movement = Player.Movement.Swimming;
				this.Move(0.333f, 0.333f, 0.333f, num);
				return;
			}
			this.movement = Player.Movement.Propelling;
			if (this.UseFlyAccessory())
			{
				this.Move(0.667f, 0.667f, 0.667f, num);
				return;
			}
		}
		else
		{
			if (this.CanClimb() && !this.IsGrounded() && this.inputVelocity.y > 0f)
			{
				this.movement = Player.Movement.Climbing;
				this.Move(0.5f, 0.7f, 0f, 1f);
				return;
			}
			if (Time.time < this.lastJumpedAt + this.JumpMaxDuration())
			{
				this.movement = Player.Movement.Jumping;
				this.Move(1f, 1f, 0f, 1f);
				return;
			}
			if (this.IsGrounded())
			{
				this.avatar.WalkOn(this.BelowBlock(), flag2);
				if (movement == Player.Movement.Stomping)
				{
					this.StompComplete();
				}
				if (movement == Player.Movement.Flailing)
				{
					this.currentEmoteAnimation = new PlayerAnimation("land", false);
				}
				if (flag4 && Time.time > this.lastJumpedAt)
				{
					this.movement = Player.Movement.Jumping;
					this.lastJumpedAt = (this.lastPropelledUpwardAt = Time.time);
					this.Move(1f, 1f, 0f, 1f);
					return;
				}
				if (flag3)
				{
					if (flag)
					{
						this.startedRunningAt = Time.time;
					}
					this.movement = ((Mathf.Abs(this._velocity.x) <= 4f || Time.time <= this.startedRunningAt + Player.walkToRunDelay) ? Player.Movement.Walking : Player.Movement.Running);
					this.Move((this.movement != Player.Movement.Running) ? 0.8f : 1f, 0f, 0f, 1f);
				}
				else
				{
					this._velocity.x = this._velocity.x * 0.6f;
					this.Move(0f, 0f, 0f, 1f);
				}
				if (this.collidingBlock != null && this.collidingBlock.frontItem.IsUsableType(Item.Use.Move))
				{
					int num2 = ((this.collidingBlock.frontMod == 0) ? 1 : (-1));
					this.externalVelocity.x = Mathf.Lerp(this.externalVelocity.x, (float)num2 * 5f, Time.deltaTime * 5f);
				}
				return;
			}
			else if (flag6 && this.flyAccessory.IsUsableType(Item.Use.Fly))
			{
				this.movement = Player.Movement.Flying;
				this.rotation = (int)Mathf.Round(30f * this._velocity.x / this.MaxHorizontalSpeed());
				this.lastPropelledUpwardAt = Time.time;
				if (this.UseFlyAccessory())
				{
					this.Move(1f, 0.75f, 0f, 1f);
					return;
				}
			}
			else
			{
				if (flag7)
				{
					this.movement = Player.Movement.Stomping;
					this.Move(0.3f, 0f, 1f, 1f);
					this.UseStompAccessory();
					return;
				}
				if (this._velocity.y == 0f)
				{
					this.movement = Player.Movement.Balancing;
					if (!flag3)
					{
						ZoneBlock zoneBlock = this.BelowBlock();
						if (zoneBlock != null && !zoneBlock.IsObstacle())
						{
							ZoneBlock zoneBlock2 = this.LeftBlock();
							ZoneBlock zoneBlock3 = this.RightBlock();
							if (zoneBlock2 != null && !zoneBlock2.IsObstacle() && zoneBlock3 != null && !zoneBlock3.IsObstacle())
							{
								float num3 = ((Mathf.Repeat(this.position.x, 1f) <= 0.5f) ? 1f : (-1f));
								this._velocity.x = 0.6f * num3;
							}
						}
					}
				}
				else if (Time.time > this.lastGroundedAt + 3f && Time.time > this.lastPropelledUpwardAt + 3.333f && this.flyAccessory != null && !this.flyAccessory.IsUsableType(Item.Use.Hover) && this.currentLiquidLevel <= 4)
				{
					this.movement = Player.Movement.Flailing;
				}
				else
				{
					this.movement = Player.Movement.Falling;
				}
				this.Move(1f, 0f, 0f, 1f);
			}
		}
	}

	// Token: 0x060036BF RID: 14015
	private void Move(float horizontalPower = 0f, float upPower = 0f, float downPower = 0f, float gravity = 1f)
	{
		bool flag = this.IsAlive() && Mathf.Abs(this.inputVelocity.x) > 0.1f;
		bool flag2 = this.IsAlive() && Mathf.Abs(this.inputVelocity.y) > 0.1f;
		Vector2 vector = this._velocity;
		Vector2 vector2 = new Vector2(this.inputVelocity.x * horizontalPower, this.inputVelocity.y * ((this.inputVelocity.y <= 0f) ? downPower : upPower));
		if (flag)
		{
			Mathf.Sign(vector2.x);
			Mathf.Sign(this._velocity.x);
		}
		if (this.movement == Player.Movement.Stomping && flag2 && Mathf.Sign(vector2.y) != Mathf.Sign(this._velocity.y))
		{
			this._velocity.y = 0f;
		}
		if (this.IsGrounded())
		{
			this._velocity.y = 0f;
		}
		else
		{
			this._velocity += Physics.gravity * Time.deltaTime * gravity * 1.5f;
			float num = ((this.movement != Player.Movement.Stomping) ? (-12.5f) : (-14.5f)) * gravity;
			if (this._velocity.y < num)
			{
				this._velocity.y = Mathf.Lerp(this._velocity.y, num, Time.deltaTime * 50f);
			}
		}
		if (this._velocity.x > -this.MaxHorizontalSpeed() && this._velocity.x < this.MaxHorizontalSpeed())
		{
			this._velocity.x = Mathf.Lerp(this._velocity.x, vector2.x * this.MaxHorizontalSpeed(), Time.deltaTime * 20f);
		}
		else
		{
			this._velocity.x = Mathf.Lerp(this._velocity.x, 0f, Time.deltaTime * 5f);
		}
		if (this._velocity.y > -14.5f && this._velocity.y < 12f)
		{
			float num2 = this.JumpMaxDuration();
			if (Time.time < this.lastJumpedAt + num2)
			{
				if (flag2)
				{
					float num3 = Mathf.Lerp(0.3f, 0.8f, this.SkillLerp("agility"));
					this._velocity.y = 12f * num3;
				}
			}
			else
			{
				this._velocity.y = Mathf.Clamp(this._velocity.y + vector2.y * 12f * Time.deltaTime * 3f, -14.5f, 12f);
				if (this.movement == Player.Movement.Climbing)
				{
					this._velocity.y = Mathf.Min(this._velocity.y, this.ClimbingSpeed());
				}
			}
		}
		if (this.movementDamping > 0f)
		{
			this.movementDamping = Mathf.Clamp(this.movementDamping, 0f, 0.9f);
			this._velocity *= 1f - this.movementDamping;
			this.movementDamping -= Time.deltaTime;
		}
		if (flag2)
		{
			if (this.movement == Player.Movement.Flying && !flag)
			{
				ZoneBlock zoneBlock = this.AboveBlock();
				if (zoneBlock != null && !zoneBlock.IsObstacle())
				{
					this._velocity.x = ((float)zoneBlock.x + 0.5f - this._position.x) * 1.5f;
				}
			}
			ZoneBlock zoneBlock2 = this.BelowBlock();
			if (this.movement == Player.Movement.Stomping && !flag)
			{
				this.MoveTowardsChute(zoneBlock2);
			}
			if ((this.movement == Player.Movement.Falling || this.IsGrounded()) && this.inputVelocity.y < 0f && Mathf.Abs(this.inputVelocity.x) < 0.4f && !this.MoveTowardsChute(zoneBlock2))
			{
				ZoneBlock zoneBlock3 = ((!this.IsLookingRight()) ? this.BelowLeftBlock() : this.BelowRightBlock());
				this.MoveTowardsChute(zoneBlock3);
			}
		}
		if (!this.partiallyGrounded && !flag && !flag2)
		{
			this._velocity.x = vector.x;
		}
		this.ClampVelocityToWorldBounds();
		this.MoveAvatar();
		this.RotateAvatar();
	}

	// Token: 0x060036C0 RID: 14016
	private bool MoveTowardsChute(ZoneBlock block)
	{
		if (block != null && !block.IsObstacle())
		{
			ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block(block.x - 1, block.y, false);
			if (zoneBlock == null || zoneBlock.IsObstacle())
			{
				ZoneBlock zoneBlock2 = ReplaceableSingleton<Zone>.main.Block(block.x - 1, block.y, false);
				if (zoneBlock == null || zoneBlock2.IsObstacle())
				{
					this._velocity.x = ((float)block.x + 0.5f - this._position.x) * 7.5f;
					this._velocity.x = this._velocity.x + Mathf.Sign(this._velocity.x) * 1f;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060036C1 RID: 14017
	private void ClampVelocityToWorldBounds()
	{
		Vector3 vector = this.avatarTransform.position + this._velocity * Time.deltaTime;
		if (vector.x < this.avatarTransform.localScale.x / 2f || vector.x > (float)ReplaceableSingleton<Zone>.main.blockSize.width - this.avatarTransform.localScale.x / 2f)
		{
			this._velocity.x = 0f;
		}
		if (vector.y > -this.avatarTransform.localScale.y || vector.y < -(float)ReplaceableSingleton<Zone>.main.blockSize.height + this.avatarTransform.localScale.y)
		{
			this._velocity.y = 0f;
		}
	}

	// Token: 0x060036C2 RID: 14018
	private void BoostMovement(Vector2 power)
	{
		this._velocity.x = this._velocity.x + power.x;
		this._velocity.y = this._velocity.y + power.y;
		this.MoveAvatar();
	}

	// Token: 0x060036C3 RID: 14019
	private void MoveAvatar()
	{
		Vector3 position = this.avatarTransform.position;
		this._velocity.z = 0f;
		this.avatarController2D.move((this._velocity + this.externalVelocity) * Time.deltaTime);
		this._velocity = (this.avatarTransform.position - position) / Time.deltaTime - this.externalVelocity;
		if (this.IsGrounded())
		{
			this.lastGroundedAt = Time.time;
		}
	}

	// Token: 0x060036C4 RID: 14020
	private void RotateAvatar()
	{
		this.avatar.rotation = this.rotation;
	}

	// Token: 0x060036C5 RID: 14021
	private bool CanClimb()
	{
		return this.CurrentBlock() != null && (this.CanClimb(this.CurrentBlock()) || this.CanClimb(this.CurrentBlock().Top()));
	}

	// Token: 0x060036C6 RID: 14022
	private bool CanClimb(ZoneBlock block)
	{
		return block != null && block.frontItem != null && block.frontItem.IsUsableType(Item.Use.Climb);
	}

	// Token: 0x060036C7 RID: 14023
	private void SnapToGrid()
	{
	}

	// Token: 0x060036C8 RID: 14024
	private void ClampVelocity()
	{
	}

	// Token: 0x060036C9 RID: 14025
	private bool UseFlyAccessory()
	{
		if (this.suppressed)
		{
			return false;
		}
		this.flyAccessoryPower = Mathf.Lerp(this.flyAccessoryPower, 1f, Time.deltaTime * 2f);
		this.UseSteam(this.flyAccessory.rate, true, false);
		this.NullifyGravity(true, 3f);
		return true;
	}

	// Token: 0x060036CA RID: 14026
	private void NullifyGravity(bool up, float rate)
	{
		if ((up && this.inputVelocity.y > 0f && this._velocity.y < 0f) || (!up && this.inputVelocity.y < 0f && this._velocity.y > 0f))
		{
			this._velocity.y = Mathf.Lerp(this._velocity.y, 0f, Time.deltaTime * rate);
		}
	}

	// Token: 0x060036CB RID: 14027
	private bool UseStompAccessory()
	{
		this.UseSteam(this.stompAccessory.rate, true, false);
		return true;
	}

	// Token: 0x060036CC RID: 14028
	public void InputMovement(float x, float y)
	{
		if (this.avatar.alive)
		{
			this.inputVelocity.x = x;
			this.inputVelocity.y = y;
			if (Mathf.Abs(x) > 0.1f)
			{
				this.avatar.lookDirection = ((x <= 0f) ? Entity.LookDirection.Left : Entity.LookDirection.Right);
			}
			if (x != 0f || y != 0f)
			{
				this.lastMovedAt = Time.time;
			}
		}
	}

	// Token: 0x060036CD RID: 14029
	public void SendMovementCommand()
	{
		int num = (int)(this._position.x * Entity.PositionModifier);
		int num2 = (int)((this._position.y - 0.8f) * Entity.PositionModifier);
		int num3 = (int)(this._velocity.x * Entity.PositionModifier);
		int num4 = (int)(this._velocity.y * Entity.PositionModifier);
		int num5 = (int)(this._target.x * Entity.PositionModifier);
		int num6 = (int)(this._target.y * Entity.PositionModifier);
		Command.Send(Command.Identity.PlayerPosition, new object[]
		{
			num,
			num2,
			num3,
			num4,
			(!this.IsLookingLeft()) ? 1 : (-1),
			num5,
			num6,
			this.animationIndex
		});
		this.nextMovementCommandTime = Time.time + 0.175f;
		this.lastSentMovementPosition.x = (int)this._position.x;
		this.lastSentMovementPosition.y = (int)this._position.y;
	}

	// Token: 0x060036CE RID: 14030
	private void StepTools()
	{
		if (this.activeShield != null)
		{
			this.UseSteam(Mathf.Max(this.activeShield.rate, 1f), true, false);
			if (this.steam <= 0f)
			{
				this.ToggleShield(false);
			}
		}
		if (this.wasShooting && Time.time > this.lastWasShootingAt + 0.2f)
		{
			this.StopShooting();
		}
	}

	// Token: 0x060036CF RID: 14031
	private void StompEntity(Entity entity)
	{
		this.SendMovementCommand();
		Command.Send(Command.Identity.InventoryUse, new object[]
		{
			1,
			this.stompAccessory.code,
			1,
			entity.entityId
		});
		this.StompComplete();
	}

	// Token: 0x060036D0 RID: 14032
	private void StompComplete()
	{
		this.lastStompedAt = Time.time;
		this._velocity.y = this._velocity.y * 0.333f;
		Effect effect = Config.main.EffectByName("shadow steam");
		if (effect != null)
		{
			effect.Spawn(this.position.x, -this.position.y, global::UnityEngine.Random.Range(8, 12));
		}
	}

	// Token: 0x060036D1 RID: 14033
	public void UseConsumableTeleporter(int x, int y)
	{
		int code = Item.Get("consumables/teleporter").code;
		Command.Send(Command.Identity.InventoryUse, new object[]
		{
			1,
			code,
			1,
			new int[] { x, y }
		});
		this.inventory.Remove(code, 1);
		this.BeginTeleportation();
	}

	// Token: 0x060036D2 RID: 14034
	public void TeleportTo(int x, int y)
	{
		this.BeginTeleportation();
		this.position = new Vector2((float)x, (float)y);
		this._lastTeleportedAt = Time.time;
	}

	// Token: 0x060036D3 RID: 14035
	public void BeginTeleportation()
	{
		Singleton<SnapshotManager>.main.Snapshot(SnapshotManager.Type.Teleport, null);
		this.isTeleporting = true;
	}

	// Token: 0x060036D4 RID: 14036
	public void CompleteTeleportation()
	{
		Singleton<SnapshotManager>.main.Hide();
		this.isTeleporting = false;
	}

	// Token: 0x060036D5 RID: 14037
	private void ExploreChunk()
	{
		ZoneBlock zoneBlock = this.CurrentBlock();
		if (zoneBlock != null)
		{
			int num = ReplaceableSingleton<Zone>.main.ChunkIndex(zoneBlock.x, zoneBlock.y);
			ReplaceableSingleton<Zone>.main.ExploreChunk(num);
		}
	}

	// Token: 0x060036D6 RID: 14038
	public void OnCollideWithEntity(Entity entity)
	{
		if (this.movement == Player.Movement.Stomping)
		{
			this.StompEntity(entity);
			return;
		}
		if (entity.config.damageAmount > 0f && entity.alive)
		{
			this.Damage(entity.config.damageType, entity.config.damageAmount, null);
			if (entity.config.obstacle > 0f)
			{
				this.movementDamping = entity.config.obstacle;
			}
		}
		if (entity is EntityBullet)
		{
			entity.rigidbodyVelocity *= 0.05f;
		}
	}

	// Token: 0x060036D7 RID: 14039
	public void OnCollideWithEffect(Effect effect)
	{
		if (this.activeShield == null || !this.avatar.shield.DefendsFrom(effect.damageType))
		{
			this.EnvironmentalDamage(effect.damageType, effect.damage);
		}
	}

	// Token: 0x060036D8 RID: 14040
	private void OnAccessoriesChanged(List<Item> accessories)
	{
		this.flyAccessory = null;
		this.stompAccessory = null;
		float num = 6f;
		foreach (Item item in this.inventory.accessories)
		{
			if (this.flyAccessory == null && (item.IsUsableType(Item.Use.Fly) || item.IsUsableType(Item.Use.Hover) || item.IsUsableType(Item.Use.Propel)))
			{
				this.flyAccessory = item;
				this.flyAccessoryPower = item.power;
			}
			if (item.light > num)
			{
				num = item.light;
			}
		}
		foreach (Item item2 in this.inventory.hidden)
		{
			if (item2.action == Item.Action.Exoleg)
			{
				this.stompAccessory = item2;
			}
		}
		this.avatar.steampackItem = this.flyAccessory;
		int num2 = ((this.flyAccessory != null) ? this.flyAccessory.code : 0);
		Dictionary<string, object> dictionary = new Dictionary<string, object> { { "u", num2 } };
		this.light.localScale = new Vector3(num * 3f, num * 3f, 1f);
		this.avatar.Change(dictionary);
		foreach (string text in this.skills.Keys)
		{
			this.skillBonuses[text] = this.inventory.SkillBonus(text);
		}
		Messenger.Broadcast<bool>("playerShieldEquipped", this.AccessoryWithAction(Item.Action.Shield) != null);
		this.SkillsChanged();
		this.MaxHealthChanged();
		this.MaxSteamChanged();
	}

	// Token: 0x060036D9 RID: 14041
	public float MaxHealth()
	{
		int num = Mathf.Clamp(this.AdjustedSkill("stamina"), 0, 10);
		return 5f + (float)(num - ((num != 10) ? 1 : 0)) * 0.5f;
	}

	// Token: 0x060036DA RID: 14042
	private void MaxHealthChanged()
	{
		Messenger.Broadcast<float>("playerMaxHealthChanged", this.MaxHealth());
	}

	// Token: 0x060036DB RID: 14043
	public bool IsAlive()
	{
		return this._health > 0f;
	}

	// Token: 0x060036DC RID: 14044
	public float MaxSteam()
	{
		return 20f + (float)this.inventory.SteamBonus();
	}

	// Token: 0x060036DD RID: 14045
	private void MaxSteamChanged()
	{
		Messenger.Broadcast<float>("playerMaxSteamChanged", this.MaxSteam());
	}

	// Token: 0x060036DE RID: 14046
	public bool HasSteam()
	{
		return this._steam > 0f && Time.time - this.cooldownStartedAt > this.CooldownTime();
	}

	// Token: 0x060036DF RID: 14047
	public void UseSteam(float amount, bool perSecond = true, bool isGun = false)
	{
		this.steam -= ((!perSecond) ? 1f : Time.deltaTime) * amount / ((!isGun) ? this.SteamEfficiency() : this.CombatEfficiency());
		this.lastUsedSteamAt = Time.time;
	}

	// Token: 0x060036E0 RID: 14048
	public void DrainSteam(float amount)
	{
		this.steam -= amount;
	}

	// Token: 0x060036E1 RID: 14049
	public void RestoreSteam(float amount)
	{
		this.steam += amount;
		this.lastUsedSteamAt = Time.time;
	}

	// Token: 0x060036E2 RID: 14050
	public float SteamEfficiency()
	{
		return Mathf.Lerp(1f, 1.5f, this.SkillLerp("engineering"));
	}

	// Token: 0x060036E3 RID: 14051
	public float CombatEfficiency()
	{
		return Mathf.Lerp(1f, 1.5f, this.SkillLerp("combat"));
	}

	// Token: 0x060036E4 RID: 14052
	public float SteamRecoveryRate()
	{
		if (this.awesomeMode)
		{
			return 10f;
		}
		return 1f;
	}

	// Token: 0x060036E5 RID: 14053
	public void RecoverSteam()
	{
		if (Time.time > this.lastUsedSteamAt + 0.5f)
		{
			this.steam += Time.deltaTime * this.SteamRecoveryRate();
		}
	}

	// Token: 0x060036E6 RID: 14054
	public float CooldownTime()
	{
		return 3f;
	}

	// Token: 0x060036E7 RID: 14055
	public void SetXp(int total, int change, string message)
	{
		this.xp = total;
		if (change > 0)
		{
			this.CreateQuip(string.Concat(new object[]
			{
				"+",
				change,
				" xp",
				(message == null || message.Length <= 0) ? string.Empty : ("\n" + message)
			}), null);
		}
	}

	// Token: 0x060036E8 RID: 14056
	public void CreateQuip(string msg, Color? color = null)
	{
		Color color2 = ((color == null) ? new Color(1f, 0.9f, 0.1f) : color.Value);
		Quip.Create(ReplaceableSingleton<Player>.main.entityId, msg, null, new Color?(color2));
	}

	// Token: 0x060036E9 RID: 14057
	public int XpForLevel(int lvl)
	{
		if (lvl <= 1)
		{
			return 0;
		}
		return this.XpForLevel(lvl - 1) + (2000 + 500 * (lvl - 1));
	}

	// Token: 0x060036EA RID: 14058
	public int MaximumSkillLevel()
	{
		return 15;
	}

	// Token: 0x060036EB RID: 14059
	public int MaximumNaturalSkillLevel()
	{
		return 10;
	}

	// Token: 0x060036EC RID: 14060
	public List<object> UpgradeableSkills()
	{
		List<object> list = new List<object>();
		foreach (KeyValuePair<string, int> keyValuePair in this.skills)
		{
			if (keyValuePair.Value < this.MaximumNaturalSkillLevel())
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x060036ED RID: 14061
	public void SetSkill(string skillName, int skillLevel)
	{
		this.skills[skillName] = skillLevel;
		this.SkillChanged(skillName);
		this.MaxHealthChanged();
		this.MaxSteamChanged();
	}

	// Token: 0x060036EE RID: 14062
	public int Skill(string skillName)
	{
		return this.skills.Get(skillName);
	}

	// Token: 0x060036EF RID: 14063
	public int SkillBonus(string skillName)
	{
		return this.skillBonuses.Get(skillName);
	}

	// Token: 0x060036F0 RID: 14064
	public int AdjustedSkill(string skillName)
	{
		return Mathf.Clamp(this.Skill(skillName) + this.SkillBonus(skillName), 1, this.MaximumSkillLevel());
	}

	// Token: 0x060036F1 RID: 14065
	private void SkillChanged(string skillName)
	{
		Messenger.Broadcast<string, int>("playerSkillChanged", skillName, this.AdjustedSkill(skillName));
	}

	// Token: 0x060036F2 RID: 14066
	private void SkillsChanged()
	{
		foreach (string text in this.skills.Keys)
		{
			this.SkillChanged(text);
		}
	}

	// Token: 0x060036F3 RID: 14067
	public float AimSteadiness()
	{
		return (float)this.AdjustedSkill("agility");
	}

	// Token: 0x060036F4 RID: 14068
	public void AddAchievement(string achievement)
	{
		this.achievements.Add(achievement);
		Messenger.Broadcast<string>("achievementAdded", achievement);
	}

	// Token: 0x060036F5 RID: 14069
	public void SetAchievementProgress(string achievement, int progress)
	{
		this.achievementProgress[achievement] = progress;
		Messenger.Broadcast<string, int>("achievementProgressChanged", achievement, progress);
	}

	// Token: 0x060036F6 RID: 14070
	public bool HasAchievement(string achievement)
	{
		return this.achievements.Contains(achievement);
	}

	// Token: 0x060036F7 RID: 14071
	public int AchievementProgress(string achievement)
	{
		return this.achievementProgress.Get(achievement);
	}

	// Token: 0x060036F8 RID: 14072
	public void AddFollowee(string identifier, string name)
	{
		this.followees[identifier] = name;
		if (this._socialInfoProcessed)
		{
			this.followeesSorted.Add(name);
			this.followeesSorted.Sort();
			Messenger.Broadcast<string>("playerDidFollow", name);
		}
	}

	// Token: 0x060036F9 RID: 14073
	public void RemoveFollowee(string identifier)
	{
		string text = this.followees.Get(identifier);
		this.followees.Remove(identifier);
		if (this._socialInfoProcessed)
		{
			this.followeesSorted.Remove(text);
			Messenger.Broadcast<string>("playerDidUnfollow", text);
		}
	}

	// Token: 0x060036FA RID: 14074
	public void AddFollower(string identifier, string name)
	{
		this.followers[identifier] = name;
		if (this._socialInfoProcessed)
		{
			this.followersSorted.Add(name);
			this.followersSorted.Sort();
			Messenger.Broadcast<string>("playerDidGetFollowed", name);
		}
	}

	// Token: 0x060036FB RID: 14075
	public void RemoveFollower(string identifier)
	{
		string text = this.followers.Get(identifier);
		this.followers.Remove(identifier);
		if (this._socialInfoProcessed)
		{
			this.followersSorted.Remove(text);
			Messenger.Broadcast<string>("playerDidGetUnfollowed", text);
		}
	}

	// Token: 0x060036FC RID: 14076
	public bool HasFollower(string identifier)
	{
		return this.followers.ContainsKey(identifier);
	}

	// Token: 0x060036FD RID: 14077
	public bool HasFollowerNamed(string name)
	{
		return this.followersSorted.Contains(name);
	}

	// Token: 0x060036FE RID: 14078
	public bool IsFollower(string identifier)
	{
		return this.followees.ContainsKey(identifier);
	}

	// Token: 0x060036FF RID: 14079
	public bool IsFollowerNamed(string name)
	{
		return this.followeesSorted.Contains(name);
	}

	// Token: 0x06003700 RID: 14080
	public bool HasMuted(string name)
	{
		return this.mutings.ContainsKey(name);
	}

	// Token: 0x06003701 RID: 14081
	private void OnSocialInfoReady(object notification)
	{
		foreach (string text in this.followers.Values)
		{
			this.followersSorted.Add(text);
		}
		this.followersSorted.Sort();
		foreach (string text2 in this.followees.Values)
		{
			this.followeesSorted.Add(text2);
		}
		this.followeesSorted.Sort();
		this._socialInfoProcessed = true;
		Messenger.Broadcast("socialInfoProcessed");
	}

	// Token: 0x06003702 RID: 14082
	public float PlayerVisibilityDistance()
	{
		return Mathf.Lerp(25f, 100f, this.SkillLerp("perception"));
	}

	// Token: 0x06003703 RID: 14083
	public float WorldDistance(Vector2 worldPos)
	{
		return Vector2.Distance(worldPos, this.avatarTransform.position);
	}

	// Token: 0x06003704 RID: 14084
	public void TryToUseTool(Vector2 blockPosition, Vector2 worldPosition)
	{
		if (this.IsPrimaryItemGun())
		{
			this.TryToShoot(worldPosition);
			return;
		}
		if (!this.TryToMineBlock(blockPosition))
		{
			this.SwingTool(worldPosition);
		}
	}

	// Token: 0x06003705 RID: 14085
	private float MaxMiningDistance()
	{
		return Mathf.Lerp(3f, 5f, this.SkillLerp("mining"));
	}

	// Token: 0x06003706 RID: 14086
	private float MiningSpeed()
	{
		if (this.awesomeMode)
		{
			return 5f;
		}
		return Mathf.Lerp(1f, 2f, this.SkillLerp("mining"));
	}

	// Token: 0x06003707 RID: 14087
	private float MiningDuration(Item item)
	{
		float num = 0.5f;
		float num2 = Mathf.Clamp(this.primaryItem.power, 0.2f, 1f);
		num /= this.MiningSpeed();
		num /= Mathf.Lerp(1f, 2f, num2);
		num *= Mathf.Lerp(1f, 5f, (float)item.toughness / 5f * (1f - num2 * 0.9f));
		if (this.primaryItem.action == Item.Action.Dig && item.diggable)
		{
			num /= 2.5f;
		}
		return num;
	}

	// Token: 0x06003708 RID: 14088
	public bool CanMineBlock(Vector2 blockPosition, bool allowIneffectuality)
	{
		string text;
		return this.CanMineBlock(blockPosition, allowIneffectuality, out text, false);
	}

	// Token: 0x06003709 RID: 14089
	public bool CanMineBlock(Vector2 blockPosition, bool allowIneffectuality, out string failureReason, bool debug = false)
	{
		failureReason = null;
		if (this.awesomeMode)
		{
			return true;
		}
		if (this.primaryItem == null)
		{
			return false;
		}
		if ((new Vector2(blockPosition.x, -blockPosition.y) - ((Vector2)this.avatarTransform.position + this.avatarBoxCollider.offset)).magnitude > this.MaxMiningDistance())
		{
			failureReason = Locale.Text("block.too_far", null, null, null);
			return false;
		}
		if (allowIneffectuality)
		{
			return true;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		if (zoneBlock == null || zoneBlock.sceneBlock == null)
		{
			if (debug)
			{
				failureReason = Locale.Text("block.no_accessible_block", null, null, null);
			}
			return false;
		}
		if (!this.CanReachBlock(blockPosition))
		{
			if (debug)
			{
				failureReason = Locale.Text("block.cannot_reach", null, null, null);
			}
			return false;
		}
		Item item = zoneBlock.MineableItem();
		if (item == null)
		{
			if (debug)
			{
				failureReason = Locale.Text("block.no_mineable_item", null, null, null);
			}
			return false;
		}
		if (item.invulnerable && !this.activeAdmin)
		{
			failureReason = Locale.Text("block.invulnerable", null, null, null);
			return false;
		}
		if ((!item.diggable || this.primaryItem.action != Item.Action.Dig) && zoneBlock.IsProtectedAgainstPlayer(Player.Action.Mine, out failureReason))
		{
			return false;
		}
		if (!this.IsSkilledToMineItem(item))
		{
			string text = string.Concat(new object[]
			{
				Locale.Text("block.not_skilled", null, null, null),
				" (",
				item.miningSkill.Capitalize(),
				" lv ",
				item.miningSkillLevel,
				")"
			});
			failureReason = text;
			return false;
		}
		return this.primaryItem.action != Item.Action.Dig || item.code != Item.GetCode("ground/earth-dug");
	}

	// Token: 0x0600370A RID: 14090
	public bool CanReachBlock(Vector2 blockPosition)
	{
		if (this.awesomeMode)
		{
			return true;
		}
		if (this.canReachBlockCache.ContainsKey(blockPosition))
		{
			return this.canReachBlockCache[blockPosition];
		}
		Vector2 vector = new Vector2(blockPosition.x + 0.5f, -blockPosition.y - 0.5f);
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)blockPosition.x, (int)blockPosition.y, false);
		if (zoneBlock == null || zoneBlock.sceneBlock == null)
		{
			this.canReachBlockCache[blockPosition] = false;
			return false;
		}
		ZoneBlock zoneBlock2 = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		int index = zoneBlock.index;
		int num = ((zoneBlock2 == null) ? (-1) : zoneBlock2.index);
		foreach (Vector2 vector2 in this.RaycastOrigins())
		{
			Vector2 vector3 = vector - vector2;
			float num2 = Mathf.Min(new float[] { vector3.magnitude });
			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, vector3, num2, this.foregroundLayerMask);
			if (!(raycastHit2D.collider != null))
			{
				this.canReachBlockCache[blockPosition] = true;
				return true;
			}
			BlockCollider blockCollider = raycastHit2D.transform.GetComponent<BlockCollider>();
			if (blockCollider == null)
			{
				blockCollider = raycastHit2D.transform.parent.GetComponent<BlockCollider>();
			}
			if (blockCollider != null && (blockCollider.blockIndex == index || blockCollider.blockIndex == num))
			{
				this.canReachBlockCache[blockPosition] = true;
				return true;
			}
		}
		this.canReachBlockCache[blockPosition] = false;
		return false;
	}

	// Token: 0x0600370B RID: 14091
	private Vector2[] RaycastOrigins()
	{
		if (this.avatarTransform == null || this.avatarBoxCollider == null)
		{
			return new Vector2[0];
		}
		return new Vector2[]
		{
			new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y + this.avatarBoxCollider.size.y / 2f),
			new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y - this.avatarBoxCollider.size.y / 2f),
			new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x - this.avatarBoxCollider.size.x / 2f, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y),
			new Vector2(this.avatarTransform.position.x + this.avatarBoxCollider.offset.x + this.avatarBoxCollider.size.x / 2f, this.avatarTransform.position.y + this.avatarBoxCollider.offset.y)
		};
	}

	// Token: 0x0600370C RID: 14092
	public bool IsSkilledToMineItem(Item item)
	{
		return item.miningSkill == null || this.AdjustedSkill(item.miningSkill) >= item.miningSkillLevel;
	}

	// Token: 0x0600370D RID: 14093
	public void SwingTool(Vector2 worldPosition)
	{
		if (!this.avatar.alive)
		{
			return;
		}
		this.MeleeAttack(worldPosition);
		this.AnimateTool();
	}

	// Token: 0x0600370E RID: 14094
	public bool TryToMineBlock(Vector2 blockPosition)
	{
		if (this.primaryItem == null || !this.primaryItem.IsMiningTool())
		{
			return false;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		if (zoneBlock != null)
		{
			this.avatar.interactionItem = zoneBlock.MineableItem();
			this.avatar.interactionPosition = blockPosition;
			if (this.miningBlockPositionChangedAt == 0f || this.miningBlockPosition != blockPosition)
			{
				this.miningBlockPositionChangedAt = Time.time;
				this.miningBlockPosition = blockPosition;
			}
			float num = Time.time - this.miningBlockPositionChangedAt;
			string text;
			if (this.CanMineBlock(blockPosition, false, out text, false))
			{
				if (this.MineBlock(blockPosition, num))
				{
					this.miningBlockPositionChangedAt = 0f;
				}
				return true;
			}
			if (text == Locale.Text("mine.protected", null, null, null) || text == Locale.Text("mine.protected.diggable", null, null, null))
			{
				this.lastMinedProtectedAt = Time.time;
			}
			if (num > 1.5f && text != null)
			{
				Notification.Create(text, 1);
			}
		}
		else
		{
			this.avatar.interactionItem = null;
		}
		return false;
	}

    public bool CanScrubBlock(Vector2 blockPosition, bool allowIneffectuality, out string failureReason, bool debug = false)
{
	failureReason = null;
	if (this.awesomeMode) {
		return true;
	}
	if (this.primaryItem == null) {
		return false;
	}
	if ((new Vector2(blockPosition.x, -blockPosition.y) - (this.avatarTransform.position + this.avatarBoxCollider.offset)).magnitude > this.MaxMiningDistance()) {
		failureReason = Locale.Text("block.too_far", null, null, null);
		return false;
	}
	if (allowIneffectuality) {
		return true;
	}
	ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
	if (zoneBlock == null || zoneBlock.sceneBlock == null) {
		if (debug) {
			failureReason = Locale.Text("block.no_accessible_block", null, null, null);
		}
		return false;
	}
	if (!this.CanReachBlock(blockPosition)) {
		if (debug) {
			failureReason = Locale.Text("block.cannot_reach", null, null, null);
		}
		return false;
	}
	Item item = zoneBlock.ScrubbableItem();
	if (item == null) {
		if (debug) {
			failureReason = Locale.Text("block.no_mineable_item", null, null, null);
		}
		return false;
	}
	if (item.invulnerable && !this.activeAdmin) {
		failureReason = Locale.Text("block.invulnerable", null, null, null);
		return false;
	}
	if (zoneBlock.IsProtectedAgainstPlayer(Player.Action.Mine, out failureReason)) {
		return false;
	}
	if (!this.IsSkilledToMineItem(item)) {
		string text = string.Concat(new object[] {
			Locale.Text("block.not_skilled", null, null, null),
			" (",
			item.miningSkill.Capitalize(),
			" lv ",
			item.miningSkillLevel,
			")"
		});
		failureReason = text;
		return false;
	}
	return false;
}

public bool CompleteScrubbingBlock(ZoneBlock block)
{
	Item.Layer layer = block.ScrubbableLayer();
	Item item = block.ItemByLayer(layer).RootItem();
	Item primaryItem = this.primaryItem;
	int num = block.ModByLayer(layer);
	int num2 = ((item.parentItem == null) ? item.code : item.parentItem.code);
	Command.Send(Command.Identity.BlockMine, new object[] {
		block.position.x,
		block.position.y,
		(int)layer,
		num2,
		0
	});
	block.SetLayer(layer, 0, 0);
    Item item2 = ((item.mod != Item.Mod.Decay || num <= 0 || item.decayInventoryItem == null) ? item.inventoryItem : item.decayInventoryItem);
    if (item2 != null) {
        int num4 = ((item.mod != Item.Mod.Stack || num <= 0) ? 1 : num);
        this.inventory.Add(item2.code, num4);
    }
	int itemsMined = this.itemsMined;
	this.itemsMined = itemsMined + 1;
	block.Place(true);
	Messenger.Broadcast<Item>("playerMinedItem", item);
	Messenger.Broadcast<ZoneBlock>("playerMinedBlock", block);
	return true;
}


public bool ScrubBlock(Vector2 blockPosition, float miningDuration)
{
	if (!this.avatar.alive) {
		return false;
	}
	ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
	if (zoneBlock != null) {
		Item item = zoneBlock.ScrubbableItem();
		if (item != null) {
			float num = Mathf.Clamp(miningDuration / this.MiningDuration(item), 0f, 1f);
			Messenger.Broadcast<ZoneBlock, Item, float>("miningDamage", zoneBlock, item, num);
			if (num >= 1f) {
				return this.CompleteScrubbingBlock(zoneBlock);
			}
		} else {
			this.avatar.interactionItem = null;
		}
	}
	this.AnimateTool();
	return false;
}

	public bool TryToScrubBlock(Vector2 blockPosition)
	{
		if (this.primaryItem == null || !this.primaryItem.IsScrubbingTool()) {
			return false;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		if (zoneBlock != null) {
			this.avatar.interactionItem = zoneBlock.ScrubbableItem();
			this.avatar.interactionPosition = blockPosition;
			if (this.miningBlockPositionChangedAt == 0f || this.miningBlockPosition != blockPosition) {
				this.miningBlockPositionChangedAt = Time.time;
				this.miningBlockPosition = blockPosition;
			}
			float num = Time.time - this.miningBlockPositionChangedAt;
			string text;
			if (this.CanScrubBlock(blockPosition, false, out text, false)) {
				if (this.ScrubBlock(blockPosition, num)) {
					this.miningBlockPositionChangedAt = 0f;
				}
				return true;
			}
			if (text == Locale.Text("mine.protected", null, null, null) || text == Locale.Text("mine.protected.diggable", null, null, null)) {
				this.lastMinedProtectedAt = Time.time;
			}
			if (num > 1.5f && text != null) {
				Notification.Create(text, 1);
			}
		}
		else {
			this.avatar.interactionItem = null;
		}
		return false;
	}

	// Token: 0x0600370F RID: 14095
	public bool MineBlock(Vector2 blockPosition, float miningDuration)
	{
		if (!this.avatar.alive)
		{
			return false;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
		if (zoneBlock != null)
		{
			Item item = zoneBlock.MineableItem();
			if (item != null)
			{
				float num = Mathf.Clamp(miningDuration / this.MiningDuration(item), 0f, 1f);
				Messenger.Broadcast<ZoneBlock, Item, float>("miningDamage", zoneBlock, item, num);
				if (num >= 1f)
				{
					return this.CompleteMiningBlock(zoneBlock);
				}
			}
			else
			{
				this.avatar.interactionItem = null;
			}
		}
		this.AnimateTool();
		return false;
	}

	// Token: 0x06003710 RID: 14096
	public bool CompleteMiningBlock(ZoneBlock block)
	{
		Item.Layer layer = block.MineableLayer();
		Item item = block.ItemByLayer(layer).RootItem();
		Item primaryItem = this.primaryItem;
		int num = block.ModByLayer(layer);
		int num2 = ((item.parentItem == null) ? item.code : item.parentItem.code);
		Command.Send(Command.Identity.BlockMine, new object[]
		{
			block.position.x,
			block.position.y,
			(int)layer,
			num2,
			0
		});
		bool flag = primaryItem.action == Item.Action.Dig && item.diggable;
		int num3 = ((!flag) ? 0 : Item.GetCode("ground/earth-dug"));
		block.SetLayer(layer, num3, 0);
		if (!flag)
		{
			Item item2 = ((item.mod != Item.Mod.Decay || num <= 0 || item.decayInventoryItem == null) ? item.inventoryItem : item.decayInventoryItem);
			if (item2 != null)
			{
				int num4 = ((item.mod != Item.Mod.Stack || num <= 0) ? 1 : num);
				this.inventory.Add(item2.code, num4);
			}
		}
		int itemsMined = this.itemsMined;
		this.itemsMined = itemsMined + 1;
		block.Place(true);
		Messenger.Broadcast<Item>("playerMinedItem", item);
		Messenger.Broadcast<ZoneBlock>("playerMinedBlock", block);
		return true;
	}

	// Token: 0x06003711 RID: 14097
	public void CancelMiningBlock()
	{
		this.miningBlockPositionChangedAt = 0f;
	}

	// Token: 0x06003712 RID: 14098
	public bool TryToInteractAtScreenPosition(Vector2 screenPosition)
	{
		if (this.TryToUseConsumable(null))
		{
			return true;
		}
		Vector2 vector = ReplaceableSingleton<Zone>.main.ScreenToWorldPosition(screenPosition);
		Vector2 vector2 = ReplaceableSingleton<Zone>.main.WorldToBlockPosition(vector);
		return this.CanReachBlock(vector2) && this.TryToUseAtPosition(vector);
	}

	// Token: 0x06003713 RID: 14099
	public bool TryToUseConsumable(Item item = null)
	{
		if (item == null)
		{
			item = this.primaryItem;
		}
		Consumable consumable = Consumable.Get(item);
		if (consumable != null)
		{
			consumable.Use(null);
			return true;
		}
		return false;
	}

	// Token: 0x06003714 RID: 14100
	public bool TryToUseAtPosition(Vector2 worldPosition)
	{
		if (!this.avatar.alive)
		{
			return false;
		}
		Entity entity = ReplaceableSingleton<Ecosystem>.main.NearbyUsableEntity(worldPosition, 0.25f);
		if (entity != null)
		{
			Command.Send(Command.Identity.EntityUse, new object[]
			{
				entity.entityId,
				(this._primaryItem == null) ? 0 : this._primaryItem.code
			});
			return true;
		}
		Vector2 vector = ReplaceableSingleton<Zone>.main.WorldToBlockPosition(worldPosition);
		return this.TryToUseBlock(vector);
	}

	// Token: 0x06003715 RID: 14101
	public bool TryToUseBlock(Vector2 blockPosition)
	{
		if (this.CanReachBlock(blockPosition))
		{
			ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)blockPosition.x, (int)blockPosition.y);
			if (zoneBlock != null)
			{
				zoneBlock.Use();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003716 RID: 14102
	private float MaxPlacingDistance()
	{
		int num = ((this.AccessoryWithUse(Item.Use.BuildingExtension) == null) ? 1 : 2);
		return Mathf.Ceil(Mathf.Lerp(5f, 13f, this.SkillLerp("building"))) * (float)num;
	}

	// Token: 0x06003717 RID: 14103
	public bool CanPlaceBlock(Vector2 blockPosition)
	{
		string text;
		return this.CanPlaceBlock(blockPosition, out text);
	}

	// Token: 0x06003718 RID: 14104
	public bool CanPlaceBlock(Vector2 blockPosition, out string failureReason)
	{
		failureReason = null;
		if ((new Vector2(blockPosition.x, -blockPosition.y) - (Vector2)this.avatarTransform.position + this.avatarBoxCollider.offset).magnitude > this.MaxPlacingDistance())
		{
			failureReason = Locale.Text("block.too_far", null, null, null);
			return false;
		}
		if (this.inventory.Quantity(this.primaryItem) < 1)
		{
			failureReason = Locale.Text("inventory.not_enough", null, null, null);
			return false;
		}
		ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)blockPosition.x, (int)blockPosition.y, false);
		if (zoneBlock != null)
		{
			if ((this.primaryItem.fieldable == Item.Fieldable.Always || this.primaryItem.fieldable == Item.Fieldable.Placed) && zoneBlock.IsProtectedAgainstPlayer(Player.Action.Place, out failureReason))
			{
				return false;
			}
			if (this.primaryItem.layer != Item.Layer.Base)
			{
				Item item = zoneBlock.ItemByLayer(this.primaryItem.layer);
				if (item != null && item.code > 0 && !item.placeover)
				{
					failureReason = Locale.Text("block.occupied", null, null, null);
					return false;
				}
			}
			if (this.primaryItem.mounted && (zoneBlock.baseItem == null || zoneBlock.baseItem.code < 2) && (zoneBlock.backItem == null || zoneBlock.backItem.code == 0))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003719 RID: 14105
	public bool TryToPlaceBlock(Vector2 blockPosition)
	{
		if (!this.avatar.alive)
		{
			return false;
		}
		string text;
		if (this.CanPlaceBlock(blockPosition, out text))
		{
			ZoneBlock zoneBlock = ReplaceableSingleton<Zone>.main.Block((int)blockPosition.x, (int)blockPosition.y, false);
			if (zoneBlock != null)
			{
				short num = this.PlaceMod(zoneBlock, this.primaryItem);
				this.inventory.Remove(this.primaryItem.code, 1);
				zoneBlock.SetLayer(this.primaryItem.layer, this.primaryItem.code, num);
				zoneBlock.Place(true);
				Command.Send(Command.Identity.BlockPlace, new object[]
				{
					zoneBlock.position.x,
					zoneBlock.position.y,
					(int)this.primaryItem.layer,
					this.primaryItem.code,
					num
				});
				if (this.primaryItem.HasDialog())
				{
					new Dialog(this.primaryItem).UseBlock(zoneBlock, null);
				}
				GameGui.Sfx(GameGui.Sound.Place);
				int itemsPlaced = this.itemsPlaced;
				this.itemsPlaced = itemsPlaced + 1;
			}
		}
		else
		{
			global::UnityEngine.Debug.Log("Couldn't place block: " + text);
		}
		return false;
	}

	// Token: 0x0600371A RID: 14106
	private short PlaceMod(ZoneBlock block, Item item)
	{
		short num = item.placeMod;
		if (item.mod == Item.Mod.Rotation && item.mirrorable)
		{
			num = (short)((!this.IsLookingLeft()) ? 0 : 4);
		}
		return num;
	}

	// Token: 0x0600371B RID: 14107
	public bool IsLookingLeft()
	{
		return this.avatar.lookDirection == Entity.LookDirection.Left;
	}

	// Token: 0x0600371C RID: 14108
	public bool IsLookingRight()
	{
		return this.avatar.lookDirection == Entity.LookDirection.Right;
	}

	// Token: 0x0600371D RID: 14109
	private void UseNearbyItems()
	{
	}

	// Token: 0x0600371E RID: 14110
	public bool TryToDropItem(Vector2 worldPosition, Item item)
	{
		EntityAvatar entityAvatar = ReplaceableSingleton<Ecosystem>.main.NearbyPeer(worldPosition, 0.25f);
		if (entityAvatar != null)
		{
			Command.Send(Command.Identity.EntityUse, new object[]
			{
				entityAvatar.entityId,
				new object[] { "trade", item.code }
			});
			return true;
		}
		Entity entity = ReplaceableSingleton<Ecosystem>.main.NearbyUsableEntity(worldPosition, 0.25f);
		if (entity != null && entity.config.usable)
		{
			Command.Send(Command.Identity.EntityUse, new object[]
			{
				entity.entityId,
				new object[] { "item", item.code }
			});
			return true;
		}
		return false;
	}

	// Token: 0x0600371F RID: 14111
	public void Heal(float amount)
	{
		this.health += amount;
	}

	// Token: 0x06003720 RID: 14112
	private bool CanBeDamaged()
	{
		return (double)Time.time > (double)this.lastDamageAt + 0.667 && this.health > 0f && !this.awesomeMode;
	}

	// Token: 0x06003721 RID: 14113
	private bool Damage(Item.Damage type, float amount, Entity attacker)
	{
		if (attacker == this.avatar || (this.CanBeDamaged() && amount > 0f))
		{
			ExternalConsole.Log("Damage", string.Concat(new string[]
			{
				type.ToString(),
				" - ",
				amount.ToString(),
				" - ",
				(attacker ?? new Entity()).ToString()
			}));
			float num = 0f;
			switch (type)
			{
			case Item.Damage.Acid:
				num = this.ElementalDamageResistance();
				break;
			case Item.Damage.Bludgeoning:
				num = this.MeleeDamageResistance();
				break;
			case Item.Damage.Cold:
				num = this.ElementalDamageResistance();
				break;
			case Item.Damage.Fire:
				num = this.ElementalDamageResistance();
				break;
			case Item.Damage.Ink:
				num = this.ElementalDamageResistance();
				break;
			case Item.Damage.Piercing:
				num = this.MeleeDamageResistance();
				break;
			case Item.Damage.Slashing:
				num = this.MeleeDamageResistance();
				break;
			case Item.Damage.Steam:
				num = this.ElementalDamageResistance();
				break;
			}
			this.lastAttackedBy = attacker;
			ExternalConsole.Log("Percieved Damage", string.Concat(new string[]
			{
				type.ToString(),
				" - ",
				(amount * (1f - num)).ToString(),
				" - ",
				(attacker ?? new Entity()).ToString()
			}));
			if (type == Item.Damage.Dessication)
			{
				this.steam -= amount * (1f - num);
			}
			else
			{
				this.health -= amount * (1f - num);
				int num2 = ((type != Item.Damage.Stink) ? 0 : 11);
				object[] array = new object[]
				{
					(int)(this.health * 1000f),
					(!attacker) ? 0 : attacker.entityId,
					num2
				};
				Command.Send(Command.Identity.Health, array);
			}
			this.lastDamageAt = Time.time;
			this.avatar.AnimateHurt(type);
			this.soundSource.PlayGroup("vox", this.HurtSound(amount * (1f - num) >= 1f), 0.7f);
			return true;
		}
		return false;
	}

	// Token: 0x06003722 RID: 14114
	public bool IsSubmerged()
	{
		return this.currentLiquidLevel > 5;
	}

	// Token: 0x06003723 RID: 14115
	private void StepSubmersion(Item liquidItem, bool wasSubmerged)
	{
		if (this.submerged)
		{
			if (!wasSubmerged)
			{
				this.lastSubmergedAt = Time.time;
				this.soundSource.PlayGroup("env", "liquid/splash/in", 0.333f);
				ReplaceableSingleton<ZoneRenderer>.main.Splash(liquidItem, new Vector3(this.avatarTransform.position.x, Mathf.Ceil(this.avatarTransform.position.y), this.avatarTransform.position.z));
			}
			if (Time.time - this.lastSubmergedAt > 1f)
			{
				this.EnvironmentalDamage(liquidItem.damageType, liquidItem.damageAmount);
				return;
			}
		}
		else if (wasSubmerged)
		{
			this.soundSource.PlayGroup("env", "liquid/splash/out", 0.333f);
			if (this.BelowBlock() != null)
			{
				liquidItem = this.BelowBlock().liquidItem;
			}
			ReplaceableSingleton<ZoneRenderer>.main.Splash(liquidItem, new Vector3(this.avatarTransform.position.x, Mathf.Floor(this.avatarTransform.position.y), this.avatarTransform.position.z));
		}
	}

	// Token: 0x06003724 RID: 14116
	private void EnvironmentalDamage(Item.Damage type, float amount)
	{
		if (Time.time > this.lastEnvironmentalDamageAt + 1f && this.AccessoryWithUse(Item.Use.Hazmat) == null)
		{
			if (amount > 0f)
			{
				this.Damage(type, amount, null);
			}
			this.lastEnvironmentalDamageAt = Time.time;
		}
	}

	// Token: 0x06003725 RID: 14117
	public void BlockDamage(Item blockItem, Vector3 velocity = default(Vector3), bool isFeet = false)
	{
		if (Time.time > this.lastEnvironmentalDamageAt + 1f)
		{
			if (velocity.magnitude >= blockItem.damageMinVelocity)
			{
				if (blockItem.damageType == Item.Damage.Piercing && this.AccessoryWithUse(Item.Use.Hazmat) != null)
				{
					return;
				}
				float num = ((!isFeet || this.stompAccessory == null) ? 0f : this.stompAccessory.Defense("all"));
				if (blockItem.damageType == Item.Damage.Piercing)
				{
					this.Damage(blockItem.damageType, blockItem.damageAmount * (1f - num), null);
				}
			}
			this.lastEnvironmentalDamageAt = Time.time;
		}
	}

	// Token: 0x06003726 RID: 14118
	public float Depth()
	{
		return this.position.y / (float)ReplaceableSingleton<Zone>.main.blockSize.height;
	}

	// Token: 0x06003727 RID: 14119
	public float SkillLerp(string skill)
	{
		float num = 15f;
		return (float)this.AdjustedSkill(skill) / num;
	}

	// Token: 0x06003728 RID: 14120
	private float MeleeDamageResistance()
	{
		return Mathf.Lerp(0f, 0.5f, this.SkillLerp("agility"));
	}

	// Token: 0x06003729 RID: 14121
	private float ElementalDamageResistance()
	{
		return Mathf.Lerp(0f, 0.5f, this.SkillLerp("survival"));
	}

	// Token: 0x0600372A RID: 14122
	private float PoisonResistance()
	{
		return Mathf.Lerp(0f, 0.5f, this.SkillLerp("stamina"));
	}

	// Token: 0x0600372B RID: 14123
	private bool CanEvadePrecipitationDamage(Item.Damage type)
	{
		if (this.AccessoryWithUse(Item.Use.Hazmat) != null)
		{
			return true;
		}
		int num = this.AdjustedSkill("survival");
		if (type == Item.Damage.Acid)
		{
			return num >= 10;
		}
		return type == Item.Damage.Fire && num >= 15;
	}

	// Token: 0x0600372C RID: 14124
	private void MeleeAttack(Vector2 worldPosition)
	{
		if (Vector2.Distance(worldPosition, this.avatarTransform.position) <= this.MeleeAttackRange())
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(worldPosition, 1f, this.entityLayerMask);
			for (int i = 0; i < array.Length; i++)
			{
				Entity component = array[i].gameObject.GetComponent<Entity>();
				this.AddTarget(component);
			}
		}
	}

	// Token: 0x0600372D RID: 14125
	private float MeleeAttackRange()
	{
		return 2.5f;
	}

	// Token: 0x0600372E RID: 14126
	public void AddTarget(Entity entity)
	{
		this.targetEntities[entity.entityId] = Time.time + 0.5f;
	}

	// Token: 0x0600372F RID: 14127
	public Entity AnyTarget()
	{
		foreach (int num in this.targetEntities.Keys)
		{
			Entity entity = ReplaceableSingleton<Ecosystem>.main.GetEntity(num);
			if (entity != null)
			{
				return entity;
			}
		}
		return null;
	}

	// Token: 0x06003730 RID: 14128
	private void StepTargeting()
	{
		float time = Time.time;
		foreach (int num in this.targetEntities.Keys.ToList<int>())
		{
			if (time > this.targetEntities[num])
			{
				this.targetEntities.Remove(num);
			}
		}
		if (time > this.lastInventoryUseMessageAt + 0.3335f && this.targetEntities.Count > 0)
		{
			this.SendInventoryUseCommand(Player.InventoryUse.Start, false);
		}
	}

	// Token: 0x06003731 RID: 14129
	public bool TryToShoot(Vector2 pt)
	{
		this._target = new Vector2(pt.x, -pt.y);
		if (!this.avatar.ReadyGun(this.primaryItem, pt))
		{
			return false;
		}
		float num = this.primaryItem.ammoCost;
		if (num == 0f)
		{
			num = 1f;
		}
		if (!this.HasSteam())
		{
			return false;
		}
		this.UseSteam(num, false, true);
		this.avatar.ShootGun(this.primaryItem, pt, true);
		if (!this.wasShooting)
		{
			this.SendInventoryUseCommand(Player.InventoryUse.Start, false);
		}
		this.wasShooting = true;
		this.lastWasShootingAt = Time.time;
		return true;
	}

	// Token: 0x06003732 RID: 14130
	public void StopShooting()
	{
		if (this.wasShooting)
		{
			this.SendInventoryUseCommand(Player.InventoryUse.End, false);
			if (this.primaryItem != null && this.primaryItem.IsGun())
			{
				this.avatar.StopShooting(true);
			}
		}
		this.wasShooting = false;
	}

	// Token: 0x06003733 RID: 14131
	private void OnShieldOn()
	{
		this.ToggleShield(true);
	}

	// Token: 0x06003734 RID: 14132
	private void OnShieldOff()
	{
		this.ToggleShield(false);
	}

	// Token: 0x06003735 RID: 14133
	private void OnShieldToggle()
	{
		if (this.activeShield != null)
		{
			this.ToggleShield(false);
			return;
		}
		if (!this.HasSteam())
		{
			return;
		}
		this.ToggleShield(true);
	}

	// Token: 0x06003736 RID: 14134
	public void ToggleShield(bool on)
	{
		if (on)
		{
			this.activeShield = this.AccessoryWithAction(Item.Action.Shield);
		}
		else
		{
			this.activeShield = null;
		}
		this.avatar.shieldItem = this.activeShield;
		Command.Identity identity = Command.Identity.InventoryUse;
		object[] array = new object[4];
		array[0] = 1;
		array[1] = ((this.activeShield == null) ? 0 : this.activeShield.code);
		array[2] = ((this.activeShield != null) ? 1 : 0);
		Command.Send(identity, array);
		Messenger.Broadcast<bool>("playerShieldActivated", on);
	}

	// Token: 0x06003737 RID: 14135
	public bool HasShield()
	{
		return this.AccessoryWithAction(Item.Action.Shield) != null;
	}

	// Token: 0x06003738 RID: 14136
	public void Emote(string animationName)
	{
		if (animationName == null)
		{
			this.currentEmoteAnimation = null;
			return;
		}
		foreach (Item item in this.inventory.wardrobe)
		{
			if (item.name == "emotes/" + animationName)
			{
				string text = item.animation;
				if (text == null)
				{
					text = animationName;
				}
				this.currentEmoteAnimation = new PlayerAnimation(text, this.avatar.AnimationDetails(text + "-idle") == null);
				break;
			}
		}
	}

	// Token: 0x06003739 RID: 14137
	public void EmoteIdle(string animationName)
	{
		this.currentEmoteAnimation = new PlayerAnimation(animationName, true);
	}

	// Token: 0x0600373A RID: 14138
	private void AnimateTool()
	{
		this.avatar.AnimateTool();
		this._lastUsedToolAt = Time.time;
		if (this.lastToolCommandUse == Player.InventoryUse.End)
		{
			this.SendInventoryUseCommand(Player.InventoryUse.Start, false);
			this.lastToolCommandUse = Player.InventoryUse.Start;
		}
	}

	// Token: 0x0600373B RID: 14139
	private PlayerAnimation CurrentAnimation()
	{
		switch (this.movement)
		{
		case Player.Movement.Walking:
			return new PlayerAnimation("walk", true);
		case Player.Movement.Running:
			return new PlayerAnimation("run", true);
		case Player.Movement.Jumping:
			return new PlayerAnimation("fly", true);
		case Player.Movement.Climbing:
			return new PlayerAnimation("climb", true);
		case Player.Movement.Hovering:
			return new PlayerAnimation("falling-1", true);
		case Player.Movement.Flying:
			return new PlayerAnimation("fly", true);
		case Player.Movement.Falling:
			return new PlayerAnimation("falling-1", true);
		case Player.Movement.Flailing:
			return new PlayerAnimation("flail", true);
		case Player.Movement.Swimming:
			return new PlayerAnimation("swim-1", true);
		case Player.Movement.Propelling:
			return new PlayerAnimation("fly", true);
		case Player.Movement.Stomping:
			return new PlayerAnimation("stomp-1", true);
		case Player.Movement.Balancing:
			return new PlayerAnimation("edge-flail", true);
		default:
			return this.CurrentIdleAnimation();
		}
	}

	// Token: 0x0600373C RID: 14140
	private PlayerAnimation CurrentIdleAnimation()
	{
		if (Time.time > this.changeIdleAt)
		{
			if (this.currentIdleAnimation != null)
			{
				this.currentIdleAnimation = null;
				this.changeIdleAt = Time.time + global::UnityEngine.Random.Range(10f, 15f);
			}
			else
			{
				this.currentIdleAnimation = new PlayerAnimation((!this.submerged) ? ("idle-" + global::UnityEngine.Random.Range(2, 5).ToString()) : ("swim-idle-flourish-" + global::UnityEngine.Random.Range(1, 4).ToString()), true);
				Spine.Animation animation = this.avatar.AnimationDetails(this.currentIdleAnimation.name);
				this.changeIdleAt = Time.time + ((animation == null) ? 5f : animation.duration);
			}
		}
		if (Time.time < this.lastMovedAt + 0.5f)
		{
			this.changeIdleAt = Time.time + global::UnityEngine.Random.Range(8f, 10f);
			if (this.currentIdleAnimation != null)
			{
				this.currentIdleAnimation = null;
			}
			this.currentEmoteAnimation = null;
		}
		PlayerAnimation playerAnimation = new PlayerAnimation("idle-1", true);
		if (this.currentEmoteAnimation != null)
		{
			playerAnimation = this.currentEmoteAnimation;
		}
		else if (this.currentIdleAnimation != null)
		{
			playerAnimation = this.currentIdleAnimation;
		}
		else if (this.submerged)
		{
			playerAnimation.name = "swim-idle";
		}
		return playerAnimation;
	}

	// Token: 0x0600373D RID: 14141
	private void SetLightColor(Color color)
	{
		Mesh mesh = this.light.GetComponent<MeshFilter>().mesh;
		Color[] array = new Color[mesh.vertices.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		mesh.colors = array;
	}

	// Token: 0x0600373E RID: 14142
	public void IncrementDirectingState()
	{
	}

	// Token: 0x0600373F RID: 14143
	private void SendStatus()
	{
	}

	// Token: 0x06003740 RID: 14144
	public void Respawn()
	{
		if (!this.avatar.alive)
		{
			GameManager.GC();
			Command.Send(Command.Identity.Respawn, new object[] { new int[1] });
			this.currentEmoteAnimation = null;
			Messenger.Broadcast("playerRespawned");
		}
	}

	// Token: 0x06003741 RID: 14145
	private Item AccessoryWithUse(Item.Use use)
	{
		foreach (Item item in this.inventory.accessories)
		{
			if (item.IsUsableType(use))
			{
				return item;
			}
		}
		return null;
	}

	// Token: 0x06003742 RID: 14146
	private Item HiddenItemWithUse(Item.Use use)
	{
		return null;
	}

	// Token: 0x06003743 RID: 14147
	private Item AccessoryWithAction(Item.Action action)
	{
		foreach (Item item in this.inventory.accessories)
		{
			if (item.action == action)
			{
				return item;
			}
		}
		return null;
	}

	// Token: 0x06003744 RID: 14148
	public static void Sfx(string sound)
	{
		ReplaceableSingleton<Player>.main.soundSource.Play(sound, 1f);
	}

	// Token: 0x06003745 RID: 14149
	private string HurtSound(bool isIntense)
	{
		string text = ((this.settings.Get("playerSounds") != null && this.settings.GetInt("playerSounds", 0) != 0) ? "female" : "male");
		if (isIntense)
		{
			return "ouch/" + text + "/heavy";
		}
		return "ouch/" + text + "/light";
	}

	// Token: 0x06003746 RID: 14150
	private void OnDrawGizmos()
	{
		foreach (Vector2 vector in this.RaycastOrigins())
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(vector, 0.1f);
		}
	}

	// Token: 0x06003747 RID: 14151
	public string DebugInfo()
	{
		Vector2 pointerBlockPosition = ReplaceableSingleton<GameGui>.main.pointerBlockPosition;
		string text;
		bool flag = this.CanMineBlock(pointerBlockPosition, false, out text, true);
		string text2;
		bool flag2 = this.CanPlaceBlock(pointerBlockPosition, out text2);
		object[] array = new object[26];
		array[0] = "[Player] Pos: ";
		array[1] = this.position;
		array[2] = ", movement: ";
		array[3] = this.movement;
		array[4] = " - ";
		array[5] = this.inputVelocity.x.ToString("F2");
		array[6] = " x ";
		array[7] = this.inputVelocity.y.ToString("F2");
		array[8] = " = ";
		int num = 9;
		Vector3 vector = this.velocity;
		array[num] = vector.x.ToString("F2");
		array[10] = " x ";
		int num2 = 11;
		vector = this.velocity;
		array[num2] = vector.y.ToString("F2");
		array[12] = ", grounded? ";
		array[13] = this.IsGrounded();
		array[14] = ", can mine? ";
		array[15] = ((!flag) ? text : "yes");
		array[16] = ", can place? ";
		array[17] = ((!flag2) ? text2 : "yes");
		array[18] = ", was shoot? ";
		array[19] = this.wasShooting;
		array[20] = " - last shoot? ";
		array[21] = this.lastWasShootingAt;
		array[22] = ", viz ";
		array[23] = (int)ReplaceableSingleton<CameraManager>.main.MaxPlayerBlockVisibility();
		array[24] = ", target ";
		array[25] = this.targetEntities.Count;
		return string.Concat(array);
	}

	// Token: 0x06003748 RID: 14152
	[ContextMenu("Log Accessories")]
	private void LogAccessories()
	{
		global::UnityEngine.Debug.Log("Accessories:");
		foreach (Item item in this.inventory.accessories)
		{
			global::UnityEngine.Debug.Log("- " + item.name);
		}
		global::UnityEngine.Debug.Log("Hidden Items:");
		foreach (Item item2 in this.inventory.hidden)
		{
			global::UnityEngine.Debug.Log("- " + item2.name);
		}
	}

	// Token: 0x06003749 RID: 14153
	static Player()
	{
	}

	// Token: 0x0600374A RID: 14154
	public void OnGUI()
	{
		if (ExternalConsole.GetInstance() != null && ExternalConsole.GetInstance().enabled)
		{
			ExternalConsole.GetInstance().Draw();
		}
	}

	// Token: 0x060053A8 RID: 21416
	public bool IsPrimaryItemMiningTool()
	{
		return this._primaryItem != null && this._primaryItem.IsMiningTool();
	}

	// Token: 0x060053A9 RID: 21417
	public bool IsPrimaryItemScrubbingTool()
	{
		return this._primaryItem != null && this._primaryItem.IsScrubbingTool();
	}

	// Token: 0x04001E13 RID: 7699
	public const int TeleportAnywhereSkillRequirement = 7;

	// Token: 0x04001E14 RID: 7700
	public const float BaseSteam = 20f;

	// Token: 0x04001E15 RID: 7701
	public const float targetPersistenceTime = 0.5f;

	// Token: 0x04001E16 RID: 7702
	private Player.Movement movement;

	// Token: 0x04001E17 RID: 7703
	public Inventory inventory;

	// Token: 0x04001E18 RID: 7704
	public string documentId;

	// Token: 0x04001E19 RID: 7705
	public string apiToken;

	// Token: 0x04001E1A RID: 7706
	public ObscuredBool admin;

	// Token: 0x04001E1B RID: 7707
	public ObscuredBool activeAdmin;

	// Token: 0x04001E1C RID: 7708
	public int entityId;

	// Token: 0x04001E1D RID: 7709
	public Transform avatarTransform;

	// Token: 0x04001E1E RID: 7710
	public CharacterController2D avatarController2D;

	// Token: 0x04001E1F RID: 7711
	public EntityAvatar avatar;

	// Token: 0x04001E20 RID: 7712
	private EntityConfig entityConfig;

	// Token: 0x04001E21 RID: 7713
	private BoxCollider2D avatarBoxCollider;

	// Token: 0x04001E22 RID: 7714
	public Transform light;

	// Token: 0x04001E23 RID: 7715
	private const float groundingRadius = 0.2f;

	// Token: 0x04001E24 RID: 7716
	private const float horizontalSpeed = 8.5f;

	// Token: 0x04001E25 RID: 7717
	private const float verticalSpeed = 12f;

	// Token: 0x04001E26 RID: 7718
	private Vector3 inputVelocity;

	// Token: 0x04001E27 RID: 7719
	private Vector3 _velocity;

	// Token: 0x04001E28 RID: 7720
	private Vector3 _target;

	// Token: 0x04001E29 RID: 7721
	private int rotation;

	// Token: 0x04001E2A RID: 7722
	private const float maxUpwardSpeed = 12f;

	// Token: 0x04001E2B RID: 7723
	private const float maxDownwardSpeed = 14.5f;

	// Token: 0x04001E2C RID: 7724
	private const float maxFallSpeed = 12.5f;

	// Token: 0x04001E2D RID: 7725
	private const float gravityMod = 1.5f;

	// Token: 0x04001E2E RID: 7726
	private float lastMovedAt;

	// Token: 0x04001E2F RID: 7727
	private float lastGroundedAt;

	// Token: 0x04001E30 RID: 7728
	private float lastPropelledUpwardAt;

	// Token: 0x04001E31 RID: 7729
	private float lastJumpedAt;

	// Token: 0x04001E32 RID: 7730
	private float _lastTeleportedAt;

	// Token: 0x04001E33 RID: 7731
	private ObscuredBool _suppressed = false;

	// Token: 0x04001E34 RID: 7732
	private ObscuredBool grounded;

	// Token: 0x04001E35 RID: 7733
	private ObscuredBool partiallyGrounded;

	// Token: 0x04001E36 RID: 7734
	public int groundedLayerMask;

	// Token: 0x04001E37 RID: 7735
	public int foregroundLayerMask;

	// Token: 0x04001E38 RID: 7736
	public int entityLayerMask;

	// Token: 0x04001E39 RID: 7737
	public static float walkToRunDelay = 0.25f;

	// Token: 0x04001E3A RID: 7738
	private float movementDamping;

	// Token: 0x04001E3B RID: 7739
	private bool movementLocked;

	// Token: 0x04001E3C RID: 7740
	private Dictionary<Vector2, bool> canReachBlockCache = new Dictionary<Vector2, bool>();

	// Token: 0x04001E3D RID: 7741
	public int directingState;

	// Token: 0x04001E3E RID: 7742
	private Vector2 miningBlockPosition;

	// Token: 0x04001E3F RID: 7743
	private float miningBlockPositionChangedAt;

	// Token: 0x04001E40 RID: 7744
	private float _lastUsedToolAt;

	// Token: 0x04001E42 RID: 7746
	private Player.InventoryUse lastToolCommandUse = Player.InventoryUse.End;

	// Token: 0x04001E43 RID: 7747
	private Dictionary<int, float> targetEntities = new Dictionary<int, float>();

	// Token: 0x04001E44 RID: 7748
	private ObscuredBool _awesomeMode;

	// Token: 0x04001E45 RID: 7749
	private ObscuredBool _clip = true;

	// Token: 0x04001E46 RID: 7750
	public int currentLiquidLevel;

	// Token: 0x04001E47 RID: 7751
	private bool wasSubmerged;

	// Token: 0x04001E48 RID: 7752
	public bool submerged;

	// Token: 0x04001E49 RID: 7753
	public float lastSubmergedAt;

	// Token: 0x04001E4A RID: 7754
	private float canConsumeAt;

	// Token: 0x04001E4B RID: 7755
	private float lastUsedSteamAt;

	// Token: 0x04001E4C RID: 7756
	private float cooldownStartedAt;

	// Token: 0x04001E4D RID: 7757
	private float lastDamageAt;

	// Token: 0x04001E4E RID: 7758
	private float lastEnvironmentalDamageAt;

	// Token: 0x04001E4F RID: 7759
	private Entity lastAttackedBy;

	// Token: 0x04001E50 RID: 7760
	private bool wasShooting;

	// Token: 0x04001E51 RID: 7761
	private float lastWasShootingAt;

	// Token: 0x04001E52 RID: 7762
	private float lastStompedAt;

	// Token: 0x04001E53 RID: 7763
	private float lastInventoryUseMessageAt;

	// Token: 0x04001E54 RID: 7764
	private Item _primaryItem;

	// Token: 0x04001E55 RID: 7765
	private Item flyAccessory;

	// Token: 0x04001E56 RID: 7766
	private float flyAccessoryPower;

	// Token: 0x04001E57 RID: 7767
	private Item stompAccessory;

	// Token: 0x04001E58 RID: 7768
	private Item activeShield;

	// Token: 0x04001E59 RID: 7769
	private Dictionary<string, object> settings;

	// Token: 0x04001E5A RID: 7770
	private Dictionary<string, int> stats;

	// Token: 0x04001E5B RID: 7771
	private Dictionary<string, int> skills;

	// Token: 0x04001E5C RID: 7772
	private Dictionary<string, int> skillBonuses;

	// Token: 0x04001E5D RID: 7773
	private List<string> achievements;

	// Token: 0x04001E5E RID: 7774
	private Dictionary<string, int> achievementProgress;

	// Token: 0x04001E5F RID: 7775
	public string karma;

	// Token: 0x04001E60 RID: 7776
	public bool karmaIsPoor;

	// Token: 0x04001E61 RID: 7777
	private bool _socialInfoProcessed;

	// Token: 0x04001E62 RID: 7778
	public Dictionary<string, string> followees;

	// Token: 0x04001E63 RID: 7779
	public List<string> followeesSorted = new List<string>();

	// Token: 0x04001E64 RID: 7780
	public Dictionary<string, string> followers;

	// Token: 0x04001E65 RID: 7781
	public List<string> followersSorted = new List<string>();

	// Token: 0x04001E66 RID: 7782
	private Dictionary<string, object> mutings;

	// Token: 0x04001E67 RID: 7783
	public Dictionary<string, object> onlinePlayers = new Dictionary<string, object>();

	// Token: 0x04001E68 RID: 7784
	private float nextMovementCommandTime;

	// Token: 0x04001E69 RID: 7785
	private float changeIdleAt;

	// Token: 0x04001E6A RID: 7786
	private float startedRunningAt;

	// Token: 0x04001E6B RID: 7787
	private PlayerAnimation currentEmoteAnimation;

	// Token: 0x04001E6C RID: 7788
	private PlayerAnimation currentIdleAnimation;

	// Token: 0x04001E6D RID: 7789
	private int animationIndex;

	// Token: 0x04001E6E RID: 7790
	private ZoneBlock targetBlock;

	// Token: 0x04001E6F RID: 7791
	private bool targetBlockAccessible;

	// Token: 0x04001E70 RID: 7792
	public bool isTeleporting;

	// Token: 0x04001E71 RID: 7793
	private string _name;

	// Token: 0x04001E72 RID: 7794
	private int _level;

	// Token: 0x04001E73 RID: 7795
	private int _xp;

	// Token: 0x04001E74 RID: 7796
	private int xpForCurrentLevel;

	// Token: 0x04001E75 RID: 7797
	private int xpForNextLevel;

	// Token: 0x04001E76 RID: 7798
	private ObscuredFloat _health;

	// Token: 0x04001E77 RID: 7799
	private ObscuredFloat _breath;

	// Token: 0x04001E78 RID: 7800
	private ObscuredFloat _freeze;

	// Token: 0x04001E79 RID: 7801
	private ObscuredFloat _thirst;

	// Token: 0x04001E7A RID: 7802
	private ObscuredFloat _steam;

	// Token: 0x04001E7B RID: 7803
	private int _playTime;

	// Token: 0x04001E7C RID: 7804
	private int _itemsMined;

	// Token: 0x04001E7D RID: 7805
	private int _itemsPlaced;

	// Token: 0x04001E7E RID: 7806
	private int _itemsCrafted;

	// Token: 0x04001E7F RID: 7807
	private int _deaths;

	// Token: 0x04001E80 RID: 7808
	private bool _premium;

	// Token: 0x04001E81 RID: 7809
	private int _crowns;

	// Token: 0x04001E82 RID: 7810
	private int _skillPoints;

	// Token: 0x04001E83 RID: 7811
	private ObscuredVector2 _position;

	// Token: 0x04001E84 RID: 7812
	private FixedPosition lastSentMovementPosition;

	// Token: 0x04001E85 RID: 7813
	public bool isZoneTeleporting;

	// Token: 0x04001E86 RID: 7814
	public ZoneBlock collidingBlock;

	// Token: 0x04001E87 RID: 7815
	public Vector3 externalVelocity;

	// Token: 0x020006F7 RID: 1783
	public enum Movement
	{
		// Token: 0x04001E89 RID: 7817
		Idle,
		// Token: 0x04001E8A RID: 7818
		Walking,
		// Token: 0x04001E8B RID: 7819
		Running,
		// Token: 0x04001E8C RID: 7820
		Jumping,
		// Token: 0x04001E8D RID: 7821
		Climbing,
		// Token: 0x04001E8E RID: 7822
		Hovering,
		// Token: 0x04001E8F RID: 7823
		Flying,
		// Token: 0x04001E90 RID: 7824
		Falling,
		// Token: 0x04001E91 RID: 7825
		Flailing,
		// Token: 0x04001E92 RID: 7826
		Swimming,
		// Token: 0x04001E93 RID: 7827
		Propelling,
		// Token: 0x04001E94 RID: 7828
		Stomping,
		// Token: 0x04001E95 RID: 7829
		Balancing
	}

	// Token: 0x020006F8 RID: 1784
	public enum InventoryUse
	{
		// Token: 0x04001E97 RID: 7831
		Select,
		// Token: 0x04001E98 RID: 7832
		Start,
		// Token: 0x04001E99 RID: 7833
		End
	}

	// Token: 0x020006F9 RID: 1785
	public enum Action
	{
		// Token: 0x04001E9B RID: 7835
		Mine,
		// Token: 0x04001E9C RID: 7836
		Place
	}
}

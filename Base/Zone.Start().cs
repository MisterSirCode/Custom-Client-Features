	protected void Start()
	{
		this.mainCamera = Camera.main;
		this._ecosystem = base.gameObject.AddComponent<Ecosystem>();
		this.deathPositions = new List<Vector2>();
		int[][] array = new int[15][];
		array[0] = new int[] { 0, 1 };
		int num = 1;
		int[] array2 = new int[2];
		array2[0] = -1;
		array[num] = array2;
		array[2] = new int[] { 0, 2 };
		int num2 = 3;
		int[] array3 = new int[2];
		array3[0] = -2;
		array[num2] = array3;
		array[4] = new int[] { -1, 1 };
		array[5] = new int[] { -1, 2 };
		array[6] = new int[] { -2, 1 };
		array[7] = new int[] { -2, 2 };
		array[8] = new int[] { 0, 3 };
		array[9] = new int[] { -1, 3 };
		int num3 = 10;
		int[] array4 = new int[2];
		array4[0] = -3;
		array[num3] = array4;
		array[11] = new int[] { -2, 3 };
		array[12] = new int[] { -3, 1 };
		array[13] = new int[] { -3, 2 };
		array[14] = new int[] { -3, 3 };
		this.adjacentMining = array;
		Messenger.AddListener<object>("zoneEntered", new Callback<object>(this.OnEntered));
		Messenger.AddListener<Vector2>("blockInfoRequest", new Callback<Vector2>(this.DebugBlock));
		Messenger.AddListener("toggleZoneBookmark", new Callback(this.OnToggleZoneBookmark));
		Destroy(Camera.current.GetComponent<ExternalMusicLoader>());
		Camera.current.gameObject.AddComponent<ExternalMusicLoader>();
	}
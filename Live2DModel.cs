using live2d;
using local.models;
using local.utils;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Live2DModel : SingletonMonoBehaviour<Live2DModel>
{
	public enum MotionType
	{
		Port,
		Battle,
		Loop,
		Secret,
		Dislike1,
		Dislike2,
		Love1,
		Love2
	}

	private Camera myCamera;

	[Button("PlayDebug", "Play", new object[]
	{

	})]
	public int play;

	[Button("forceStop", "Stop", new object[]
	{

	})]
	public int stop;

	[Button("NextMotion", "NextMotion", new object[]
	{

	})]
	public int NextMotionButton;

	[Button("DebugChange", "DebugChange", new object[]
	{

	})]
	public int DebugChangeCharacter;

	[Button("setModel", "Reload", new object[]
	{

	})]
	public int Reload;

	public Live2DModel.MotionType RandomMotionType;

	[Button("RandomMotion", "RandomMotion", new object[]
	{

	})]
	public int Random;

	[Button("PlayDebugPrevMotion", "PlayPrevMotion", new object[]
	{

	})]
	public int PrevMotion;

	public TextAsset prevMotionFile;

	public bool isDontRelease;

	public int DebugMstID;

	public int NowMstID;

	public static bool __DEBUG_MotionNAME_Draw;

	private readonly string[] motionName = new string[]
	{
		"_port.mtn",
		"_battle.mtn",
		"_loop.mtn",
		"_secret.mtn"
	};

	[SerializeField]
	private Live2DModel.MotionType NowMotion;

	private Live2DModelUnity live2DModelUnity;

	private Matrix4x4 live2DCanvasPos;

	private MotionQueueManager motionMgr;

	public TextAsset motionFile;

	public TextAsset mocFile;

	public Texture2D[] textureFiles;

	public Live2DMotion motion;

	private IEnumerator IEnum_MotionFinish;

	public bool isLive2DModel;

	public bool isLive2DChange;

	public bool isOneDraw;

	private bool isStop;

	private bool isOnePlay;

	private bool isDrawed;

	public Action StopAction;

	public float modelW;

	public float modelH;

	public float aspect;

	private string[] DebugMotionNameList = new string[]
	{
		"_port.mtn",
		"_battle.mtn",
		"_loop.mtn",
		"_secret.mtn"
	};

	[SerializeField]
	private UITexture DebugTexture;

	[Button("DebugChangeMotion", "DebugChangeMotion", new object[]
	{

	})]
	public int Button33;

	public bool IsStop
	{
		get
		{
			return this.isStop;
		}
		private set
		{
			this.isStop = value;
			if (this.isStop && this.StopAction != null)
			{
				this.StopAction.Invoke();
				this.StopAction = null;
			}
		}
	}

	protected override void Awake()
	{
		if (base.CheckInstance())
		{
			Live2D.init();
			this.motionMgr = new MotionQueueManager();
			this.IsStop = false;
			this.isOneDraw = false;
			this.isOnePlay = false;
			this.isLive2DModel = false;
			this.myCamera = base.GetComponent<Camera>();
		}
	}

	private void OnPostRender()
	{
		if (this.live2DModelUnity == null)
		{
			return;
		}
		this.live2DModelUnity.setMatrix(base.get_transform().get_localToWorldMatrix() * this.live2DCanvasPos);
		if (!this.IsStop || this.isOneDraw)
		{
			this.motionMgr.updateParam(this.live2DModelUnity);
			this.live2DModelUnity.update();
			this.live2DModelUnity.draw();
			this.isDrawed = true;
			if (this.motionMgr.isFinished() || this.isOneDraw)
			{
				this.isOneDraw = false;
			}
		}
	}

	public void Play()
	{
		this.myCamera.set_enabled(true);
		this.FinishCoroutineStop();
		this.motionMgr.startMotion(this.motion, false);
		this.IsStop = false;
		if (this.live2DModelUnity != null)
		{
			this.motionMgr.updateParam(this.live2DModelUnity);
			this.live2DModelUnity.update();
		}
		if (!this.motion.isLoop())
		{
			this.IEnum_MotionFinish = this.MotionFinishedStop(this.motion);
			base.StartCoroutine(this.IEnum_MotionFinish);
		}
	}

	public void PlayOnce()
	{
		this.Play();
		this.isOnePlay = true;
	}

	public void Play(Live2DModel.MotionType type, Action Onfinished)
	{
		this.ChangeMotion(type);
		this.StopAction = Onfinished;
		this.Play();
	}

	public void PlayOnce(Live2DModel.MotionType type, Action Onfinished)
	{
		this.ChangeMotion(type);
		this.StopAction = Onfinished;
		this.Play();
		this.isOnePlay = true;
	}

	public void forceStop()
	{
		this.myCamera.set_enabled(false);
		this.motionMgr.stopAllMotions();
		this.IsStop = true;
		this.FinishCoroutineStop();
	}

	private void FinishCoroutineStop()
	{
		if (this.IEnum_MotionFinish != null)
		{
			base.StopCoroutine(this.IEnum_MotionFinish);
			this.IEnum_MotionFinish = null;
		}
	}

	[DebuggerHidden]
	public IEnumerator MotionFinishedStop(Live2DMotion motion)
	{
		Live2DModel.<MotionFinishedStop>c__Iterator35 <MotionFinishedStop>c__Iterator = new Live2DModel.<MotionFinishedStop>c__Iterator35();
		<MotionFinishedStop>c__Iterator.motion = motion;
		<MotionFinishedStop>c__Iterator.<$>motion = motion;
		<MotionFinishedStop>c__Iterator.<>f__this = this;
		return <MotionFinishedStop>c__Iterator;
	}

	public void ChangeMotion(Live2DModel.MotionType type)
	{
		string motionPath = this.getMotionPath(type);
		TextAsset textAsset = ResourceManager.LoadResourceOrAssetBundle(motionPath) as TextAsset;
		if (textAsset == null)
		{
			return;
		}
		this.motionFile = textAsset;
		this.ChangeMotion(type, this.motionFile);
	}

	private void ChangeMotion(Live2DModel.MotionType type, TextAsset motionFile)
	{
		this.motion = Live2DMotion.loadMotion(motionFile.get_bytes());
		bool loop = type == Live2DModel.MotionType.Loop;
		this.motion.setLoop(loop);
		this.NowMotion = type;
		this.prevMotionFile = motionFile;
	}

	public Texture ChangeCharacter(int MstID, bool isDamaged)
	{
		return this.ChangeCharacter(MstID, isDamaged, -1);
	}

	public Texture ChangeCharacter(Live2DModelUnity Live2D, ShipModel Ship)
	{
		this.motionMgr.stopAllMotions();
		this.NowMstID = Ship.MstId;
		int resourceMstId = Utils.GetResourceMstId(this.NowMstID);
		this.live2DModelUnity = Live2D;
		this.motionFile = (ResourceManager.LoadResourceOrAssetBundle(string.Concat(new object[]
		{
			"Live2D/",
			resourceMstId,
			"/",
			resourceMstId,
			"_loop.mtn"
		})) as TextAsset);
		this.motion = Live2DMotion.loadMotion(this.motionFile.get_bytes());
		this.motion.setLoop(true);
		this.motionMgr.startMotion(this.motion, false);
		this.isDrawed = false;
		this.Play();
		this.isLive2DChange = !this.isLive2DModel;
		this.isLive2DModel = true;
		return this.myCamera.get_targetTexture();
	}

	public Texture ChangeCharacter(int MstID, bool isDamaged, int DeckID)
	{
		this.motionMgr.stopAllMotions();
		int resourceMstId = Utils.GetResourceMstId(MstID);
		string text = string.Concat(new object[]
		{
			"Live2D/",
			resourceMstId,
			"/",
			resourceMstId,
			".moc"
		});
		TextAsset textAsset = Resources.Load(text) as TextAsset;
		if (textAsset != null && !isDamaged)
		{
			this.mocFile = textAsset;
			for (int i = 0; i < 4; i++)
			{
				if (this.textureFiles[i] != null)
				{
					Resources.UnloadAsset(this.textureFiles[i]);
				}
				this.textureFiles[i] = null;
				this.textureFiles[i] = Resources.Load<Texture2D>(string.Concat(new object[]
				{
					"Live2D/",
					resourceMstId,
					"/texture_0",
					i
				}));
			}
			this.NowMstID = MstID;
			this.motionFile = (ResourceManager.LoadResourceOrAssetBundle(string.Concat(new object[]
			{
				"Live2D/",
				resourceMstId,
				"/",
				resourceMstId,
				"_loop.mtn"
			})) as TextAsset);
			this.setModel(null);
			this.isDrawed = false;
			this.Play();
			this.isLive2DChange = !this.isLive2DModel;
			this.isLive2DModel = true;
			return this.myCamera.get_targetTexture();
		}
		int texNum = (!isDamaged) ? 9 : 10;
		this.isLive2DChange = this.isLive2DModel;
		this.isLive2DModel = false;
		if (this.live2DModelUnity != null)
		{
			this.live2DModelUnity.releaseModel();
			this.live2DModelUnity = null;
		}
		return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(resourceMstId, texNum);
	}

	private void setModel()
	{
		this.setModel(null);
	}

	private void setModel(Live2DModelUnity LModel)
	{
		if (this.live2DModelUnity != null)
		{
			if (this.isDontRelease)
			{
				this.live2DModelUnity = null;
			}
			else
			{
				this.live2DModelUnity.releaseModel();
			}
		}
		if (LModel != null)
		{
			this.live2DModelUnity = LModel;
		}
		else
		{
			this.live2DModelUnity = Live2DModelUnity.loadModel(this.mocFile.get_bytes());
		}
		for (int i = 0; i < this.textureFiles.Length; i++)
		{
			this.live2DModelUnity.setTexture(i, this.textureFiles[i]);
		}
		this.modelW = this.live2DModelUnity.getCanvasWidth();
		this.modelH = this.live2DModelUnity.getCanvasHeight();
		this.live2DCanvasPos = Matrix4x4.Ortho(0f, this.modelW, this.modelH, 0f, -50f, 50f);
		this.aspect = this.modelH / this.modelW;
		this.motion = Live2DMotion.loadMotion(this.motionFile.get_bytes());
		this.motion.setLoop(true);
		this.motionMgr.startMotion(this.motion, false);
	}

	public void Enable()
	{
		this.myCamera.set_enabled(true);
		if (this.isLive2DModel)
		{
			this.Play();
		}
	}

	public void Disable()
	{
		this.forceStop();
		this.myCamera.set_enabled(false);
	}

	private string getMotionPath(Live2DModel.MotionType type)
	{
		if (type < (Live2DModel.MotionType)this.motionName.Length)
		{
			return string.Concat(new object[]
			{
				"Live2D/",
				this.NowMstID,
				"/",
				this.NowMstID,
				this.motionName[(int)type]
			});
		}
		int num = 0;
		string result = string.Empty;
		switch (type)
		{
		case Live2DModel.MotionType.Dislike1:
			num = Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.NowMstID).Motion1;
			if (num > 1000)
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					this.NowMstID,
					"/",
					num,
					this.motionName[0]
				});
			}
			else
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					num,
					"/",
					num,
					this.motionName[0]
				});
			}
			break;
		case Live2DModel.MotionType.Dislike2:
			num = Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.NowMstID).Motion2;
			if (num > 1000)
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					this.NowMstID,
					"/",
					num,
					this.motionName[0]
				});
			}
			else
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					num,
					"/",
					num,
					this.motionName[0]
				});
			}
			break;
		case Live2DModel.MotionType.Love1:
			num = Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.NowMstID).Motion3;
			if (num > 1000)
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					this.NowMstID,
					"/",
					num,
					this.motionName[0]
				});
			}
			else
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					num,
					"/",
					num,
					this.motionName[0]
				});
			}
			break;
		case Live2DModel.MotionType.Love2:
			num = Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.NowMstID).Motion4;
			if (num > 1000)
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					this.NowMstID,
					"/",
					num,
					this.motionName[0]
				});
			}
			else
			{
				result = string.Concat(new object[]
				{
					"Live2D/",
					num,
					"/",
					num,
					this.motionName[0]
				});
			}
			break;
		}
		if (num == 0)
		{
			Debug.LogWarning("���[�V�����}�X�^�ɓo�^�����Ă��܂���");
			return string.Concat(new object[]
			{
				"Live2D/",
				this.NowMstID,
				"/",
				this.NowMstID,
				this.motionName[0]
			});
		}
		return result;
	}

	private void NextMotion()
	{
		int num = (int)this.NowMotion;
		num = (int)Util.LoopValue(num + 1, 0f, 7f);
		this.NowMotion = (Live2DModel.MotionType)num;
		this.ChangeMotion(this.NowMotion);
		this.isOnePlay = true;
		this.Play();
	}

	private void RandomMotion()
	{
		int num = 0;
		do
		{
			int num2 = UnityEngine.Random.Range(1, 500);
			this.motionFile = Resources.Load<TextAsset>(string.Concat(new object[]
			{
				"Live2D/",
				num2,
				"/",
				num2,
				this.DebugMotionNameList[(int)this.RandomMotionType]
			}));
			if (this.motionFile != null)
			{
				break;
			}
			num++;
		}
		while (num < 200);
		this.prevMotionFile = this.motionFile;
		this.PlayDebug();
	}

	public Texture DebugChange()
	{
		for (int i = 1; i < 200; i++)
		{
			if (Mst_DataManager.Instance.Mst_ship.ContainsKey(this.NowMstID))
			{
				ShipModelMst shipModelMst = new ShipModelMst(this.NowMstID);
			}
			string text = string.Concat(new object[]
			{
				"Live2D/",
				this.NowMstID + i,
				"/",
				this.NowMstID + i,
				".moc"
			});
			TextAsset textAsset = Resources.Load(text) as TextAsset;
			if (textAsset != null)
			{
				this.NowMstID += i;
				break;
			}
			if (i == 199)
			{
				this.NowMstID = 1;
			}
		}
		return this.ChangeCharacter(this.NowMstID, false);
	}

	private void OnDestroy()
	{
		Mem.DelAry<Texture2D>(ref this.textureFiles);
		if (this.live2DModelUnity != null)
		{
			this.live2DModelUnity.releaseModel();
		}
		this.live2DModelUnity = null;
		this.motionMgr = null;
		this.motionFile = null;
		this.mocFile = null;
		this.textureFiles = null;
		this.motion = null;
	}

	public void PlayDebug()
	{
		this.ChangeMotion(this.NowMotion);
		this.Play();
	}

	public void PlayDebugPrevMotion()
	{
		this.ChangeMotion(Live2DModel.MotionType.Port, this.prevMotionFile);
		this.isOnePlay = true;
		this.Play();
	}

	public void DebugChangeMotion()
	{
		string text = string.Concat(new object[]
		{
			"Live2D/",
			this.DebugMstID,
			"/",
			this.DebugMstID,
			this.motionName[(int)this.RandomMotionType]
		});
		TextAsset textAsset = Resources.Load<TextAsset>(text);
		if (textAsset == null)
		{
			return;
		}
		this.motionFile = textAsset;
		this.ChangeMotion(this.RandomMotionType, this.motionFile);
		this.isOnePlay = true;
		this.Play();
	}

	public void DestroyCache()
	{
		this.live2DModelUnity = null;
	}

	public Live2DModelUnity CreateLive2DModelUnity(int MstID)
	{
		Texture2D[] array = new Texture2D[4];
		int resourceMstId = Utils.GetResourceMstId(MstID);
		string text = string.Concat(new object[]
		{
			"Live2D/",
			resourceMstId,
			"/",
			resourceMstId,
			".moc"
		});
		TextAsset textAsset = Resources.Load(text) as TextAsset;
		Live2DModelUnity live2DModelUnity = Live2DModelUnity.loadModel(textAsset.get_bytes());
		for (int i = 0; i < 4; i++)
		{
			array[i] = Resources.Load<Texture2D>(string.Concat(new object[]
			{
				"Live2D/",
				MstID,
				"/texture_0",
				i
			}));
		}
		for (int j = 0; j < this.textureFiles.Length; j++)
		{
			live2DModelUnity.setTexture(j, array[j]);
		}
		this.modelW = live2DModelUnity.getCanvasWidth();
		this.modelH = live2DModelUnity.getCanvasHeight();
		this.live2DCanvasPos = Matrix4x4.Ortho(0f, this.modelW, this.modelH, 0f, -50f, 50f);
		this.aspect = this.modelH / this.modelW;
		return live2DModelUnity;
	}
}

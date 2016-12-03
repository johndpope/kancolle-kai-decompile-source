using System;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class AllocMem : SingletonMonoBehaviour<AllocMem>
{
	public bool isShow = true;

	public bool isShowFPS;

	public bool isShowInEditor;

	private float fLastCollect;

	private float fLastCollectNum;

	private float fDelta;

	private float fLastDeltaTime;

	private int nAllocRate;

	private int nLastAllocMemory;

	private float fLastAllocSet = -9999f;

	private int nAllocMem;

	private int nCollectAlloc;

	private int nPeakAlloc;

	protected override void Awake()
	{
		base.Awake();
	}

	public void Start()
	{
		base.set_useGUILayout(false);
	}

	public void OnGUI()
	{
		if (!this.isShow || (!Application.get_isPlaying() && !this.isShowInEditor))
		{
			return;
		}
		int num = GC.CollectionCount(0);
		if (this.fLastCollectNum != (float)num)
		{
			this.fLastCollectNum = (float)num;
			this.fDelta = Time.get_realtimeSinceStartup() - this.fLastCollect;
			this.fLastCollect = Time.get_realtimeSinceStartup();
			this.fLastDeltaTime = Time.get_deltaTime();
			this.nCollectAlloc = this.nAllocMem;
		}
		this.nAllocMem = (int)GC.GetTotalMemory(false);
		this.nPeakAlloc = ((this.nAllocMem <= this.nPeakAlloc) ? this.nPeakAlloc : this.nAllocMem);
		if (Time.get_realtimeSinceStartup() - this.fLastAllocSet > 0.3f)
		{
			int num2 = this.nAllocMem - this.nLastAllocMemory;
			this.nLastAllocMemory = this.nAllocMem;
			this.fLastAllocSet = Time.get_realtimeSinceStartup();
			if (num2 >= 0)
			{
				this.nAllocRate = num2;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[Currently allocated]\t");
		stringBuilder.Append(((float)this.nAllocMem / 1000000f).ToString("0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("[Peak allocated]\t");
		stringBuilder.Append(((float)this.nPeakAlloc / 1000000f).ToString("0"));
		stringBuilder.Append("mb (last\tcollect ");
		stringBuilder.Append(((float)this.nCollectAlloc / 1000000f).ToString("0"));
		stringBuilder.Append(" mb)\n");
		stringBuilder.Append("[Allocation rate]\t");
		stringBuilder.Append(((float)this.nAllocRate / 1000000f).ToString("0.0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("[Collection frequency]\t");
		stringBuilder.Append(this.fDelta.ToString("0.00"));
		stringBuilder.Append("s\n");
		stringBuilder.Append("[Last collect delta]\t");
		stringBuilder.Append(this.fLastDeltaTime.ToString("0.000"));
		stringBuilder.Append("s (");
		stringBuilder.Append((1f / this.fLastDeltaTime).ToString("0.0"));
		stringBuilder.Append(" fps)");
		stringBuilder.Append("mb\n");
		stringBuilder.Append(string.Format("[UseMemory]{0}/{1}MB", Profiler.GetTotalAllocatedMemory() / 1048576u, Profiler.GetTotalReservedMemory() / 1048576u));
		stringBuilder.Append("\n");
		stringBuilder.Append(string.Format("[UnUseMemory]{0}MB", Profiler.GetTotalUnusedReservedMemory() / 1048576u));
		if (this.isShowFPS)
		{
			stringBuilder.Append("\n" + (1f / Time.get_deltaTime()).ToString("0.0") + " fps");
		}
		GUI.Box(new Rect(2f, 5f, 310f, (float)(95 + ((!this.isShowFPS) ? 20 : 36))), string.Empty);
		GUI.Label(new Rect(10f, 5f, 1000f, 200f), stringBuilder.ToString());
	}

	public string GetText()
	{
		if (!this.isShow || (!Application.get_isPlaying() && !this.isShowInEditor))
		{
			return string.Empty;
		}
		int num = GC.CollectionCount(0);
		if (this.fLastCollectNum != (float)num)
		{
			this.fLastCollectNum = (float)num;
			this.fDelta = Time.get_realtimeSinceStartup() - this.fLastCollect;
			this.fLastCollect = Time.get_realtimeSinceStartup();
			this.fLastDeltaTime = Time.get_deltaTime();
			this.nCollectAlloc = this.nAllocMem;
		}
		this.nAllocMem = (int)GC.GetTotalMemory(false);
		this.nPeakAlloc = ((this.nAllocMem <= this.nPeakAlloc) ? this.nPeakAlloc : this.nAllocMem);
		if (Time.get_realtimeSinceStartup() - this.fLastAllocSet > 0.3f)
		{
			int num2 = this.nAllocMem - this.nLastAllocMemory;
			this.nLastAllocMemory = this.nAllocMem;
			this.fLastAllocSet = Time.get_realtimeSinceStartup();
			if (num2 >= 0)
			{
				this.nAllocRate = num2;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Currently allocated\t\t\t");
		stringBuilder.Append(((float)this.nAllocMem / 1000000f).ToString("0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("Peak allocated\t\t\t\t");
		stringBuilder.Append(((float)this.nPeakAlloc / 1000000f).ToString("0"));
		stringBuilder.Append("mb (last\tcollect ");
		stringBuilder.Append(((float)this.nCollectAlloc / 1000000f).ToString("0"));
		stringBuilder.Append(" mb)\n");
		stringBuilder.Append("Allocation rate\t\t\t\t");
		stringBuilder.Append(((float)this.nAllocRate / 1000000f).ToString("0.0"));
		stringBuilder.Append("mb\n");
		stringBuilder.Append("Collection frequency\t\t");
		stringBuilder.Append(this.fDelta.ToString("0.00"));
		stringBuilder.Append("s\n");
		stringBuilder.Append("Last collect delta\t\t\t");
		stringBuilder.Append(this.fLastDeltaTime.ToString("0.000"));
		stringBuilder.Append("s (");
		stringBuilder.Append((1f / this.fLastDeltaTime).ToString("0.0"));
		stringBuilder.Append(" fps)");
		if (this.isShowFPS)
		{
			stringBuilder.Append("\n" + (1f / Time.get_deltaTime()).ToString("0.0") + " fps");
		}
		GUI.Box(new Rect((float)(Screen.get_width() / 2), 5f, 310f, (float)(80 + ((!this.isShowFPS) ? 0 : 16))), string.Empty);
		GUI.Label(new Rect(10f, 5f, 1000f, 200f), stringBuilder.ToString());
		return stringBuilder.ToString();
	}
}

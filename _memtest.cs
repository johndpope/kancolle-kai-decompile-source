using System;
using UnityEngine;

public class _memtest : MonoBehaviour
{
	private int update_count;

	private int always;

	private UILabel actLabel;

	private string text;

	private void Start()
	{
		this.update_count = 0;
		this.always = 0;
		this.actLabel = GameObject.Find("actLabel").GetComponent<UILabel>();
		this.text = string.Format("[LOG]\tMonoUsed\tMonoSize\tMonoUsed(%)\tTotalUsed\tTotalSize\tTotalUsed(%)\tUnUse\n", new object[0]);
		Debug.Log(this.text);
	}

	private void Update()
	{
		if (this.always++ % 2 == 0)
		{
			this.actLabel.SetActive(false);
		}
		else
		{
			this.actLabel.SetActive(true);
		}
		if (this.update_count++ < 10)
		{
			return;
		}
		this.update_count = 0;
		uint monoUsedSize = Profiler.GetMonoUsedSize();
		uint monoHeapSize = Profiler.GetMonoHeapSize();
		uint totalAllocatedMemory = Profiler.GetTotalAllocatedMemory();
		uint totalReservedMemory = Profiler.GetTotalReservedMemory();
		uint totalUnusedReservedMemory = Profiler.GetTotalUnusedReservedMemory();
		this.text = string.Format("Mono :{0}/{1} kb({2:f1}%)\nTotal:{3}/{4} kb({5:f1}%)\nUnUse:{6}kb\n", new object[]
		{
			monoUsedSize / 1024u,
			monoHeapSize / 1024u,
			100.0 * monoUsedSize / monoHeapSize,
			totalAllocatedMemory / 1024u,
			totalReservedMemory / 1024u,
			100.0 * totalAllocatedMemory / totalReservedMemory,
			totalUnusedReservedMemory / 1024u
		});
		base.get_gameObject().GetComponent<UILabel>().text = this.text;
		this.text = string.Format("{0}\t{1}\t{2:f1}%\t{3}\t{4}\t{5:f1}%\t{6}\n", new object[]
		{
			monoUsedSize / 1024u,
			monoHeapSize / 1024u,
			100.0 * monoUsedSize / monoHeapSize,
			totalAllocatedMemory / 1024u,
			totalReservedMemory / 1024u,
			100.0 * totalAllocatedMemory / totalReservedMemory,
			totalUnusedReservedMemory / 1024u
		});
		Debug.Log("[LOG]\t" + this.text);
	}
}

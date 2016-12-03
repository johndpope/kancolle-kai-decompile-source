using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mem
{
	public static void ZeroMemory<T>(IList<T> destination)
	{
		for (int i = 0; i < destination.get_Count(); i++)
		{
			destination.set_Item(i, default(T));
		}
	}

	public static void ZeroMemory<T>(T[] o, int offset, int length)
	{
		for (int i = 0; i < length; i++)
		{
			o[offset + i] = default(T);
		}
	}

	public static void Del<T>(ref T p)
	{
		p = default(T);
	}

	public static void Del(ref UISprite p)
	{
		if (p != null)
		{
			p.Clear();
		}
		p = null;
	}

	public static void Del(ref ParticleSystem p)
	{
		if (p != null)
		{
			Renderer component = p.GetComponent<Renderer>();
			if (component != null)
			{
				Material[] materials = component.get_materials();
				if (materials != null)
				{
					for (int i = 0; i < materials.Length; i++)
					{
						materials[i] = null;
					}
				}
				component.set_material(null);
			}
			Object.Destroy(p);
		}
		p = null;
	}

	public static void DelAry<T>(ref T[] p)
	{
		p = null;
	}

	public static void DelAry<T>(ref T[,] p)
	{
		p = null;
	}

	public static void DelList<T>(ref List<T> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelQueue<T>(ref Queue<T> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelDictionary<T, V>(ref Dictionary<T, V> p)
	{
		p.Clear();
		p = null;
	}

	public static void DelHashtable(ref Hashtable p)
	{
		p.Clear();
		p = null;
	}

	public static void DelComponent<T>(ref T p) where T : Component
	{
		Object.Destroy(p.get_gameObject());
		Mem.Del<T>(ref p);
	}

	public static void DelIDisposable<T>(ref T p) where T : IDisposable
	{
		p.Dispose();
		Mem.Del<T>(ref p);
	}

	public static void DelSafe<T>(ref T p)
	{
		if (p != null)
		{
			Mem.Del<T>(ref p);
			p = default(T);
		}
	}

	public static void DelArySafe<T>(ref T[] p)
	{
		if (p != null)
		{
			Mem.DelAry<T>(ref p);
			p = null;
		}
	}

	public static void DelListSafe<T>(ref List<T> p)
	{
		if (p != null)
		{
			Mem.DelList<T>(ref p);
		}
	}

	public static void DelQueueSafe<T>(ref Queue<T> p)
	{
		if (p != null)
		{
			Mem.DelQueue<T>(ref p);
		}
	}

	public static void DelDictionarySafe<T, V>(ref Dictionary<T, V> p)
	{
		if (p != null)
		{
			Mem.DelDictionary<T, V>(ref p);
		}
	}

	public static void DelHashtableSafe(ref Hashtable p)
	{
		if (p != null)
		{
			Mem.DelHashtable(ref p);
		}
	}

	public static void DelComponentSafe<T>(ref T p) where T : Component
	{
		if (p != null && p.get_gameObject() != null)
		{
			Mem.DelComponent<T>(ref p);
		}
		else
		{
			Mem.Del<T>(ref p);
		}
	}

	public static void DelIDisposableSafe<T>(ref T p) where T : IDisposable
	{
		if (p != null)
		{
			Mem.DelIDisposable<T>(ref p);
		}
	}

	public static void DelMeshSafe(ref Transform p)
	{
		if (p != null)
		{
			if (p.GetComponent<MeshFilter>() != null)
			{
				Mesh mesh = p.GetComponent<MeshFilter>().get_mesh();
				if (mesh != null)
				{
				}
			}
			MeshRenderer component = p.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.get_material() != null)
				{
					component.get_material().set_mainTexture(null);
					component.get_material().set_shader(null);
					Object.DestroyImmediate(component.get_material(), true);
					component.set_material(null);
				}
				if (component.get_materials() != null)
				{
					for (int i = 0; i < component.get_materials().Length; i++)
					{
						component.get_materials()[i].set_mainTexture(null);
						component.get_materials()[i].set_shader(null);
						Object.DestroyImmediate(component.get_materials()[i], true);
						component.get_materials()[i] = null;
					}
				}
			}
		}
	}

	public static void DelSkyboxSafe(ref Skybox p)
	{
		if (p != null && p.get_material() != null)
		{
			p.set_material(null);
		}
		Mem.Del<Skybox>(ref p);
	}

	public static void New<T>(ref object p) where T : new()
	{
		p = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
	}

	public static void NewAry<T>(ref T[] p, int n)
	{
		p = new T[n];
	}
}

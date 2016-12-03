using System;
using UnityEngine;
using UnityEngine.PSVita;

public class TestTouches : MonoBehaviour
{
	private GUIText gui;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.gui)
		{
			GameObject gameObject = new GameObject("Touch Info");
			gameObject.AddComponent<GUIText>();
			gameObject.set_hideFlags(61);
			gameObject.get_transform().set_position(new Vector3(0.1f, 0.5f, 0f));
			this.gui = gameObject.GetComponent<GUIText>();
			this.gui.set_pixelOffset(new Vector2(5f, 100f));
		}
		PSVitaInput.set_secondaryTouchIsScreenSpace(true);
		this.gui.set_text("\n\n\n\n\n\n\n\nSimulated Mouse\n");
		GUIText expr_8B = this.gui;
		string text = expr_8B.get_text();
		expr_8B.set_text(string.Concat(new object[]
		{
			text,
			" pos: ",
			Input.get_mousePosition().x,
			", ",
			Input.get_mousePosition().y,
			"\n"
		}));
		for (int i = 0; i < 3; i++)
		{
			GUIText expr_F9 = this.gui;
			expr_F9.set_text(expr_F9.get_text() + " button: " + i);
			GUIText expr_11A = this.gui;
			expr_11A.set_text(expr_11A.get_text() + " held: " + Input.GetMouseButton(i));
			GUIText expr_140 = this.gui;
			expr_140.set_text(expr_140.get_text() + " up: " + Input.GetMouseButtonUp(i));
			GUIText expr_166 = this.gui;
			expr_166.set_text(expr_166.get_text() + " down: " + Input.GetMouseButtonDown(i));
			GUIText expr_18C = this.gui;
			expr_18C.set_text(expr_18C.get_text() + "\n");
		}
		GUIText expr_1B2 = this.gui;
		expr_1B2.set_text(expr_1B2.get_text() + "\nTouch Screen Front");
		GUIText expr_1CD = this.gui;
		text = expr_1CD.get_text();
		expr_1CD.set_text(string.Concat(new object[]
		{
			text,
			"\n touchCount: ",
			Input.get_touchCount(),
			"\n"
		}));
		Touch[] touches = Input.get_touches();
		for (int j = 0; j < touches.Length; j++)
		{
			Touch touch = touches[j];
			GUIText expr_229 = this.gui;
			text = expr_229.get_text();
			expr_229.set_text(string.Concat(new object[]
			{
				text,
				" pos: ",
				touch.get_position().x,
				", ",
				touch.get_position().y
			}));
			GUIText expr_28C = this.gui;
			text = expr_28C.get_text();
			expr_28C.set_text(string.Concat(new object[]
			{
				text,
				" mp: ",
				Input.get_mousePosition().x,
				", ",
				Input.get_mousePosition().y
			}));
			GUIText expr_2EB = this.gui;
			expr_2EB.set_text(expr_2EB.get_text() + " fid: " + touch.get_fingerId());
			GUIText expr_312 = this.gui;
			expr_312.set_text(expr_312.get_text() + " dpos: " + touch.get_deltaPosition());
			GUIText expr_339 = this.gui;
			expr_339.set_text(expr_339.get_text() + " dtime: " + touch.get_deltaTime());
			GUIText expr_360 = this.gui;
			expr_360.set_text(expr_360.get_text() + " tapcount: " + touch.get_tapCount());
			GUIText expr_387 = this.gui;
			expr_387.set_text(expr_387.get_text() + " phase: " + touch.get_phase());
			GUIText expr_3AE = this.gui;
			expr_3AE.set_text(expr_3AE.get_text() + "\n");
			if (touch.get_phase() == null)
			{
				MonoBehaviour.print("Began");
			}
			if (touch.get_phase() == 3)
			{
				if (touch.get_tapCount() == 2)
				{
					MonoBehaviour.print("Ended Multitap");
				}
				else
				{
					MonoBehaviour.print("Ended");
				}
			}
		}
		GUIText expr_422 = this.gui;
		expr_422.set_text(expr_422.get_text() + "\nRear Touch Pad");
		GUIText expr_43D = this.gui;
		expr_43D.set_text(expr_43D.get_text() + "\n isScreenSpace: " + PSVitaInput.get_secondaryTouchIsScreenSpace());
		if (!PSVitaInput.get_secondaryTouchIsScreenSpace())
		{
			GUIText expr_46C = this.gui;
			expr_46C.set_text(expr_46C.get_text() + "\n width: " + PSVitaInput.get_secondaryTouchWidth());
			GUIText expr_491 = this.gui;
			expr_491.set_text(expr_491.get_text() + " height: " + PSVitaInput.get_secondaryTouchHeight());
		}
		GUIText expr_4B6 = this.gui;
		text = expr_4B6.get_text();
		expr_4B6.set_text(string.Concat(new object[]
		{
			text,
			"\n touchCount: ",
			PSVitaInput.get_touchCountSecondary(),
			"\n"
		}));
		Touch[] touchesSecondary = PSVitaInput.get_touchesSecondary();
		for (int k = 0; k < touchesSecondary.Length; k++)
		{
			Touch touch2 = touchesSecondary[k];
			GUIText expr_515 = this.gui;
			text = expr_515.get_text();
			expr_515.set_text(string.Concat(new object[]
			{
				text,
				" pos: ",
				touch2.get_position().x,
				", ",
				touch2.get_position().y
			}));
			GUIText expr_578 = this.gui;
			expr_578.set_text(expr_578.get_text() + " fid: " + touch2.get_fingerId());
			GUIText expr_59F = this.gui;
			expr_59F.set_text(expr_59F.get_text() + " dpos: " + touch2.get_deltaPosition());
			GUIText expr_5C6 = this.gui;
			expr_5C6.set_text(expr_5C6.get_text() + " dtime: " + touch2.get_deltaTime());
			GUIText expr_5ED = this.gui;
			expr_5ED.set_text(expr_5ED.get_text() + " tapcount: " + touch2.get_tapCount());
			GUIText expr_614 = this.gui;
			expr_614.set_text(expr_614.get_text() + " phase: " + touch2.get_phase());
			GUIText expr_63B = this.gui;
			expr_63B.set_text(expr_63B.get_text() + "\n");
		}
	}
}

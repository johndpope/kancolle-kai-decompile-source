using System;
using UnityEngine;
using UnityEngine.PSVita;

public class ButtonInput : MonoBehaviour
{
	private void OnGUI()
	{
		int num = 40;
		GUI.Toggle(new Rect(85f, (float)num, 300f, 20f), PSVitaInput.WirelesslyControlled(), " Wireless Controller detected");
		num += 15;
		GUI.Toggle(new Rect(85f, (float)num, 300f, 20f), Input.GetButton("Left Shoulder"), " Left Shoulder");
		GUI.Toggle(new Rect(385f, (float)num, 300f, 20f), Input.GetButton("Right Shoulder"), " Right Shoulder");
		num += 15;
		GUI.Toggle(new Rect(85f, (float)num, 300f, 20f), Input.GetButton("Left Shoulder 1"), " Left Shoulder 1 (Vita TV Only)");
		GUI.Toggle(new Rect(385f, (float)num, 300f, 20f), Input.GetButton("Right Shoulder 1"), " Right Shoulder 1 (Vita TV Only)");
		num += 15;
		GUI.Toggle(new Rect(85f, (float)num, 300f, 20f), Input.GetButton("Left Stick"), " Left Stick (Vita TV Only)");
		GUI.Toggle(new Rect(385f, (float)num, 300f, 20f), Input.GetButton("Right Stick"), " Right Stick (Vita TV Only)");
		num += 30;
		int num2 = 80;
		int num3 = num;
		GUI.TextField(new Rect((float)num2, (float)num3, 140f, 20f), " D-Pad");
		GUI.Toggle(new Rect((float)num2, (float)(num3 + 60), 100f, 20f), Input.GetButton("Dleft"), " Left");
		GUI.Toggle(new Rect((float)(num2 + 40), (float)(num3 + 30), 100f, 20f), Input.GetButton("Dup"), " Up");
		GUI.Toggle(new Rect((float)(num2 + 80), (float)(num3 + 60), 100f, 20f), Input.GetButton("Dright"), " Right");
		GUI.Toggle(new Rect((float)(num2 + 40), (float)(num3 + 90), 100f, 20f), Input.GetButton("Ddown"), " Down");
		int num4 = 380;
		int num5 = num;
		GUI.TextField(new Rect((float)num4, (float)num5, 140f, 20f), " Main Buttons");
		GUI.Toggle(new Rect((float)num4, (float)(num5 + 60), 100f, 20f), Input.GetButton("Square"), " Square");
		GUI.Toggle(new Rect((float)(num4 + 40), (float)(num5 + 30), 100f, 20f), Input.GetButton("Triangle"), " Triangle");
		GUI.Toggle(new Rect((float)(num4 + 80), (float)(num5 + 60), 100f, 20f), Input.GetButton("Circle"), " Circle");
		GUI.Toggle(new Rect((float)(num4 + 40), (float)(num5 + 90), 100f, 20f), Input.GetButton("Cross"), " Cross");
		int num6 = 590;
		int num7 = num;
		GUI.Toggle(new Rect((float)num6, (float)num7, 140f, 20f), Input.GetButton("Select"), " Select");
		GUI.Toggle(new Rect((float)num6, (float)(num7 + 20), 140f, 20f), Input.GetButton("Start"), " Start");
		num += 120;
		int num8 = 50;
		int num9 = num;
		GUI.TextField(new Rect((float)num8, (float)num9, 200f, 20f), " Left Stick X Axis: " + Input.GetAxis("Left Stick Horizontal"));
		GUI.TextField(new Rect((float)num8, (float)(num9 + 25), 200f, 20f), " Left Stick Y Axis: " + Input.GetAxis("Left Stick Vertical"));
		int num10 = 350;
		int num11 = num;
		GUI.TextField(new Rect((float)num10, (float)num11, 200f, 20f), " Right Stick X Axis: " + Input.GetAxis("Right Stick Horizontal"));
		GUI.TextField(new Rect((float)num10, (float)(num11 + 25), 200f, 20f), " Right Stick Y Axis: " + Input.GetAxis("Right Stick Vertical"));
	}

	private void Update()
	{
	}
}

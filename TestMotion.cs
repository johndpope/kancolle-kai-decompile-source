using System;
using UnityEngine;
using UnityEngine.PSVita;

public class TestMotion : MonoBehaviour
{
	private GUIText gui;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.gui)
		{
			GameObject gameObject = new GameObject("Motion Info");
			gameObject.AddComponent<GUIText>();
			gameObject.set_hideFlags(61);
			gameObject.get_transform().set_position(new Vector3(0.7f, 0.9f, 0f));
			this.gui = gameObject.GetComponent<GUIText>();
		}
		this.gui.set_pixelOffset(new Vector2(0f, 0f));
		Input.get_compass().set_enabled(true);
		PSVitaInput.set_gyroTiltCorrectionEnabled(false);
		PSVitaInput.set_gyroDeadbandFilterEnabled(true);
		this.gui.set_text("\nInput");
		GUIText expr_9C = this.gui;
		expr_9C.set_text(expr_9C.get_text() + "\n .deviceOrientation: ");
		GUIText expr_B7 = this.gui;
		expr_B7.set_text(expr_B7.get_text() + "\n   " + Input.get_deviceOrientation());
		GUIText expr_DC = this.gui;
		expr_DC.set_text(expr_DC.get_text() + "\n\nInput.acceleration");
		GUIText expr_F7 = this.gui;
		expr_F7.set_text(expr_F7.get_text() + "\n .x,y,z: " + Input.get_acceleration());
		GUIText expr_11C = this.gui;
		expr_11C.set_text(expr_11C.get_text() + "\n .magnitude: " + Input.get_acceleration().get_magnitude());
		GUIText expr_149 = this.gui;
		expr_149.set_text(expr_149.get_text() + "\n\nInput.gyro");
		GUIText expr_164 = this.gui;
		expr_164.set_text(expr_164.get_text() + "\n .enabled: " + Input.get_gyro().get_enabled());
		GUIText expr_18E = this.gui;
		expr_18E.set_text(expr_18E.get_text() + "\n .attitude: " + Input.get_gyro().get_attitude());
		GUIText expr_1B8 = this.gui;
		expr_1B8.set_text(expr_1B8.get_text() + "\n .gravity: " + Input.get_gyro().get_gravity());
		GUIText expr_1E2 = this.gui;
		expr_1E2.set_text(expr_1E2.get_text() + "\n .rotationRate: " + Input.get_gyro().get_rotationRate());
		GUIText expr_20C = this.gui;
		expr_20C.set_text(expr_20C.get_text() + "\n .rotationRateUnbiased: " + Input.get_gyro().get_rotationRateUnbiased());
		GUIText expr_236 = this.gui;
		expr_236.set_text(expr_236.get_text() + "\n .updateInterval: " + Input.get_gyro().get_updateInterval());
		GUIText expr_260 = this.gui;
		expr_260.set_text(expr_260.get_text() + "\n .userAcceleration: " + Input.get_gyro().get_userAcceleration());
		GUIText expr_28A = this.gui;
		expr_28A.set_text(expr_28A.get_text() + "\nPSVitaInput.gyroDeadbandFilterEnabled: " + PSVitaInput.get_gyroDeadbandFilterEnabled());
		GUIText expr_2AF = this.gui;
		expr_2AF.set_text(expr_2AF.get_text() + "\nPSVitaInput.gyroTiltCorrectionEnabled: " + PSVitaInput.get_gyroTiltCorrectionEnabled());
		GUIText expr_2D4 = this.gui;
		expr_2D4.set_text(expr_2D4.get_text() + "\n\nInput.compass");
		GUIText expr_2EF = this.gui;
		expr_2EF.set_text(expr_2EF.get_text() + "\n .enabled: " + Input.get_compass().get_enabled());
		GUIText expr_319 = this.gui;
		expr_319.set_text(expr_319.get_text() + "\n .magneticHeading: " + Input.get_compass().get_magneticHeading());
		GUIText expr_343 = this.gui;
		expr_343.set_text(expr_343.get_text() + "\n .trueHeading: " + Input.get_compass().get_trueHeading());
		GUIText expr_36D = this.gui;
		expr_36D.set_text(expr_36D.get_text() + "\n .rawVector: " + Input.get_compass().get_rawVector());
		GUIText expr_397 = this.gui;
		expr_397.set_text(expr_397.get_text() + "\n .timestamp: " + Input.get_compass().get_timestamp());
		GUIText expr_3C1 = this.gui;
		expr_3C1.set_text(expr_3C1.get_text() + "\n PSVitaInput.compassFieldStability:");
		GUIText expr_3DC = this.gui;
		expr_3DC.set_text(expr_3DC.get_text() + "\n   " + PSVitaInput.get_compassFieldStability());
		if (PSVitaInput.get_compassFieldStability() != 2)
		{
			GUIText expr_40C = this.gui;
			expr_40C.set_text(expr_40C.get_text() + "\nCompass unstable, needs calibration!");
		}
	}
}

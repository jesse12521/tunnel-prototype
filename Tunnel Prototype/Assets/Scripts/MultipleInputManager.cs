using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultipleInputManager
{
	// Axis

	public static float Cust_Horizontal()
	{
		float h = 0.0f;
		h += Input.GetAxis ("Cust_JHorizontal");
		h += Input.GetAxis ("Cust_KHorizontal");

		return Mathf.Clamp (h, -1.0f, 1.0f);
	}

	public static float Cust_Vertical()
	{
		float v = 0.0f;
		v += Input.GetAxis ("Cust_JVertical");
		v += Input.GetAxis ("Cust_KVertical");

		return Mathf.Clamp (v, -1.0f, 1.0f);
	}

	public static Vector3 Cust_HV()
	{
		return new Vector3(Cust_Horizontal(), 0, Cust_Vertical());
	}


	public static float Cust_LHorizontal()
	{
		float h = 0.0f;
		h += Input.GetAxis ("Cust_LHorizontal");

		return Mathf.Clamp (h, -1.0f, 1.0f);
	}

	public static float Cust_LVertical()
	{
		float v = 0.0f;
		v += Input.GetAxis ("Cust_LVertical");

		return Mathf.Clamp (v, -1.0f, 1.0f);
	}

	public static Vector3 Cust_Look()
	{
		return new Vector3(Cust_LHorizontal(), Cust_LVertical(), 0);
	}

	// Buttons

	public static bool Cust_AButton()
	{
		bool a = false;
		a = Input.GetButtonDown ("Cust_AButton") || Input.GetMouseButtonDown (0);
		return a;
	}

	public static bool Cust_BButton()
	{
		return Input.GetButtonDown ("Cust_BButton");
	}

	public static bool Cust_XButton()
	{
		return Input.GetButtonDown ("Cust_XButton");
	}

	public static bool Cust_YButton()
	{
		return Input.GetButtonDown ("Cust_YButton");
	}

	public static bool Cust_RunButton()
	{
		return Input.GetButton ("Cust_RunButton");
	}

	public static bool Cust_MenuButton()
	{
		return Input.GetButtonDown ("Cust_MenuButton");
	}



	//Axis to work as Buttons
	public static bool Cust_Up()
	{
		float u = 0.0f;
		u = Input.GetAxis("Cust_Up");
		if (u > 0.0f || Input.GetKey(KeyCode.E)) {
			return true;
		}
		return false;
	}

	public static bool Cust_Down()
	{	
		float d = 0.0f;
		d = Input.GetAxis("Cust_Down");
		if (d > 0.0f || Input.GetKey(KeyCode.Q)) {
			return true;
		}
		return false;
	}
}

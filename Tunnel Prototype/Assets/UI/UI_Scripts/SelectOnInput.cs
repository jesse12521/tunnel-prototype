﻿using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;

	// Update is called once per frame
	void Update()
	{
		if ((Input.GetAxis("Cust_KVertical") != 0 || Input.GetAxis("Cust_JVertical") != 0) && buttonSelected == false)
		{
			eventSystem.SetSelectedGameObject(selectedObject);
			buttonSelected = true;
		}
	}

	private void OnDisable()
	{
		buttonSelected = false;
	}
}
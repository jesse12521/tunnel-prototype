
using UnityEngine;

public class MenuController : MonoBehaviour {

	public GameObject menu;
	public GameObject cam;
	//public GameObject reticule;
	private bool on = false;
	private CamController camscript;

	void Start()
	{
		camscript = cam.GetComponent("CamController") as CamController;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (on == false)
			{
				menu.SetActive(true);
				camscript.enabled = false;
				//reticule.SetActive(false);
				on = true;
			}
			else
			{
				menu.SetActive(false);
				camscript.enabled = true;
				//reticule.SetActive(true);
				on = false;
			}
		}
	}
}

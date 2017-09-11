
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour {

	#region Variables

	TextMeshProUGUI distText;
	public Camera cam;
	private CamController control;
	public float distVal = 0.0f;
	#endregion

	#region Unity Methods

	void Start () 
	{
		control = cam.GetComponent<CamController>();
		distVal = control.GetDist();
		distText = gameObject.GetComponent<TextMeshProUGUI>();
		distText.text = "Dist: " + distVal.ToString("F2") + " meters";
	}


	void Update () 
	{
		distVal = control.GetDist();
		distText.text = "Dist: " + distVal.ToString("F2") + " meters";
	}
	
	#endregion
	
}

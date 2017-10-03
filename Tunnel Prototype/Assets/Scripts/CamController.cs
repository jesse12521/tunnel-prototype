using UnityEngine;

public class CamController : MonoBehaviour
{
     //Have to hold right-click to rotate
     //Q/E keys are used to raise/lower the camera

	public float mainSpeed = 100.0f; //Regular speed
	public float shiftAdd = 250.0f; //Multiplied by how long shift is held.  Basically running
	public float maxShift = 1000.0f; //Maximum speed when holding shift



	private bool isRotating = false; 
	private float speedMultiplier; 

	public float mouseSensitivity = 0.1f;        // Mouse rotation sensitivity.
	public float joystickSensitivity = 0.1f;        // Joystick rotation sensitivity.
	private float rotationY = 0.0f;
	private float rotationX = 0.0f;
	
	//public TextMeshProUGUI distText;
	//public float distVal = 0.0f;
	public GameObject prefab;
	private GameObject[] nodeList;
	public int nodeIndex;
	public float dist = 0.0f;
	private float time = 0.0f;

	private Vector3 resetPosition;
	private Vector3 resetRotation;


	void Start()
	{
		//distText =  GetComponent<TextMeshProUGUI>();
		nodeList = new GameObject[2];
		nodeIndex = 0;
		resetPosition = transform.position;
		resetRotation = new Vector3(0.0f, 0.0f, 0.0f);
	}

	public float GetDist()
	{
		return dist;
	}

	void FixedUpdate()
	{
		time += Time.deltaTime;

		if (MultipleInputManager.Cust_AButton() && time > 0.8f)
		{
			time = 0.0f;
			Ray ray;
			RaycastHit hit;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(!Input.GetMouseButtonDown(0))
			{
				ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			}
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				if (hit.collider.name == "TunnelMesh" && nodeIndex < 2)
				{
					GameObject node = Instantiate(prefab, hit.point, Quaternion.identity);
					if(nodeList[nodeIndex] == null)
					{
						nodeList[nodeIndex] = node;
					}
					else
					{
						Destroy(nodeList[nodeIndex].gameObject);
						nodeList[nodeIndex] = node;
					}
					
					//nodeIndex = ((nodeIndex + 1) % 2);
					Debug.Log("it's going to add one");
					nodeIndex++;
					if(nodeIndex == 2)
					{
						nodeIndex = 0;
					}
				}
			}
			if (nodeList[0] != null && nodeList[1] != null)
			{
				dist = Vector3.Distance(nodeList[0].transform.position, nodeList[1].transform.position);
			}
		}

		//Hold right-mouse button to rotate
		if (Input.GetMouseButtonDown(1))
		{
			isRotating = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			isRotating = false;
		}
		if (isRotating)
		{
			rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X");// * mouseSensitivity;
			rotationY += Input.GetAxis ("Mouse Y");// * mouseSensitivity;
			rotationY = Mathf.Clamp(rotationY, -90, 90);
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
		}
		else
		{
			rotationX = transform.localEulerAngles.y + MultipleInputManager.Cust_Look().x;// * joystickSensitivity;
			rotationY += MultipleInputManager.Cust_Look ().y;// * joystickSensitivity;
			rotationY = Mathf.Clamp(rotationY, -90, 90);
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
		}

		//Keyboard commands
		Vector3 p = GetBaseInput();

		p = p * mainSpeed;
		speedMultiplier = mainSpeed * Time.deltaTime;
		p = p * Time.deltaTime;

		Vector3 newPosition = transform.position;
		transform.Translate(p);
		newPosition.x = transform.position.x;
		newPosition.z = transform.position.z;

		//Manipulate Y plane by using Q/E keys
		if (MultipleInputManager.Cust_Down())
		{
			newPosition.y -= speedMultiplier;
		}
		if (MultipleInputManager.Cust_Up())
		{
			newPosition.y += speedMultiplier;
		}
		
		transform.position = newPosition;


		if (MultipleInputManager.Cust_RunButton())
		{
			transform.position = resetPosition;
			transform.eulerAngles = resetRotation;
			rotationY = 0.0f;
		}
	}
	

	private Vector3 GetBaseInput()
	{
		//X and Z movement
		Vector3 p_Velocity = new Vector3();
		p_Velocity += MultipleInputManager.Cust_HV();
		
		return p_Velocity;
	}


	void OnCollisionEnter (Collision col)
	{
	Debug.Log("Enter called.");

		//move back when collision occurs
		if (col.gameObject.CompareTag("Tunnel"))
		{
			Vector3 dir = col.contacts[0].point - transform.position;
			dir = -dir.normalized;
			Vector3 moveBack = transform.position + dir / 2;
			transform.position = moveBack;
		}
	}

	void OnCollisionStay(Collision col)
	{
		Debug.Log("Stay occuring..");
	}

	void OnCollisionExit(Collision col)
	{
		Debug.Log("Exit called.");
	}
}

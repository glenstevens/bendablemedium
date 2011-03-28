using UnityEngine;
using System.Collections;

public class DiabloCameraController : MonoBehaviour {
	
	public Transform player;
	public float horizontalAngle = 39.2f;
	public float verticalAngle = -48.5f;
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -2.4f);
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f);
	public float maxCamDist = 1;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(horizontalAngle, verticalAngle, 0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null) 
			return;
		
		Quaternion camYRotation = Quaternion.Euler(0, horizontalAngle, 0);
		Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
		Vector3 farCamPoint = camYRotation * camOffset;
		float farDist = Vector3.Distance(farCamPoint, closeCamPoint);
		Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
		transform.position = closeCamPoint + closeToFarDir * maxCamDist;
	}
}

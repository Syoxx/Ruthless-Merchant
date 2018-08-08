using UnityEngine;
using System.Collections;

[AddComponentMenu("AFS/Touch Bending/Player Listener")]
public class touchBendingPlayerListener : MonoBehaviour {
	
	public float maxSpeed = 8.0f;
	public float Player_DampSpeed = 0.75f;
	
	private Transform myTransform;
	
	private Vector3 Player_Position;
	private Vector3 Player_OldPosition;
	public float Player_Speed = 0.0f;
	private float Player_NewSpeed = 0.0f;
	public Vector3 Player_Direction;

	public bool Update_PlayerVars = true;
	
	// Use component caching
	void Awake () {
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
	//	StartCoroutine( AfsPlayerDataUpdate() );
		Player_Position = transform.position;
		Player_OldPosition = Player_Position;
	}
	
	//IEnumerator AfsPlayerDataUpdate () {
	//	while (Update_PlayerVars) {
	//		yield return new WaitForEndOfFrame();

	void LateUpdate() {
			Player_Position = myTransform.position;
			// bring speed into 0-1 range. needs maxSpeed
			Player_NewSpeed = (Player_Position - Player_OldPosition).magnitude / (Time.deltaTime) / (maxSpeed);
			
			// damp speed
			var dampDecelerate = (1.0f - Mathf.Exp( -20.0f * Time.deltaTime ));
			var dampAccelerate = 0.25f * dampDecelerate;
			dampDecelerate *= 0.125f;

			if (Player_NewSpeed < Player_Speed) {
				Player_Speed = Mathf.Lerp( Player_Speed, Player_NewSpeed, dampDecelerate * Player_DampSpeed);
			}
			else {
				Player_Speed = Mathf.Lerp( Player_Speed, Player_NewSpeed, dampAccelerate * Player_DampSpeed);
			}
			
			// recalc player direction
			if (Player_Position != Player_OldPosition) {
				Player_Direction = Vector3.Normalize( Player_Position - Player_OldPosition);
			}
			// store old positions
			Player_OldPosition = Player_Position;
		}
	//}
}

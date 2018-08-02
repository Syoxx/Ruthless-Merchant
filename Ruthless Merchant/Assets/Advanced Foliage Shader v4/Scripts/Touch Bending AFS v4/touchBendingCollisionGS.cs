using UnityEngine;
using System.Collections;

[AddComponentMenu("AFS/Touch Bending/CollisionGS")]
public class touchBendingCollisionGS: MonoBehaviour {
	
	public Material simpleBendingMaterial;
	public Material touchBendingMaterial;
		
	public float stiffness = 10.0f;
	public float disturbance = 0.3f;
	public float duration = 5.0f;
	
	// Use component caching
	private Transform myTransform;
	private Renderer myRenderer;
	
	private Matrix4x4 myMatrix;
	
	private Vector3 axis;
	private Vector3 axis1;
		
	private bool touched = false;
	private bool doubletouched = false;
	
	private bool left = false;
	private bool finished = true;
	
	private bool left1 = false;
	private bool finished1 = true;
	
	private float intialTouchForce = 0.0f;
	private float touchBending = 0.0f;
	private float targetTouchBending = 0.0f;
	private float easingControl = 0.0f;
	
	private float intialTouchForce1 = 0.0f;
	private float touchBending1 = 0.0f;
	private float targetTouchBending1 = 0.0f;
	private float easingControl1 = 0.0f;

	private int Player_ID;
	private touchBendingPlayerListener PlayerVars;
	private Vector3 Player_Direction;
	private float Player_Speed;
	
	private int Player1_ID;
	private touchBendingPlayerListener PlayerVars1;
	private Vector3 Player_Direction1;
	private float Player_Speed1;
	
	private float timer = 0.0f;
	private float timer1 = 0.0f;
	
	private float lerptime = 0.0f;
	
	// Init component caching
	void Awake () {
		myTransform = transform;
		myRenderer = GetComponent<Renderer>();
	}
	
	void Start () {
		myRenderer.sharedMaterial = simpleBendingMaterial;
	}
	
	void OnTriggerEnter(Collider other) {
		touchBendingPlayerListener tempPlayerVars = other.GetComponent<touchBendingPlayerListener>();
		// register touch only if collider has the touchBendingPlayerListener script attached and enabled
		if( tempPlayerVars != null && tempPlayerVars.enabled) {
			// no other touch event registered
			if (!touched) {
				//register gameobject ID
				Player_ID = other.GetInstanceID();
				
				Destroy(myRenderer.material);
				
				PlayerVars = tempPlayerVars;
				Player_Direction = PlayerVars.Player_Direction;
				Player_Speed = PlayerVars.Player_Speed;
				intialTouchForce = Player_Speed;
				
				// instantiate mat
				myRenderer.material = touchBendingMaterial;
				myRenderer.material.SetVector("_TouchBendingPosition", new Vector4(myTransform.position.x, myTransform.position.y, myTransform.position.z, 0f) );
				
				axis = PlayerVars.Player_Direction;
				// rotate by 90
				axis = Quaternion.Euler(0,90,0) * axis;
				
				timer = 0.0f;
				touched = true;
				left = false;
				targetTouchBending = 1.0f;
				touchBending = targetTouchBending;
				finished = false;
			}
			else {
				/// 2nd+ touch: we simply drop the first registered collision
				if ( doubletouched == true ) {
					SwapTouchBending ();	
				}
				//register gameobject ID
				Player1_ID = other.GetInstanceID();
				
				PlayerVars1 = tempPlayerVars;
				Player_Direction1 = PlayerVars1.Player_Direction;
				Player_Speed1 = PlayerVars1.Player_Speed;
				intialTouchForce1 = Player_Speed1;
				
				axis1 = Player_Direction1;
				// rotate by 90
				axis1 = Quaternion.Euler(0,90,0) * axis1;
				
				timer1 = 0.0f;
				left1 = false;
				targetTouchBending1 = 1.0f;
				touchBending1 = targetTouchBending1;
				finished1 = false;
				lerptime = duration - timer;
				doubletouched = true;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(Player_ID != Player1_ID) {
			// which one do we have to set?
			if (other.GetInstanceID() == Player_ID) {
				left = true;
				targetTouchBending = 0.0f;
			}
			else {
				left1 = true;
				targetTouchBending1 = 0.0f;	
			}
		}
		else {
			left = true;
			targetTouchBending = 0.0f;	
			left1 = true;
			targetTouchBending1 = 0.0f;		
		}
	}
	
	void Update () {	
		if (touched) {
			// update speed Player 0
			Player_Speed = PlayerVars.Player_Speed;
			touchBending = Mathf.Lerp (touchBending, targetTouchBending, (timer) / duration );
			easingControl = Bounce( timer);
			
// a) just 1 touch event registered
			if (!doubletouched) {
				if (finished && targetTouchBending == 0.0f) {	
					ResetTouchBending ();	
				}
				else {
					// Calculate rotation matrix
					Quaternion rotation = Quaternion.Euler (axis * (intialTouchForce * stiffness) * easingControl );
					myMatrix.SetTRS( Vector3.zero, rotation, new Vector3(1,1,1) );
					myRenderer.material.SetMatrix ("_RotMatrix", myMatrix);
					// set extra force
					myRenderer.material.SetVector("_TouchBendingForce", new Vector4(Player_Direction.x, Player_Direction.y, Player_Direction.z, Player_Speed * easingControl * disturbance) );
					if (left) {
						timer += Time.deltaTime; // so if the player stops outside the collider this does not affect the animation
					}
					else {
						timer += Time.deltaTime * Player_Speed;	// but if the player stops inside it will
					}
				}
			}
// b) 2 touch events registered
			else {
// b.1) first touch bending animation has just ended
				if (finished && targetTouchBending == 0.0f) {	
					SwapTouchBending ();
					doubletouched = false;
					// update speed Player 0
					Player_Speed = PlayerVars.Player_Speed;
					touchBending = Mathf.Lerp (touchBending, targetTouchBending, (timer) / duration );
					easingControl = Bounce( timer);
					// Usually this will never happen… but you never know	
					if (finished && targetTouchBending == 0.0f) {
						// if second touch bending animation has ended at the same time
						ResetTouchBending ();	
					}
					else {
						// Calculate rotation matrix
						Quaternion rotation = Quaternion.Euler (axis * (intialTouchForce * stiffness) * easingControl );
						myMatrix.SetTRS( Vector3.zero, rotation, new Vector3(1,1,1) );
						myRenderer.material.SetMatrix ("_RotMatrix", myMatrix);
						// set extra force
						myRenderer.material.SetVector("_TouchBendingForce", new Vector4(Player_Direction.x, Player_Direction.y, Player_Direction.z, Player_Speed * easingControl * disturbance) );
						if (left) {
							timer += Time.deltaTime;
						}
						else {
							timer += Time.deltaTime * Player_Speed;	
						}
					}
				}
// b.2 ) calculate both animations
				else {
					// update speed Player 1
					Player_Speed1 = PlayerVars1.Player_Speed;
					touchBending1 = Mathf.Lerp (touchBending1, targetTouchBending1, timer1 / duration );
					easingControl1 = Bounce1( timer1 );
					// Usually this will never happen as touch event 1 should always end after touch event 0 … but you never know	
					if (finished1 && targetTouchBending1 == 0.0f) {
						doubletouched = false;	
					}
					else {
						// Calculate rotation matrix
						Quaternion rotation = Quaternion.Euler (axis * (intialTouchForce * stiffness) * easingControl );
						Quaternion rotation1 = Quaternion.Euler (axis1 * (intialTouchForce1 * stiffness) * easingControl1 );
						rotation = rotation * rotation1;
						myMatrix.SetTRS( Vector3.zero, rotation, new Vector3(1,1,1) );
						myRenderer.material.SetMatrix ("_RotMatrix", myMatrix);
						// set extra force
//						myRenderer.material.SetVector("_TouchBendingForce", Vector4.Lerp (new Vector4(Player_Direction.x, Player_Direction.y, Player_Direction.z, Player_Speed * easingControl * disturbance), new Vector4(Player_Direction1.x, Player_Direction1.y, Player_Direction1.z, Player_Speed1 * easingControl1 * disturbance), timer1 / (lerptime + 0.0001f) * 8.0f) );
						myRenderer.material.SetVector("_TouchBendingForce", Vector4.Lerp (new Vector4(Player_Direction.x, Player_Direction.y, Player_Direction.z, Player_Speed * easingControl1 * disturbance), new Vector4(Player_Direction1.x, Player_Direction1.y, Player_Direction1.z, Player_Speed1 * easingControl1 * disturbance), timer1 / (lerptime + 0.0001f) * 8.0f) );

						if (left) {
							timer += Time.deltaTime;
						}
						else {
							timer += Time.deltaTime * Player_Speed;	
						}
						if (left1) {
							timer1 += Time.deltaTime;
						}
						else {
							timer1 += Time.deltaTime * Player_Speed1;	
						}
					}
				}
			}
		}
	}
	
	public float Bounce(float x) {
		if ( ( x / duration ) >= 1f ) {
			if (easingControl == 0.0f && left == true) {
				finished = true;
			}
			return targetTouchBending;
		}
		return Mathf.Lerp( Mathf.Sin(x * 10.0f / duration) / (x + 1.25f) * 8.0f, touchBending, Mathf.Sqrt(x / duration) );
	}
	
	public float Bounce1(float x) {
		if ( ( x / duration ) >= 1f ) {
			if (easingControl1 == 0.0f && left1 == true) {
				finished1 = true;
			}
			return targetTouchBending1;
		}
		return Mathf.Lerp( Mathf.Sin(x * 10.0f / duration) / (x + 1.25f) * 8.0f, touchBending1, Mathf.Sqrt(x / duration) );
	}
	
	public void SwapTouchBending () {
		Player_ID = Player1_ID;
		PlayerVars = PlayerVars1;
		Player_Direction = Player_Direction1;
		Player_Speed = Player_Speed1;
		intialTouchForce = intialTouchForce1;
		touchBending = touchBending1;
		targetTouchBending = targetTouchBending1;
		easingControl = easingControl1;
		left = left1;
		finished = finished1;
		axis = axis1;
		timer = timer1;
	}
		
	public void ResetTouchBending () {
		DestroyImmediate(myRenderer.material);
		myRenderer.sharedMaterial = simpleBendingMaterial;
		touched = false;
		doubletouched = false;
	}
}

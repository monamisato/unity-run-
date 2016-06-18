using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	// ここから
	public float speed = 5.0f;
	public float slideSpeed = 2.0f;
	public Camera camera;
	public Text danger;

	// ここまでの間に変数を書いてね

	Animator animator;
	UIScript uiscript;

	// ゲームが始まった時に一回だけ呼ばれる
	void Start () {
		animator = GetComponent <Animator> ();
		uiscript = GameObject.Find ("Canvas").GetComponent<UIScript> ();
	}

	// 1フレームごとに呼ばれる
	void Update ()
	{
		
		//ここから
		transform.position += Vector3.forward * speed * Time.deltaTime;
		float pos_x = transform.position.x;
		if (Input.GetKey (KeyCode.LeftArrow)) { 
			if (pos_x > -1.9f) {
				transform.position += Vector3.left * slideSpeed * Time.deltaTime;
			}
		}
		if (Input.GetKey (KeyCode.RightArrow)) { 
			if (pos_x < 1.9f) {
				transform.position += Vector3.right * slideSpeed * Time.deltaTime;
			} 
		}



		if (Input.GetKey (KeyCode.Space)) {
			animator.SetBool ("STOP", true);
			animator.SetBool ("RUN", false);
			speed = 0;

		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			animator.SetBool ("STOP", false);
			animator.SetBool ("RUN", true);
			speed = 5.0f;
		}

	





		//ここまでの間に書こう！
		shotdanger();

		//アニメーションについて(いじらない)
		if (Input.GetKey (KeyCode.UpArrow)) {
			animator.SetBool ("JUMP", true);
		}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			animator.SetBool ("JUMP", false);
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			animator.SetBool ("SLIDE", true);
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			animator.SetBool ("SLIDE", false);
		}
	}



	void shotdanger()
	{

		Vector3 center = new Vector3 (Screen.width / 2, Screen.height / 2, 0);
		Ray ray = camera.ScreenPointToRay (center);//ここで飛ばす
		int distance = 5;
		//RaycastHitでRayにぶつかったものの情報をRaycastHitの変数に格納する 
		RaycastHit hitInfo;//変数作成

		if (Physics.Raycast (ray, out hitInfo, distance)) 
		{
			if (hitInfo.collider.tag == "barrier")
			{
				danger.text = "danger";
			}

		}

	}
		
	// Triggerである障害物にぶつかったとき
	void OnTriggerEnter (Collider collider){
		
		var stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		bool isJump = stateInfo.IsName("Base Layer.JUMP00");
		bool isSlide = stateInfo.IsName("Base Layer.SLIDE00");
		bool isRun = stateInfo.IsName("Base Layer.RUN00_F");

		bool isHigh = collider.CompareTag("High");
		bool isLow = collider.CompareTag("Low");
		bool isBarrier = collider.CompareTag ("barrier");
		bool isGoal = collider.CompareTag ("goal");

		// 障害物に当たったとき
		if( (isHigh == true && isSlide == false) ||
			(isLow == true && isJump == false) ||
		    (isBarrier == true)){
			//この下に書こう
			speed = 0;
			animator.SetBool ("DEAD", true);
			// UI
			uiscript.Gameover();

		}
		//ゴールした時
		if(isGoal == true){
			//この下に書こう
			speed = 0;

			animator.SetBool ("WIN", true);
			// UI
			uiscript.Goal();
		}
	}
}

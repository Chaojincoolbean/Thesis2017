using UnityEngine;
using System.Collections;

public class Gun_Full_Contoller : MonoBehaviour {
	
	//ShootContols
	public GameObject	Bullet;
	public GameObject	Shell;
	public GameObject	Flash;	
	public AudioClip	ShotSound;
	public float		ShootingSlowness = 1F;
	public GameObject	FirePosition;
	public GameObject	ShellPosition;
	public float		BulletForce = 20f;
	public float		ShellForce = 2;
	private bool		beingHandled = false;
	public AudioClip 	RelaodSound;
	
	
	//PlayerContols
	public KeyCode		Forward = 	KeyCode.Z;
	public KeyCode 		Backward = 	KeyCode.S;
	public KeyCode		Left = 		KeyCode.Q;
	public KeyCode		Right =		KeyCode.D;
	public KeyCode		Jump =		KeyCode.Space;
	public KeyCode 		Sprint = 	KeyCode.LeftShift;
	private Animator 	PlayerAnimator;
	public float		speed = 5F;
	public float		jumpSpeed = 5F;
	public float		sprintMultiplier = 2F;
	public float 		gravity = 1F;
	private float		moveH;
	private float		moveV;
	private Vector3		moveDirection = Vector3.zero;
	
	
	//GunContols
	public KeyCode	Relaod = 	KeyCode.R;
	public KeyCode	Shoot =		KeyCode.Mouse0;
	
	
	
	void Start (){
		
		PlayerAnimator = GetComponent <Animator>();
	}
	
	//Shooting
	private IEnumerator Shooting(){
		
		beingHandled = true;
		Camera cam = Camera.main;
		GameObject thebullet;
		GameObject theshell;
		GameObject theflash;
		if (FirePosition) theflash = (GameObject)Instantiate(Flash, FirePosition.transform.position + FirePosition.transform.forward, FirePosition.transform.rotation);
		else  theflash = (GameObject)Instantiate(Flash, FirePosition.transform.position + FirePosition.transform.forward, FirePosition.transform.rotation);		
		theflash.transform.parent = transform;		
		if (FirePosition) thebullet = (GameObject)Instantiate(Bullet, FirePosition.transform.position + FirePosition.transform.forward, FirePosition.transform.rotation);
		else  thebullet = (GameObject)Instantiate(Bullet, FirePosition.transform.position + FirePosition.transform.forward, FirePosition.transform.rotation);
		if (ShellPosition) theshell = (GameObject)Instantiate(Shell, ShellPosition.transform.position + ShellPosition.transform.right, ShellPosition.transform.rotation);
		else  theshell = (GameObject)Instantiate(Shell, ShellPosition.transform.position + ShellPosition.transform.forward, ShellPosition.transform.rotation);

		thebullet.GetComponent<Rigidbody>().AddForce(FirePosition.transform.forward * BulletForce, ForceMode.Impulse);
		theshell.GetComponent<Rigidbody>().AddForce(ShellPosition.transform.right * ShellForce, ForceMode.Impulse);
		GetComponent<AudioSource>().PlayOneShot(ShotSound);
		if(PlayerAnimator)PlayerAnimator.Play(Animator.StringToHash("Shoot"));
		yield return new WaitForSeconds (ShootingSlowness);
		beingHandled = false;
		
	}
	
	//PlayerControls
	void Update (){
		
		float Sprintspeed = speed;
		float Horizontal = 0F;
		float Vertical = 0F;
		
		if (Input.GetKey(Shoot) && !beingHandled) {
			StartCoroutine (Shooting());
		}
		
		if (Input.GetKeyDown(Sprint) && !beingHandled) {
			if(PlayerAnimator)PlayerAnimator.Play(Animator.StringToHash("Run_Trans_Start"));
		}
		
		if (Input.GetKeyDown(Relaod) && !beingHandled) {
			GetComponent<AudioSource>().PlayOneShot(RelaodSound);
			if(PlayerAnimator)PlayerAnimator.Play(Animator.StringToHash("Relaod"));


		}
			
		if (Input.GetKeyUp(Sprint)) {
			if(PlayerAnimator)PlayerAnimator.Play(Animator.StringToHash("Run_trans_Stop"));			
		}
		
	}
	
	
}
	
	
namespace VRTK.Examples {
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

	public class MP40Fire : VRTK_InteractableObject {

	// Use this for initialization
		public GameObject muzzleFlash;
		private Transform slide;
		private Transform muzzle;

	public override void StartUsing(VRTK_InteractUse usingObject)
		{
			base.StartUsing(usingObject);
			FireRayCast();
		}
	protected void Start () {
			slide = gameObject.transform.GetChild (2);
			muzzle = gameObject.transform.GetChild (4);
		}
	
	// Update is called once per frame
		private void FireRayCast () {
			print ("gun fired");
			slide.DOLocalMoveY (-43f, 0.07f);
			Invoke ("SlideRetract", 0.07f);
			Instantiate (muzzleFlash, muzzle.position, Quaternion.identity);
		}

		private void SlideRetract() {
			slide.DOLocalMoveY (3.147339f, 0.07f);
		}
	}
}

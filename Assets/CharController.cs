using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour
{
	private Animator anim;
	public static CharController obj;
	public int currentPose = 0;

	//  0 -Normal Ride.
	//	1 - front bend
	//	2 - backbend
	//  3 - StandForRide

	void Start ()
	{
		anim = this.GetComponent<Animator> ();
		obj = this;
	}

	private void ResetAnim ()
	{
		anim.SetInteger ("CharPoseNo", -1);
		currentPose = -1;
	}


	public void LeanBackWard ()
	{

		//RBike.obj.Hipp.GetComponent<Rigidbody> ().AddForce (-200, 400, 0);

		anim.SetInteger ("CharPoseNo", 2);
		currentPose = 2;
	}

	public void LeanForward ()
	{


		//RBike.obj.Hipp.GetComponent<Rigidbody> ().AddForce (0, 400, 0);

		anim.SetInteger ("CharPoseNo", 1);
		currentPose = 1;
	}

	public void LeanNormal ()
	{
		anim.SetInteger ("CharPoseNo", 0);
		currentPose = 0;
	}

	public void RidePose ()
	{
		anim.SetInteger ("CharPoseNo", 3);
		currentPose = 3;
	}

	void Update ()
	{
	}
}

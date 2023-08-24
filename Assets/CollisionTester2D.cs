using UnityEngine;
using System.Collections;

public class CollisionTester2D : MonoBehaviour 
{
	public static bool front_collided;
	public static bool rear_collided;

	void Start () {}

//	void OnCollisionEnter2D(Collision2D col)
//	{
//		if(gameObject.name=="coll_front_wheel"){
//			front_collided = true;
//		}
//		if(gameObject.name=="coll_rear_wheel"){
//			rear_collided = true;
//		}
//	}

	void OnCollisionStay2D(){
		if(gameObject.name=="coll_front_wheel"){
			front_collided = true;
		}
		if(gameObject.name=="coll_rear_wheel"){
			rear_collided = true;
		}
	
	}

//	void OnCollisionExit2D(Collision2D col){
//		if(gameObject.name=="coll_front_wheel"){
//			front_collided = false;
//		}
//		if(gameObject.name=="coll_rear_wheel"){
//			rear_collided = false;
//		}
//	}
//	
	void Update () {
	  		front_collided = false;

	} 
}

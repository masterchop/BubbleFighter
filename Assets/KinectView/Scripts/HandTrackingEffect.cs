﻿using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections;
using Kinect = Windows.Kinect;

public class HandTrackingEffect : MonoBehaviour {
	public GameObject BodySourceManager = null;
	public GameObject basic, fire, ice, ultimate, block;

    private ulong _trackedId1, _trackedId2;

    private BodySourceManager _bodyManager;

	private float timeBA1, timeIce1, timeFire1, timeUlti1, timeBlock1;
	private float timeBA2, timeIce2, timeFire2, timeUlti2, timeBlock2;

	private AudioSource baka;
	private AudioSource hadookan;

	// Use this for initialization
	void Start ()
	{
		hadookan = this.GetComponents<AudioSource> ()[0];
		baka = this.GetComponents<AudioSource> ()[1];
		_trackedId1 = 0;
		_trackedId2 = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (null == this.BodySourceManager)
		{
			return;
		}

		_bodyManager = BodySourceManager.GetComponent<BodySourceManager>();
		if (null == _bodyManager)
		{
			return;
		}
		
		Kinect.Body[] data = _bodyManager.GetData();
		if (data == null)
		{
			return;
		}
		
		if(_trackedId1 == 0 && PlayerTracking.player1TrackNum != 0)
		{
			_trackedId1 = PlayerTracking.player1TrackNum;
		}

		if(_trackedId2 == 0	&& PlayerTracking.player2TrackNum != 0)
		{
			_trackedId2 = PlayerTracking.player2TrackNum;
		}
		// are we still tracking and if so, be sure we track the hand position?
		bool isTracking1 = false;
		bool isTracking2 = false;

		timeBA1 += Time.deltaTime; timeIce1+= Time.deltaTime; timeFire1+= Time.deltaTime; 
		timeUlti1+= Time.deltaTime; timeBlock1+= Time.deltaTime;

		timeBA2 += Time.deltaTime; timeIce2+= Time.deltaTime; timeFire2+= Time.deltaTime; 
		timeUlti2+= Time.deltaTime; timeBlock2+= Time.deltaTime;

		foreach(var body in data)
		{
			if (body == null)
			{
				continue;
			}
			
			if(body.IsTracked && _trackedId1 == body.TrackingId)
			{
				isTracking1 = true; 	// flag that we are still tracking the person
				
                // render particles if the right hand is closed
				if (IsBlock(body))
				{
					//Block
				}
				else if (IsLeftBasic(body))
				{
					//Basic Attack from Left Hand
					if(timeBA1 > 0.5f)
					{
						createSpell(1, 0, true, body);
						timeBA1 = 0;
					}
				}
				else if(IsRightBasic(body))
				{
					//Basic Attack from Right Hand
					if(timeBA1 > 0.5f)
					{
						createSpell(1, 0, false, body);
						timeBA1 = 0;
					}
				}
				else if (IsLeftIce(body))
				{
					//Ice Attack from Left Hand
					if(timeIce1 > 3.0f)
					{
						createSpell(2, 0, true, body);
						timeIce1 = 0;
					}
				}
				else if(IsRightIce(body))
				{
					//Ice Attack from Right Hand
					if(timeIce1 > 3.0f)
					{
						createSpell(2, 0, false, body);
						timeIce1 = 0;
					}
				}
				else if (IsLeftFire(body))
				{
					//Fire Attack from Left Hand
					if(timeFire1 > 3.0f)
					{
						createSpell(3, 0, true, body);
						timeFire1 = 0;
					}
				}
				else if(IsRightFire(body))
				{
					//Fire Attack from Right Hand
					if(timeFire1 > 3.0f)
					{
						createSpell(3, 0, false, body);
						timeFire1 = 0;
					}
				}
				else if (IsUltimate(body))
				{
					if(timeUlti1 > 6.0f)
					{
						createSpell(4, 0, false, body);
						timeUlti1 = 0;
					}
				}
				else
				{
					//Standby
				}
			}
			else if(body.IsTracked && _trackedId2 == body.TrackingId)
			{
				isTracking2 = true; 	// flag that we are still tracking the person
				
				// render particles if the right hand is closed
				if (IsBlock(body))
				{
					//Block
				}
				else if (IsLeftBasic(body))
				{
					//Basic Attack from Left Hand
					if(timeBA2 > 0.5f)
					{
						createSpell(1, 1, true, body);
						timeBA2 = 0;
					}
				}
				else if(IsRightBasic(body))
				{
					//Basic Attack from Right Hand
					if(timeBA2 > 0.5f)
					{
						createSpell(1, 1, false, body);
						timeBA2 = 0;
					}
				}
				else if (IsLeftIce(body))
				{
					//Ice Attack from Left Hand
					if(timeIce2 > 3.0f)
					{
						createSpell(2, 1, true, body);
						timeIce2 = 0;
					}
				}
				else if(IsRightIce(body))
				{
					//Ice Attack from Right Hand
					if(timeIce2 > 3.0f)
					{
						createSpell(2, 1, false, body);
						timeIce2 = 0;
					}
				}
				else if (IsLeftFire(body))
				{
					//Fire Attack from Left Hand
					if(timeFire2> 3.0f)
					{
						createSpell(3, 1, true, body);
						timeFire2 = 0;
					}
				}
				else if(IsRightFire(body))
				{
					//Fire Attack from Right Hand
					if(timeFire2 > 3.0f)
					{
						createSpell(3, 1, false, body);
						timeFire2 = 0;
					}
				}
				else if (IsUltimate(body))
				{
					if(timeUlti2 > 6.0f)
					{
						createSpell(4, 1, false, body);
						timeUlti2 = 0;
					}
				}
				else
				{
					//Standby
				}
			}
		}
		
		// reset tracked person
		if(!isTracking1)
		{
			_trackedId1 = 0;
		}

		if(!isTracking2)
		{
			_trackedId2 = 0;
		}
	}

	private bool IsUltimate(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Lasso 
		    && body.HandLeftState == Kinect.HandState.Lasso)
			return true;
		else
			return false;
	}

	private bool IsLeftFire(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Closed 
		    && body.HandLeftState == Kinect.HandState.Lasso)
			return true;
		else
			return false;
	}
	
	private bool IsRightFire(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Lasso 
		    && body.HandLeftState == Kinect.HandState.Closed)
			return true;
		else
			return false;
	}

	private bool IsLeftIce(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Open 
		    && body.HandLeftState == Kinect.HandState.Lasso)
			return true;
		else
			return false;
	}
	
	private bool IsRightIce(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Lasso 
		    && body.HandLeftState == Kinect.HandState.Open)
			return true;
		else
			return false;
	}

	private bool IsLeftBasic(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Open 
		    && body.HandLeftState == Kinect.HandState.Closed)
			return true;
		else
			return false;
	}

	private bool IsRightBasic(Kinect.Body body)
	{
		if (body.HandRightState == Kinect.HandState.Closed 
			&& body.HandLeftState == Kinect.HandState.Open)
			return true;
		else
			return false;
	}

	private bool IsBlock(Kinect.Body body)
	{
		if(body.HandRightState == Kinect.HandState.Closed
		   && body.HandLeftState == Kinect.HandState.Closed)
			return true;
		else
			return false;
	}

	private void createSpell(int spellType, int playerNum, bool isLeftHand, Kinect.Body body) 
	{
		Kinect.CameraSpacePoint position = body.Joints [Kinect.JointType.HandRight].Position;
		if (isLeftHand) 
		{
			position = body.Joints [Kinect.JointType.HandLeft].Position;
		}
		else 
		{
			position = body.Joints [Kinect.JointType.HandRight].Position;
		}

		GameObject moves = null;
		switch (spellType) 
		{
			case 1:
			{
				moves = (GameObject)Instantiate (basic);
				break;
			}
			case 2:
			{
				moves = (GameObject)Instantiate (ice);
				baka.Play();
				break;
			}
			case 3:
			{
				moves = (GameObject)Instantiate (fire);
				baka.Play();
				break;
			}
			case 4:
			{
				moves = (GameObject)Instantiate (ultimate);
				hadookan.Play();
				break;
			}
			case 5:
			{
//				moves = (GameObject)Instantiate (block);
				break;
			}
		}

		if (moves != null)
		{
			Destroy (moves, 3);
		}
		if(playerNum == 0)
		{
			moves.layer = LayerMask.NameToLayer("Player1");
			moves.transform.position = new Vector3(-position.Z* 10f, position.Y* 10f, position.X* 10f);
		}
		else if(playerNum == 1)
		{
			moves.layer = LayerMask.NameToLayer("Player2");
			moves.transform.position = new Vector3(position.Z* 10f, position.Y* 10f, position.X* 10f);
		}
//		Debug.Log(string.Format("X:{0}, Y:{1}, Z{2}", position.X * 10, position.Y* 10, position.Z* 10));
		if (playerNum == 0 && spellType != 5) 
		{
			moves.rigidbody.velocity = new Vector3 (50, 0, 0);
//			moves.rigidbody.AddForce (2000, 0, 0);
		}
		else if (playerNum == 1 && spellType != 5) 
		{
			moves.rigidbody.velocity = new Vector3 (-50, 0, 0);
//			moves.rigidbody.AddForce (-2000, 0, 0);
		}
	}
}

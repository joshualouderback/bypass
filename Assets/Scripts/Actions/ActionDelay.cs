/*************************************************************************/
/*!
\file   ActionDelay.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Action delay is an action used to place time delays inbetween other
*  actions. It is pre-written into a simple action to make ensure no bugs
*  arise, therefore allowing more time to be spent on what is important.
**************************************************************************/

using UnityEngine;
using System.Collections;

public class ActionDelay : Action
{
	private float waitTime_; // Total time to wait
	private float timeLeft_; // Time left to wait

	// Constructor to set duration to all variables
	public ActionDelay(ActionManager manager, float duration) 
	{
		timeLeft_ = waitTime_ = duration;
		SetParent(manager);
		AddAction(this);
	}

	// Coroutine for updating delays
	public override IEnumerator Update ()
	{
		// We have started running
		running_ = true;
		
		// Delay until there is no time left to wait
		while(timeLeft_ > 0)
		{
			// If we are not paused, decrement time
			if(!IsPaused())
				timeLeft_ -= Time.deltaTime;
			// yield until timer stops
			yield return null; 
		}

		// Mark action as completed, and break yields
		completed_ = true;
		yield break;
	}	
}


/*************************************************************************/
/*!
\file   ActionRoutine.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Action Routine allows the user to create a function that is a coroutine.
*  This supports functions that can be called over a series of frames, 
*  unlike in Action Calls. It also gives the user over control of how
*  they want the function to delay, either through number of frames 
*  and/or time.
**************************************************************************/

using UnityEngine;
using System.Collections;

public class ActionRoutine : Action
{
	public delegate IEnumerator Function();
	private Function function_;
	protected Coroutine actionRoutine_ = null;
	
	public ActionRoutine(ActionManager manager, Function function)
	{
		function_ = function;
		SetParent(manager);
		AddAction(this);
	}

	public override void Cancel ()
	{
		// Cancel the routine we started
		if(parent_ != null && actionRoutine_ != null)
			parent_.GetTarget().StopCoroutine(actionRoutine_);
	}

	// Update is called once per frame
	public override IEnumerator Update () 
	{
		// We have started running
		running_ = true;
		
		// While we are paused, wait
		while(IsPaused()) 
		{
			yield return null;
		}
			
		// Wait until we return from the function
		actionRoutine_ = parent_.GetTarget().StartCoroutine(function_());
		yield return actionRoutine_;
		completed_ = true;
		
		// We are complete, break yielding
		yield break;
	}	
}

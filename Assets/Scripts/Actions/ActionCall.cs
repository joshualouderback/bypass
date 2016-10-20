/*************************************************************************/
/*!
\file   ActionCall.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Action Call is a ONE FRAME function that takes no arguements and returns
*  nothing. Once this action is reached, the function will be ran. If you
*  want to do something over a period of frames use an Action Routine.
**************************************************************************/


using UnityEngine;
using System.Collections;

public class ActionCall : Action
{
	public delegate void Function(); // Delegates for ActionCall must be of this type
	private Function function_;      // Store the delegate the user attaches to this action

	// Constructor, provide function for action call
	public ActionCall(ActionManager manager, Function function)
	{
		function_ = function;
		SetParent(manager);
		AddAction(this);
	}

	// Coroutine that updates action calls
	public override IEnumerator Update () 
	{
		// We have started running
		running_ = true;
		
		// While we are paused, wait
		while(IsPaused()) 
		{
			yield return null;
		}

		// Activate single frame function 
		function_();
		// Mark as complete, and break yieldings
		completed_ = true;
		yield break;
	}	
}
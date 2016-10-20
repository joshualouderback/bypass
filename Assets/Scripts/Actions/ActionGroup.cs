/*************************************************************************/
/*!
\file   ActionGroup.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Action Group is a list of actions, all started at the same time 
*  (same frame), and are running simultaneously until the action 
*  themselves are completed. Note that actions in a group do not have to 
*  complete at the same time, the group is completed when all actions of
*  the group have completed. 
**************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionGroup : ActionManager
{
	// List of actions in the group
	private List<Action> list = new List<Action>();

	// Non-Default Constructor
	public ActionGroup(ActionManager manager)
	{
		// Set our parent to them
		SetParent(manager);
		// And set our target as theirs
		target_ = manager.GetTarget();
		// Add ourselves to them
		base.AddAction(this);
	}
	
	// If we need to cancel all 
	public override void Cancel()
	{
		// Loop through every action and stop it
		foreach(var action in list)
		{
			// Cancel the action
			action.Cancel();
		}

		// Clear it
		list.Clear();
	}

	// Add function to push actions into group
	protected override void AddAction(Action action)
	{
		// Parent the action to this group
		action.SetParent(this);
		// Add the action to the group list
		list.Add(action);
	}

	// Coroutine for updating Action Groups
	public override IEnumerator Update()
	{
		// We have started running
		running_ = true;
		
		// If we are started and empty, wait
		while(list.Count == 0)
		{
			// This just allows users to add the group into sequence before adding
			// actions into the group
			yield return null;
		}
		
		// While we are paused, wait
		while(IsPaused()) 
		{
			yield return null;
		}
		
		// Loop through every action and start it
		foreach(var action in list)
		{
			action.StartAction();
		}
		
		// Now all have started, yield until they all are completed
		for(int i = 0; i < list.Count; )
		{
			if(!list[i].IsCompleted())
				yield return null;
			else 
				++i;
		}
		
		// Clear the group and mark as completed
		completed_ = true;
		list.Clear();	
		// Now we are done, break yielding
		yield break;
		
	}
}

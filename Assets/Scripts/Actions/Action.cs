/*************************************************************************/
/*!
\file   Action.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Actions allow designers to couple up "Actions" into
*  groups and sequences to make complex events/sequences simplier
*  to create and manage since it is all managed by the action system.
**************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Empty class we use for singleton in order to start coroutines
public class Actions : MonoBehaviour
{
}

// The "Managers" of actions (groups and sequences)
public abstract class ActionManager : Action
{
	// Since every action relies on these, but always look to their parent,
	// then we abstracted them out to the managers to reduce memory footprint
	protected Actions target_ = null;
	protected bool paused_ = false;
	
	// Children need to get the parents target to know who to ask to start
	// and stop their coroutines
	public Actions GetTarget()
	{
		return target_;
	}

	// Since the managers control pausing and resume we need to override the request
	// virtual functions of the children actions
	public override void Pause() 
	{
		paused_ = true; 
	}
	public override void Resume() 
	{ 
		paused_ = false; 
	}
}

// The base class of all actions
public abstract class Action
{
	protected ActionManager parent_ = null;
	protected bool completed_ = false;
	protected bool running_ = false;
	protected Coroutine routine_ = null;

	// Constructor
	public Action()
	{
	}

	// Generic Functions
	public Coroutine StartAction()
	{
		// Set our routine, because me manage ourselves
		routine_ = parent_.GetTarget().StartCoroutine(Update());
		// Return the routine, just in case others want to yield on us
		return routine_;
	}
	public void SetParent(ActionManager parent)
	{
		// Set our parent and our target to our parents target
		parent_ = parent;
	}
	
	// Settors
	public virtual void Pause() 
	{
		parent_.Pause(); 
	}
	public virtual void Resume() 
	{ 
		parent_.Resume();
	}

	// Gettors
	public virtual bool IsPaused() 
	{ 
		return parent_.IsPaused(); 
	}
	public bool IsCompleted() 
	{ 
		return completed_; 
	}
	public bool IsRunning()
	{
		return running_;
	}

	// Virtual methods
	protected virtual void AddAction(Action action)
	{
		parent_.AddAction(action);
	}
	public virtual IEnumerator Update()
	{		
		yield break;
	}
	public virtual void Cancel()
	{
		if(routine_ != null && parent_ != null)
		{
 			parent_.GetTarget().StopCoroutine(routine_);
		}
	}
}





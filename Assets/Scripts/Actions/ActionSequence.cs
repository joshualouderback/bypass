/*************************************************************************/
/*!
\file   ActionSequence.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  An Action Sequence is queue of actions completing each one before moving
*  onto the next action. All actions start in a sequence. Once an action 
*  is placed in the queue it will begin the sequence and will end when
*  the sequence is empty.
**************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Action Sequence is a
public class ActionSequence : ActionManager
{
	// The sequence of actions, placed in queue to maintain FIFO
	private Queue<Action> sequence = new Queue<Action>();

	// Override base isPaused implementation because we could be our own parent
	public override bool IsPaused()
	{
		// We may not be our parent if we are a sequence in a sequence, so check
		if(parent_ == this)
			return paused_;
		else
			return parent_.IsPaused();
	}

	// In case we want to add a sequence to a sequence
	public ActionSequence(ActionManager manager)
    {
		// Set our parent to them
		SetParent(manager);
		// And set our target as theirs
		target_ = manager.GetTarget();
		// Add ourselves to them
		base.AddAction(this);
	}

	// Sequences should always start as themselves
	public ActionSequence(GameObject target)
	{
		parent_ = this;

		// If target is specified
		if(target != null)
		{
			// Get action component
			var actions = target.GetComponent<Actions>();
			// If the has it attach our target to it
			if(actions != null)
				target_ = actions;
			else // Otherwise add it ourselves and attach
				target_ = target.AddComponent<Actions>();
		}
		else // Otherwise use singleton
		{
			target_ = Singleton<Actions>.Instance;
		}
	}

	// Add function to allow users to add any action type to queue	
	protected override void AddAction(Action action)
	{
		// Parent the action to this sequence
		action.SetParent(this);
	
		// Place action in queue
		sequence.Enqueue(action);

		// If this is our first action, and we are the parent
		// then we can activate our own sequence coroutine
		if(sequence.Count == 1 && parent_ == this)
		{
			routine_ = target_.StartCoroutine(this.Update());
		}
	}

	// Since we are a manager we have to tell our children to cancel
	public override void Cancel()
	{
		// As long as we have objects
		if(sequence.Count > 0)
		{
			// Grab the current action
			Action action = sequence.Peek();
			// Cancel the action
			action.Cancel();
			// Remove the rest
			sequence.Clear();
		}
	}

	// Coroutine for updating sequences
	public override IEnumerator Update()
	{
		// We have started running
		running_ = true;
		
		// While there are actions to update, update them
		while(sequence.Count > 0)
		{
			// Get the current action, so we can update it
			Action action = sequence.Peek();
			
			// If we are paused return null
			while(IsPaused())
				yield return null;
			
			// Call the action once, then wait until it is completed
			while(!action.IsCompleted())
			{	
				if(!action.IsRunning())
					yield return action.StartAction();
					//yield return Singleton<Actions>.Instance.StartCoroutine(action.Update());
				else
					yield return null;
			}
				
			if(sequence.Count > 0)
			{
				// Remove the current action
				sequence.Dequeue();
			}
		}

		// Complete, break yielding
		yield break;
	}
	
}



/*************************************************************************/
/*!
\file   EventManager.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\brief
*    Event Manager is used to tie custom delegates to CustomEvents.
**************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CustomEvent
{	
}

public class EventManager
{
	// Internal reference to our instance
	static EventManager instance_;
	static GameObject SPACE_;

	// Public static instance because we only want one Manager
	public static EventManager Instance
	{
		// Gettor to get the static instance
		get
		{
			if (instance_ == null)
			{
				instance_ = new EventManager();
				SPACE_ = new GameObject();
			}
			
			return instance_;
		}
	}

	public delegate void EventDelegate<T>(T e) where T : CustomEvent;
	
	readonly Dictionary<System.Type, Dictionary<GameObject, System.Delegate>> delegates_ = 
		new Dictionary<System.Type, Dictionary<GameObject, System.Delegate>>();
	
	/**************************************************************************
  * /author Joshua Louderback
  * /brief 
  *    Used to register event based delegates like so
  *    void OnEnable()
  *    {
  *      EventManager.Instance.Connect<NameOfEvent>(NameOfDelegateToListen);
  *    }
  * /param listener
  *    The event based delegate you are using. 
  * /return
  *    Nothing. 
  **************************************************************************/
	public void Connect<T>(EventDelegate<T> listener, GameObject obj) where T : CustomEvent
	{
		// If they pass null grab the space object
		if(obj == null)
		{
			obj = SPACE_;
		}
		// Try to find if we have a dictionary for this type already
		if (delegates_.ContainsKey(typeof(T)))
		{
			System.Delegate d;
			// If we have duplicates combine them 
			if(delegates_[typeof(T)].TryGetValue(obj, out d))
			{
				delegates_[typeof(T)][obj] = System.Delegate.Combine(d, listener);
			}
			else // Otherwise just add it
			{
				// If we do then just add the listener to that dictionary
				delegates_[typeof(T)][obj] = listener;
			}
		}
		else // Otherwise, we don't have a dictionary
		{
			// So create one
			delegates_[typeof(T)] = new Dictionary<GameObject, System.Delegate>();
			// And add it
			delegates_[typeof(T)][obj] = listener;
		}
	}
	
	/**************************************************************************
  * /author Joshua Louderback
  * /brief 
  *    Used to remove a event based delegates like so
  *    void OnDisable()
  *    {
  *      EventManager.Instance.Disconnect<NameOfEvent>(NameOfDelegateToListen);
  *    }
  * /param listener
  *    The event based delegate you were using to listen.
  * /return
  *    Nothing. 
  **************************************************************************/
	public void Disconnect<T>(EventDelegate<T> listener, GameObject obj) where T : CustomEvent
	{
		// If they pass null grab the space object
		if(obj == null)
		{
			obj = SPACE_;
		}
		// Try to find if there is a dictionary of that type
		if (delegates_.ContainsKey(typeof(T)))
		{
			// Grab the delegate out of that dictionary if it exists
			System.Delegate d;
			if(delegates_[typeof(T)].TryGetValue(obj, out d))
			{
				// There is, so attempt to remove it
				System.Delegate currentDelegate = System.Delegate.Remove(d, listener);
				
				// If it is removed, then remove it from our local dictionary
				if (currentDelegate == null)
				{
					delegates_[typeof(T)].Remove(obj);
				}
				else // Otherwise it failed, so re-add it
				{
					delegates_[typeof(T)][obj] = currentDelegate;
				}
			}
		}
	}
	
	/**************************************************************************
	* /author Joshua Louderback
	* 
	* /brief 
	*    Used to send an event to all listeners.
	* /param e
	*    The custom event you want to raise. 
	* /return
	*    Nothing. 
	**************************************************************************/
	
	public void SendEvent<T>(T e, GameObject obj) where T : CustomEvent
	{
        // If they pass null grab the space object
        if (obj == null)
		{
			obj = SPACE_;
		}
		// Make sure the event is not null
		if (e == null)
		{
			// It is, so throw an exception
			throw new UnityException("Event is null");
		}
		// Try to find the delegates of the type of event
		if (delegates_.ContainsKey(typeof(T)))
		{
			// See if it has that object
			if(delegates_[typeof(T)].ContainsKey(obj))
			{
				// If it does grab it
				EventDelegate<T> callback = delegates_[typeof(T)][obj] as EventDelegate<T>;
				// Check for null 
				if (callback != null)
				{
					// If the obj is null than we call all of them
					if(obj == SPACE_)
					{
						callback(e);
					}
					// Otherwise we check if we are that objec
					else if(obj == (callback.Target as MonoBehaviour).gameObject)
					{
						//callback.Method.Invoke(callback.Target, new object[]{e});
						// Below fixes the limiting event recieve bug on one object
						// TODO: Uncomment when we want to fix this
						callback.DynamicInvoke(new object[]{e});
					}
				}
			}
		}
	}
}	

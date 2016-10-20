/*************************************************************************/
/*!
\file   ActionProperty.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2016 DigiPen (USA) Corporation, all rights reserved.
\brief
*  An Action Property is prebuilt functions that interpolate a property
*  of the user supplied object. Properties can be interpolated over
*  different kinds of eases/curves that can be set when creating property.
**************************************************************************/


using UnityEngine;
using System.Collections;

public abstract class ActionProperty<T> : Action
{
	public delegate void Settor(T newValue);
	protected EasesAndCurves.EaseFunction easeFunction_;	
	protected float duration_;	
	protected T startValue_;
	protected T change_;
	protected Settor settor_;

	public abstract T Add(T interpolated, T start);
	public abstract T Scale(float scalar, T difference);

	public override IEnumerator Update ()
	{
		// Set the current time to duration and set as running
		float currentTime = 0;
		running_ = true;
		
		// While we have time, update once per frame
		while(currentTime < duration_)
		{
			// If not paused, update
			if(!IsPaused())
			{
				// Calculate the ease interpolation factor
				var easeFactor = easeFunction_(currentTime, duration_);
				
				// Interpolate with ease factor and use callback to set it
				settor_(Add(Scale(easeFactor, change_), startValue_)); 

				// Update frame time, yield till next frame
				currentTime += Time.deltaTime;
			}
			yield return null;
		}

		// Set the position as exactly the last position, just in case of floating point error
		settor_(Add(change_, startValue_));
		// Mark as completed, then break yielding
		completed_ = true;
		yield break;
	}

}

public class ActionPropertyVec4 : ActionProperty<Vector4>
{
	public ActionPropertyVec4(ActionManager manager, Vector4 startValue, Vector4 endValue, float duration, EasesAndCurves.Eases ease, 
	                          Settor settor)
	{
		startValue_ = startValue;
		change_ = endValue - startValue;
		duration_ = duration;
		settor_ = settor;
		easeFunction_ = new EasesAndCurves().GetEase(ease);
		SetParent(manager);
		AddAction(this);
	}

	public override Vector4 Add(Vector4 interpolated, Vector4 start)
	{
		return interpolated + start;
	}
	
	public override Vector4 Scale(float scalar, Vector4 difference)
	{
		return scalar * difference;
	}
}

public class ActionPropertyVec3 : ActionProperty<Vector3>
{
	public ActionPropertyVec3(ActionManager manager, Vector3 startValue, Vector3 endValue, float duration, EasesAndCurves.Eases ease, 
	                          Settor settor)
	{
		startValue_ = startValue;
		change_ = endValue - startValue;
		duration_ = duration;
		settor_ = settor;
		easeFunction_ = new EasesAndCurves().GetEase(ease);
		SetParent(manager);
		AddAction(this);
	}

	public override Vector3 Add(Vector3 interpolated, Vector3 start)
	{
		return interpolated + start;
	}
	
	public override Vector3 Scale(float scalar, Vector3 difference)
	{
		return scalar * difference;
	}

}

public class ActionPropertyVec2 : ActionProperty<Vector2>
{
	public ActionPropertyVec2(ActionManager manager, Vector2 startValue, Vector2 endValue, float duration, EasesAndCurves.Eases ease,
	                          Settor settor)
	{
		startValue_ = startValue;
		change_ = endValue - startValue;
		duration_ = duration;
		settor_ = settor;
		easeFunction_ = new EasesAndCurves().GetEase(ease);
		SetParent(manager);
		AddAction(this);
	}

	public override Vector2 Add(Vector2 interpolated, Vector2 start)
	{
		return interpolated + start;
	}
	
	public override Vector2 Scale(float scalar, Vector2 difference)
	{
		return scalar * difference;
	}
}

public class ActionPropertyFloat : ActionProperty<float>
{
	public ActionPropertyFloat(ActionManager manager, float startValue, float endValue, float duration, EasesAndCurves.Eases ease, 
	                           Settor settor)
	{
		startValue_ = startValue;
		change_ = endValue - startValue;
		duration_ = duration;
		settor_ = settor;
		easeFunction_ = new EasesAndCurves().GetEase(ease);
		SetParent(manager);
		AddAction(this);
	}

	public override float Add(float interpolated, float start)
	{
		return interpolated + start;
	}
	
	public override float Scale(float scalar, float difference)
	{
		return scalar * difference;
	}
}

public class ActionPropertyColor : ActionProperty<Color>
{
	public ActionPropertyColor(ActionManager manager, Color startValue, Color endValue, float duration, EasesAndCurves.Eases ease, 
	                           Settor settor)
	{
		startValue_ = startValue;
		change_ = endValue - startValue;
		duration_ = duration;
		settor_ = settor;
		easeFunction_ = new EasesAndCurves().GetEase(ease);
		SetParent(manager);
		AddAction(this);
	}
	
	public override Color Add(Color interpolated, Color start)
	{
		return interpolated + start;
	}

	public override Color Scale(float scalar, Color difference)
	{
		return scalar * difference;
	}
}
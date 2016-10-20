/*************************************************************************/
/*!
\file   EasesAndCurves.cs
\author Joshua Louderback
\par    Project: Hereafter
\par    Course: GAM300
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\brief
*  Eases and Curves is a math library for actions allowing users
*  to interpolate across different curves.
**************************************************************************/

using UnityEngine;
using System.Collections;



public class EasesAndCurves 
{
	//All the various ease types.
	public enum Eases
	{
		Linear,
		QuadIn,
		QuadInOut,
		QuadOut,
		SinIn,
		SinInOut,
		SinOut,
		ExpoIn,
		ExpoInOut,
		ExpoOut,
		CircIn,
		CircInOut,
		CircOut,
		CubicIn,
		CubicInOut,
		CubicOut,
		QuarticIn,
		QuarticInOut,
		QuarticOut,
		QuinticIn,
		QuinticInOut,
		QuinticOut
	};

	public delegate float EaseFunction(float currentTime, float duration);

	public EaseFunction GetEase(Eases easeType)
	{
		// Grab the function for caculating the ease
		switch(easeType)
		{
		case Eases.Linear:
			return Linear;
		case Eases.QuadIn:
			return QuadIn;
		case Eases.QuadInOut:
			return QuadInOut;
		case Eases.QuadOut:
			return QuadOut;
		case Eases.SinIn:
			return SinIn;
		case Eases.SinInOut:
			return SinInOut;
		case Eases.SinOut:
			return SinOut;
		case Eases.ExpoIn:
			return ExpoIn;
		case Eases.ExpoInOut:
			return ExpoInOut;
		case Eases.ExpoOut:
			return ExpoOut;
		case Eases.CircIn:
			return CircIn;
		case Eases.CircInOut:
			return CircInOut;
		case Eases.CircOut:
			return CircOut;
		case Eases.CubicIn:
			return CubicIn;
		case Eases.CubicInOut:
			return CubicInOut;
		case Eases.CubicOut:
			return CubicOut;
		case Eases.QuarticIn:
			return QuarticIn;
		case Eases.QuarticInOut:
			return QuarticInOut;
		case Eases.QuarticOut:
			return QuarticOut;
		case Eases.QuinticIn:
			return QuinticIn;
		case Eases.QuinticInOut:
			return QuinticInOut;
		case Eases.QuinticOut:
			return QuinticOut;
		default:
			return Linear;
		}
	}

	public float Linear(float currentTime, float duration)
	{
		return currentTime / duration;
	}

	public float QuadIn(float currentTime, float duration)
	{
		currentTime /= duration;
		
		return currentTime * currentTime;
	}

	public float QuadOut(float currentTime, float duration)
	{
		currentTime /= duration;
		return -1 * currentTime * (currentTime - 2);
	}

	public float QuadInOut(float currentTime, float duration)
	{
		currentTime /= duration / 2;

		if (currentTime < 1)
		{
			return (0.5f * currentTime * currentTime);
		}
		
		currentTime -= 1;
		return -0.5f * (currentTime * (currentTime - 2) - 1);
	}

	//----------------- Sinusoidal curves -----------------// 
	public float SinIn(float currentTime, float duration)
	{
		return -1 * Mathf.Cos(currentTime / duration * (Mathf.PI / 2.0f));
	}


	public float SinOut(float currentTime, float duration)
	{
		return Mathf.Sin(currentTime / duration * (Mathf.PI / 2.0f));
	}

	public float SinInOut(float currentTime, float duration)
	{
		return -0.5f * (Mathf.Cos(Mathf.PI * currentTime / duration) - 1);
	}

	//----------------- Exponential curves -----------------//

	public float ExpoIn(float currentTime, float duration)
	{

		return Mathf.Pow(2, 10 * (currentTime / duration - 1));
	}


	public float ExpoOut(float currentTime, float duration)
	{
		return (-Mathf.Pow(2, -10.0f * currentTime / duration) + 1);
	}


	public float ExpoInOut(float currentTime, float duration)
	{
		currentTime /= duration / 2;

		if (currentTime < 1)
		{
			return (1.0f / (2 * Mathf.Pow(2, 10 * (currentTime - 1))));
		}
		--currentTime;
		
		return (1.0f / (2 * (-Mathf.Pow(2, -10 * currentTime) + 2)));
	}


	//----------------- Circular curves -----------------//
	public float CircIn(float currentTime, float duration)
	{
		currentTime /= duration;
		
		return -1 * (Mathf.Sqrt(1 - currentTime * currentTime) - 1);
	}


	public float CircOut(float currentTime, float duration)
	{
		currentTime /= duration;
		--currentTime;
		
		return Mathf.Sqrt(1 - currentTime * currentTime);
	}

	public float CircInOut(float currentTime, float duration)
	{
		currentTime /= duration / 2.0f;

		if (currentTime < 1)
		{
			return (-0.5f * (Mathf.Sqrt(1 - currentTime * currentTime) - 1));
		}
		currentTime -= 2;
		
		return (0.5f) * (Mathf.Sqrt(1 - currentTime * currentTime) + 1);
	}


	//----------------- Cubic curves -----------------//
	public float CubicIn(float currentTime, float duration)
	{
		currentTime /= duration;
		return currentTime * currentTime * currentTime;
	}


	public float CubicOut(float currentTime, float duration)
	{
		currentTime /= duration;
		currentTime -= 1;
		return (currentTime * currentTime * currentTime + 1);
	}

	public float CubicInOut(float currentTime, float duration)
	{
		currentTime /= (duration / 2.0f);
		if (currentTime < 1)
		{
			return (0.5f) * currentTime * currentTime * currentTime;
		}
		
		currentTime -= 2;
		return (0.5f) * (currentTime * currentTime * currentTime + 2);
	}
	//----------------- Quartic curves -----------------// 
	public float QuarticIn(float currentTime, float duration)
	{	
		currentTime /= duration;
		return (currentTime * currentTime * currentTime * currentTime);
	}

	public float QuarticOut(float currentTime, float duration)
	{
		currentTime /= duration;
		currentTime -= 1;
		return (-1.0f) * (currentTime * currentTime * currentTime * currentTime - 1);
	}


	public float QuarticInOut(float currentTime, float duration)
	{
		currentTime /= duration / 2;
		if (currentTime < 1)
		{
			return (0.5f) * currentTime * currentTime * currentTime * currentTime;
		}
		
		currentTime -= 2;
		return (-0.5f) * (currentTime * currentTime * currentTime * currentTime - 2);
	}

	//----------------- Quintic curves -----------------//
	public float QuinticIn(float currentTime, float duration)
	{
		currentTime /= duration;
		return (currentTime * currentTime * currentTime * currentTime * currentTime);
	}


	public float QuinticOut(float currentTime, float duration)
	{
		currentTime /= duration;
		currentTime -= 1;
		return (currentTime * currentTime * currentTime * currentTime * currentTime + 1);
	}

	public float QuinticInOut(float currentTime, float duration)
	{
		currentTime /= (duration / 2.0f);

		if (currentTime < 1)
		{
			return (0.5f) * currentTime * currentTime * currentTime * currentTime * currentTime;
		}
		
		currentTime -= 2;
		return 0.5f * (currentTime * currentTime * currentTime * currentTime * currentTime + 2);
	}
}

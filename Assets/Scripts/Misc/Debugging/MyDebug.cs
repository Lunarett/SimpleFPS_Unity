using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDebug<T>
{
	public static void NullCheck(T obj)
	{
		if(obj == null) Debug.LogError($"{obj.GetType()} is returning null!");
	}

	public static bool IsNull(T obj)
	{
		return obj == null;
	}
}

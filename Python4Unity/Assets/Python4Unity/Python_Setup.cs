using Python.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Python_Setup : MonoBehaviour
{
	IEnumerator Start()
	{
		Runtime.PythonDLL = Application.dataPath + "/StreamingAssets/Python4Unity/python39/python39.dll";
		PythonEngine.Initialize(mode: ShutdownMode.Reload);

		//addPath();

		IEnumerator coroutine = ver();
		yield return coroutine;
		Debug.Log("Hello Python! : " + coroutine.Current);
	}

	public void OnApplicationQuit()
	{
		if (PythonEngine.IsInitialized)
		{
			PythonEngine.Shutdown(ShutdownMode.Reload);
			Debug.Log("Python ShutDowned");
		}
	}

	public IEnumerator ver()
	{
		yield return new WaitUntil(() => PythonEngine.IsInitialized);
		using (Py.GIL())
		{
			dynamic sys = Py.Import("sys");
			dynamic platform = Py.Import("platform");

			yield return "Python" + sys.version + "\nPlatform:" + platform.platform(terse: true) + "\nUnity:" + Application.unityVersion;
		}
	}

	private void addPath()
	{
		PythonEngine.RunSimpleString("import sys;sys.path.append('" + Application.dataPath + "/StreamingAssets/(yourlib)');");
	}
}
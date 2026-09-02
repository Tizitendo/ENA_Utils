using BepInEx;
using Logger;
using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ENA_Utils;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ENA_Utils : BaseUnityPlugin
{
	public const string PluginGUID = PluginAuthor + "." + PluginName;
	public const string PluginAuthor = "Onyx";
	public const string PluginName = "ENA_Utils";
	public const string PluginVersion = "1.0.0";

	public static GameObject ModUtilObj;
	private static ModUtil _modUtil;

	public void Awake()
	{
		Log.Init(Logger);
		JoelG.ENA4.SceneChanger.SceneChangeEvent += SceneChangeEvent;
	}

    private void SceneChangeEvent(JoelG.ENA4.SceneChanger.SceneChangeEventArgs args)
    {
		if (!ModUtilObj)
		{
			ModUtilObj = new("ModUtil");
			_modUtil = ModUtilObj.AddComponent<ModUtil>();
			DontDestroyOnLoad(ModUtilObj);
		}
    }

	public static new void StartCoroutine(IEnumerator enumerator)
	{
		if (ModUtilObj && _modUtil)
		{
			_modUtil.StartCoroutine(enumerator);
		} else
		{
			Log.Error("ModUtilobject doesn't exist");
		}
	}

	
	public class ModUtil : MonoBehaviour
	{
		void Start()
		{
			Initializer.Init();
		}
	}

	public static string GetModBundlePath(string bundleName)
	{
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), bundleName + ".bundle");
	}

	public static string GetGameBundlePath(string bundleName)
	{
		return System.IO.Path.Combine(
			Addressables.RuntimePath,
			"StandaloneWindows64",
			bundleName + ".bundle");
	}
}


public static class Reflection
{
	private const BindingFlags AllFlags =
        BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Static | BindingFlags.Instance;
	

	public static MethodInfo GetPropertyGetter(this Type type, string propName) => type.GetProperty(propName, AllFlags).GetGetMethod(true);
	public static MethodInfo GetPropertySetter(this Type type, string propName) => type.GetProperty(propName, AllFlags).GetSetMethod(true);
}



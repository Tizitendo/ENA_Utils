using Logger;
using System;
using System.Reflection;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements.Collections;
using UnityEngine.Categorization;

[PublicAPI]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
[MeansImplicitUse]
public class Initializer : Attribute
{
	public float priority = 0;
	private static HashSet<string> assemblyBlacklist = new HashSet<string>
	{
		"mscorlib", "BepInEx.Preloader", "BepInEx", "System.Core", "MonoMod.Utils", "Mono.Cecil", "Mono.Cecil.Mdb", "Mono.Cecil.Pdb", "Mono.Cecil.Rocks", 
		"MonoMod.RuntimeDetour", "0Harmony", "System", "HarmonyXInterop", "Mono.Security", "System.Configuration", "System.Xml", 
		"BepInEx.MonoMod.HookGenPatcher", "MonoMod", "MonoMod.RuntimeDetour.HookGen", "netstandard", "UnityEngine.CoreModule", "UnityEngine", 
		"UnityEngine.AIModule", "UnityEngine.ARModule", "UnityEngine.AccessibilityModule", "UnityEngine.AndroidJNIModule", "UnityEngine.AnimationModule", 
		"UnityEngine.AssetBundleModule", "UnityEngine.AudioModule", "UnityEngine.ClothModule", "UnityEngine.ClusterInputModule", 
		"UnityEngine.ClusterRendererModule", "UnityEngine.CrashReportingModule", "UnityEngine.DSPGraphModule", "UnityEngine.DirectorModule", 
		"UnityEngine.GIModule", "UnityEngine.GameCenterModule", "UnityEngine.GridModule", "UnityEngine.HotReloadModule", "UnityEngine.IMGUIModule", 
		"UnityEngine.ImageConversionModule", "UnityEngine.InputModule", "UnityEngine.InputLegacyModule", "UnityEngine.JSONSerializeModule", 
		"UnityEngine.LocalizationModule", "UnityEngine.NVIDIAModule", "UnityEngine.ParticleSystemModule", "UnityEngine.PerformanceReportingModule", 
		" UnityEngine.PhysicsModule", "UnityEngine.Physics2DModule", "UnityEngine.ProfilerModule", 
		"UnityEngine.RuntimeInitializeOnLoadManagerInitializerModule", "UnityEngine.ScreenCaptureModule", "UnityEngine.SharedInternalsModule", 
		"UnityEngine.SpriteMaskModule", "UnityEngine.SpriteShapeModule", "UnityEngine.StreamingModule", " UnityEngine.SubstanceModule", 
		"UnityEngine.SubsystemsModule", "UnityEngine.TLSModule", "UnityEngine.TerrainModule", "UnityEngine.TerrainPhysicsModule", 
		"UnityEngine.TextCoreFontEngineModule", "UnityEngine.TextCoreTextEngineModule", "UnityEngine.TextRenderingModule", "UnityEngine.TilemapModule", 
		"UnityEngine.UIModule", "UnityEngine.UIElementsModule", "UnityEngine.UIElementsNativeModule", "UnityEngine.UNETModule", "UnityEngine.UmbraModule", 
		"UnityEngine.UnityAnalyticsModule", "UnityEngine.UnityAnalyticsCommonModule", "UnityEngine.UnityConnectModule", "UnityEngine.UnityCurlModule", 
		"UnityEngine.UnityTestProtocolModule", "UnityEngine.UnityWebRequestModule", "UnityEngine.UnityWebRequestAssetBundleModule", 
		"UnityEngine.UnityWebRequestAudioModule", "UnityEngine.UnityWebRequestTextureModule", "UnityEngine.UnityWebRequestWWWModule", "UnityEngine.VFXModule", 
		"UnityEngine.VRModule", "UnityEngine.VehiclesModule", "UnityEngine.VideoModule", "UnityEngine.VirtualTexturingModule", "UnityEngine.WindModule", 
		"UnityEngine.XRModule", "Assembly-CSharp-firstpass", "Assembly-CSharp", "DOTween.Modules", "UnityFx.Outline", 
		"Unity.RenderPipelines.Universal.Shaders", "DOTweenPro.Scripts", "Unity.Addressables", "UnityFx.Outline.URP", "Unity.ProBuilder", 
		"Unity.RenderPipelines.Core.ShaderLibrary", "UnityEngine.UI", "UnityHierarchyFolders.Runtime", "Unity.ProBuilder.Poly2Tri", "YarnSpinner.Unity", 
		"Cinemachine", "LMirman.VespaIO.LLAPI", "PathCreator.Examples", "Unity.RenderPipeline.Universal.ShaderLibrary", "Rewired_Windows_Functions", 
		"Coffee.UIParticle", "ScreenSpaceReflections", "Unity.Recorder", "JoelG.ENA4", "Unity.Recorder.Base", "Unity.RenderPipelines.Universal.Runtime", 
		"NaughtyAttributes.Test", "Unity.VisualEffectGraph.Runtime", "Unity.Timeline", "LMirman.Utilities", "Unity.TextMeshPro", "Unity.ProBuilder.Csg", 
		"Unity.ResourceManager", "Unity.RenderPipelines.Core.Runtime", "Febucci.TextAnimator.Runtime", "PathCreator", "com.rlabrecque.steamworks.net", 
		"Unity.Mathematics", "Unity.ProBuilder.KdTree", "LMirman.RewiredGlyphs", "Unity.Burst", "YarnSpinnerTests.CommandsInAnAssemblyDefinition", 
		"Unity.ProBuilder.Stl", "NaughtyAttributes.Core", "LMirman.VespaIO", "Unity.ScriptableBuildPipeline", "FMODUnityResonance", "FMODUnity", 
		"Unity.RenderPipelines.ShaderGraph.ShaderGraphLibrary", "Yarn.Microsoft.Extensions.FileSystemGlobbing", "DOTween", "Yarn.System.Buffers", 
		"Yarn.Antlr4.Runtime.Standard", "Rewired_Windows", "YarnSpinner.Compiler", "System.Drawing.Common", "Yarn.System.Runtime.CompilerServices.Unsafe", 
		"Unity.Burst.Unsafe", "Yarn.Google.Protobuf", "Yarn.System.Text.Encodings.Web", "Rewired_Core", "Yarn.Microsoft.Bcl.AsyncInterfaces", 
		"Newtonsoft.Json", "YarnSpinner", "System.IO.Packaging", "Yarn.System.Threading.Tasks.Extensions", "Yarn.System.Numerics.Vectors", "DOTweenPro", 
		"DemiLib", "Microsoft.Win32.SystemEvents", "Yarn.CsvHelper", "Yarn.System.Reflection.TypeExtensions", "Yarn.System.Text.Json", "Yarn.System.Memory", 
		"UnityExplorer.BIE5.Mono", "UniverseLib.Mono", "Microsoft.CSharp", "System.Collections.Concurrent", "System.Collections", 
		"System.ComponentModel.Composition", "System.Console", "System.Data.DataSetExtensions", "System.Data", "System.Transactions", 
		"System.Diagnostics.Debug", "System.Diagnostics.Tools", "System.Drawing", "System.EnterpriseServices", "System.Globalization", 
		"System.IO.Compression", "System.IO.Compression.FileSystem", "System.IO", "System.IO.FileSystem", "System.IO.FileSystem.Primitives", 
		"System.Linq", "System.Net.Http", "System.Numerics", "System.ObjectModel", "System.Reflection", " System.Reflection.Extensions", 
		"System.Resources.ResourceManager", "System.Runtime", "System.Runtime.Extensions", "System.Runtime.InteropServices", "System.Runtime.Serialization", 
		"System.ServiceModel.Internals", "System.Security", "System.Text.Encoding", "System.Threading", "System.Threading.Tasks", 
		"System.Xml.Linq", "System.Xml.ReaderWriter", "UnityEngine.CoreModule", "MonoMod.Utils.GetManagedSizeHelper"
	};

	public static void Init()
	{
		SortedList<float, List<MethodInfo>> initializers = new((new DescendingComp()));
		foreach (Assembly assembly in GetScannableAssemblies())
		{
			foreach (Type type in GetTypes(assembly))
			{
				if (!type.IsClass)
					continue;
				foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					foreach (Initializer customAttribute in method.GetCustomAttributes<Initializer>(false))
					{
						if (initializers.ContainsKey(customAttribute.priority))
						{
							initializers.Get(customAttribute.priority).Add(method);
						} else
						{
							initializers.Add(customAttribute.priority, [method]);
						}
					}
				}
			}
		}
		foreach(List<MethodInfo> methodInfos in initializers.Values)
		{
			foreach(MethodInfo method in methodInfos)
			{
				method.Invoke(null, null);
			}
		}
	}

	public Initializer(float priority = 0)
	{
		this.priority = priority;
	}

	public static IEnumerable<Assembly> GetScannableAssemblies()
	{
		return from a in AppDomain.CurrentDomain.GetAssemblies()
			where !assemblyBlacklist.Contains(a.GetName().Name)
			select a;
	}

	public static Type[] GetTypes(Assembly assembly)
	{
		Type[] types;
		try
		{
			types = assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			Log.Error($"ScanAssembly:  {ex}");
			types = ex.Types;
			if (types == null)
			{
				return null;
			}
		}
		catch (Exception arg)
		{
			Log.Error($"ScanAssembly:  {arg}");
			return null;
		}
		return types;
	}

	class DescendingComp : IComparer<float>
	{
		public int Compare(float x, float y)
		{
			return -x.CompareTo(y);
		}
	}
}


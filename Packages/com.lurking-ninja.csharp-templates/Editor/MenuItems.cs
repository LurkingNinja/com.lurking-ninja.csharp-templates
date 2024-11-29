/***
 * C# Templates
 * Copyright (c) 2022-2024 Lurking Ninja.
 *
 * MIT License
 * https://github.com/LurkingNinja/com.lurking-ninja.csharp-templates
 */
// ReSharper disable PartialTypeWithSinglePart
namespace LurkingNinja.CSharpTemplates.Editor
{
				using System.Linq;
				using UnityEditor;

    public static partial class MenuItems
    {
        internal const string _BASE_MENU_CS = "Assets/Create C#/";
        private const string _UNITY_CSHARP_MENU = _BASE_MENU_CS + "Unity's C# Script %&o";
        private const string _CREATE_FOLDER_MENU = _BASE_MENU_CS + "Create Folder &#f";

        private static readonly string _UNITY_CS_TEMPLATE_PATH = EditorApplication.applicationContentsPath
#if UNITY_6000_0_OR_NEWER
																+ "/Resources/ScriptTemplates/1-Scripting__MonoBehaviour Script-NewMonoBehaviourScript.cs.txt";
#else
																+ "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";
#endif	    
	    
								[MenuItem(_CREATE_FOLDER_MENU, false, 10001)]
								private static void CreateFolder() => ProjectWindowUtil.CreateFolder();
		
								[MenuItem(_UNITY_CSHARP_MENU, false, 10002)]
								public static void CreateUnityCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
								        _UNITY_CS_TEMPLATE_PATH, GetFilename(_UNITY_CS_TEMPLATE_PATH));
	    
								private static string GetFilename(string assetPath) =>
								    assetPath.Split('-').Last().Replace(".txt", string.Empty);
	    
#if UNITY_6000_0_OR_NEWER
								private static readonly string _UNITY_CS_EMPTY_TEMPLATE_PATH = EditorApplication.applicationContentsPath 
												    + "/Resources/ScriptTemplates/3-Scripting__Empty C# Script-NewEmptyCSharpScript.cs.txt";
								private const string _UNITY_CS_EMPTY_MENU = _BASE_MENU_CS + "C# Script %&n";
								[MenuItem(_UNITY_CS_EMPTY_MENU, false, 10003)]
								public static void CreateUnityEmptyCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
																_UNITY_CS_EMPTY_TEMPLATE_PATH, GetFilename(_UNITY_CS_EMPTY_TEMPLATE_PATH));

								private static readonly string _UNITY_CS_SO_TEMPLATE_PATH = EditorApplication.applicationContentsPath 
																+ "/Resources/ScriptTemplates/2-Scripting__ScriptableObject Script-NewScriptableObjectScript.cs.txt";
								private const string _UNITY_SO_MENU = _BASE_MENU_CS + "New Scriptable Object %&s";
								[MenuItem(_UNITY_SO_MENU, false, 10004)]
								public static void CreateUnitySoMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
								        _UNITY_CS_SO_TEMPLATE_PATH, GetFilename(_UNITY_CS_SO_TEMPLATE_PATH));

				    private static readonly string _UNITY_CS_ASM_TEMPLATE_PATH = EditorApplication.applicationContentsPath 
																+ "/Resources/ScriptTemplates/22-Scripting__Assembly Definition-NewAssembly.asmdef.txt";
				    private const string _UNITY_ASM_MENU = _BASE_MENU_CS + "New Assembly Definition";
				    [MenuItem(_UNITY_ASM_MENU, false, 10005)]
				    public static void CreateUnityAsmMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
																_UNITY_CS_ASM_TEMPLATE_PATH, GetFilename(_UNITY_CS_ASM_TEMPLATE_PATH));

				    private static readonly string _UNITY_CS_ASMREF_TEMPLATE_PATH = EditorApplication.applicationContentsPath 
												+ "/Resources/ScriptTemplates/23-Scripting__Assembly Definition Reference-NewAssemblyReference.asmref.txt";
				    private const string _UNITY_ASMREF_MENU = _BASE_MENU_CS + "New Assembly Reference";
				    [MenuItem(_UNITY_ASMREF_MENU, false, 10005)]
				    public static void CreateUnityAsmRefMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
												_UNITY_CS_ASMREF_TEMPLATE_PATH, GetFilename(_UNITY_CS_ASMREF_TEMPLATE_PATH));
#endif
    }
}
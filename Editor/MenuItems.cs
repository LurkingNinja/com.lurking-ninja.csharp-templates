using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace LurkingNinja.CSharpTemplates.Editor
{
    public static partial class MenuItems
    {
        private const string BASE_MENU_CS = "Assets/Create C#/";
        private const string UNITY_CSHARP_MENU = BASE_MENU_CS + "Unity's C# Script %&o";
        private const string CUSTOM_CSHARP_MENU = BASE_MENU_CS + "C# Script %&n";
		private const string CREATE_FOLDER_MENU = BASE_MENU_CS + "Create Folder &#f";
		private const string CUSTOM_INTERFACE_MENU = BASE_MENU_CS + "Create Interface %#i";
		private const string SCRIPTABLE_OBJECT_MENU = BASE_MENU_CS + "Create ScriptableObject %&s";
		private const string EDITOR_SCRIPTABLE_OBJECT_MENU = BASE_MENU_CS + "Create Editor ScriptableSingleton";
        
		private const string DEFAULT_BEHAVIOR_FILENAME = "NewBehaviorScript.cs";
		private const string DEFAULT_INTERFACE_FILENAME = "NewInterfaceScript.cs";
		private const string DEFAULT_SCRIPTABLE_FILENAME = "NewScriptableObject.cs";
        private static readonly string _baseTemplatePath =
	        "Packages/com.lurking-ninja.csharp-templates/Editor/ScriptTemplates/";
        private static readonly string _unityCsTemplatePath = EditorApplication.applicationContentsPath +
                "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";

		internal static string GetProjectWindowPath()
		{
			var projectWindowUtilType = typeof(ProjectWindowUtil);
			var getActiveFolderPath = projectWindowUtilType
					.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
			var obj = getActiveFolderPath?.Invoke(null, Array.Empty<object>());
			return obj != null ? obj.ToString() : "";
		}
		
		private static void CreateScriptableObject<T>() where T : ScriptableObject
		{
			var asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset, $"{GetProjectWindowPath()}/{typeof(T).Name}.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			EditorGUIUtility.PingObject(asset);
		}

		[MenuItem(CREATE_FOLDER_MENU, false, 10001)]
		private static void CreateFolder() => ProjectWindowUtil.CreateFolder();
		
		[MenuItem(UNITY_CSHARP_MENU, false, 10002)]
		public static void CreateUnityCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_unityCsTemplatePath, DEFAULT_BEHAVIOR_FILENAME);

		[MenuItem(CUSTOM_CSHARP_MENU, false, 10013)]
		public static void CreateCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_baseTemplatePath + "NewBehaviourScript.cs.txt", DEFAULT_BEHAVIOR_FILENAME);
		
		[MenuItem(CUSTOM_INTERFACE_MENU, false, 10014)]
		public static void CreateInterfaceMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_baseTemplatePath + "NewInterfaceScript.cs.txt", DEFAULT_INTERFACE_FILENAME);
		
		[MenuItem(SCRIPTABLE_OBJECT_MENU, false, 10015)]
		public static void CreateScriptableObjectMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_baseTemplatePath + "ScriptableObject.cs.txt", DEFAULT_SCRIPTABLE_FILENAME);
		
		[MenuItem(EDITOR_SCRIPTABLE_OBJECT_MENU, false, 10016)]
		public static void CreateEditorScriptableObjectMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_baseTemplatePath + "ScriptableSingleton.cs.txt", DEFAULT_SCRIPTABLE_FILENAME);
    }
}

/***
 * C# Templates
 * Copyright (c) 2022-2024 Lurking Ninja.
 *
 * MIT License
 * https://github.com/LurkingNinja/com.lurking-ninja.csharp-templates
 */
namespace LurkingNinja.CSharpTemplates.Editor
{
	using UnityEditor;

	// ReSharper disable once PartialTypeWithSinglePart
	public static partial class MenuItems
    {
        internal const string BASE_MENU_CS = "Assets/Create C#/";
        private const string _UNITY_CSHARP_MENU = BASE_MENU_CS + "Unity's C# Script %&o";
		private const string _CREATE_FOLDER_MENU = BASE_MENU_CS + "Create Folder &#f";
        
		internal const string DEFAULT_BEHAVIOR_FILENAME = "NewBehaviorScript.cs";

		private static readonly string _UNITY_CS_TEMPLATE_PATH = EditorApplication.applicationContentsPath
			+ "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";

		[MenuItem(_CREATE_FOLDER_MENU, false, 10001)]
		private static void CreateFolder() => ProjectWindowUtil.CreateFolder();
		
		[MenuItem(_UNITY_CSHARP_MENU, false, 10002)]
		public static void CreateUnityCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_UNITY_CS_TEMPLATE_PATH, DEFAULT_BEHAVIOR_FILENAME);
    }
}

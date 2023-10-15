using UnityEditor;

namespace LurkingNinja.CSharpTemplates.Editor
{
    public static partial class MenuItems
    {
        internal const string BASE_MENU_CS = "Assets/Create C#/";
        private const string UNITY_CSHARP_MENU = BASE_MENU_CS + "Unity's C# Script %&o";
		private const string CREATE_FOLDER_MENU = BASE_MENU_CS + "Create Folder &#f";
        
		internal const string DEFAULT_BEHAVIOR_FILENAME = "NewBehaviorScript.cs";

		private static readonly string _unityCsTemplatePath = EditorApplication.applicationContentsPath
			+ "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";

		[MenuItem(CREATE_FOLDER_MENU, false, 10001)]
		private static void CreateFolder() => ProjectWindowUtil.CreateFolder();
		
		[MenuItem(UNITY_CSHARP_MENU, false, 10002)]
		public static void CreateUnityCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			_unityCsTemplatePath, DEFAULT_BEHAVIOR_FILENAME);
    }
}

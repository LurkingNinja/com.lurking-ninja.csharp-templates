using UnityEditor;

namespace LurkingNinja.CSharpTemplates.Editor
{
    public static partial class MenuItems
    {
        internal const string BASE_MENU_CS = "Assets/Create C#/";
        private const string UnityCsharpMenu = BASE_MENU_CS + "Unity's C# Script %&o";
		private const string CreateFolderMenu = BASE_MENU_CS + "Create Folder &#f";
        
		internal const string DEFAULT_BEHAVIOR_FILENAME = "NewBehaviorScript.cs";

		private static readonly string UnityCsTemplatePath = EditorApplication.applicationContentsPath
			+ "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";

		[MenuItem(CreateFolderMenu, false, 10001)]
		private static void CreateFolder() => ProjectWindowUtil.CreateFolder();
		
		[MenuItem(UnityCsharpMenu, false, 10002)]
		public static void CreateUnityCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			UnityCsTemplatePath, DEFAULT_BEHAVIOR_FILENAME);
    }
}

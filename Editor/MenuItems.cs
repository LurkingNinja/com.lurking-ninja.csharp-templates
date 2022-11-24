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

/*
		[MenuItem(CustomCsharpMenu, false, 10013)]
		public static void CreateCsMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			BaseTemplatePath + "NewBehaviourScript.cs.txt", DefaultBehaviorFilename);
		
		[MenuItem(CustomInterfaceMenu, false, 10014)]
		public static void CreateInterfaceMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			BaseTemplatePath + "NewInterfaceScript.cs.txt", DefaultInterfaceFilename);
		
		[MenuItem(ScriptableObjectMenu, false, 10015)]
		public static void CreateScriptableObjectMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			BaseTemplatePath + "ScriptableObject.cs.txt", DefaultScriptableFilename);
		
		[MenuItem(EditorScriptableObjectMenu, false, 10016)]
		public static void CreateEditorScriptableObjectMenu() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
			BaseTemplatePath + "ScriptableSingleton.cs.txt", DefaultScriptableFilename);
*/
    }
}

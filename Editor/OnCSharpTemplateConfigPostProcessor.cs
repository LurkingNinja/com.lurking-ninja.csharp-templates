using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

namespace LurkingNinja.CSharpTemplates.Editor
{
    public class OnCSharpTemplateConfigPostProcessor : AssetPostprocessor
    {
        private static readonly StringBuilder _oneMenu = new ();
        private static readonly StringBuilder _allMenus = new ();

        internal static string GenerateTemplateFilename(int index) =>
            $"{CSharpTemplatesSettings.C_SHARP_TEMPLATES_CONFIG_PATH}/template{index}.cs.txt";
        
        private static string GenerateMenu(int index, TemplateEntry entry, string path)
        {
            _oneMenu.Clear();
            _oneMenu.AppendLine($"[MenuItem(\"{MenuItems.BASE_MENU_CS}{entry.templateName}\", false, {10013 + index})]");
            _oneMenu.Append(
                $"public static void CreateMenu{index}() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(");
            _oneMenu.AppendLine($"\"{path}\", \"{entry.defaultFilename}\");");
            return _oneMenu.ToString();
        }

        private static string GenerateAllMenus()
        {
            var setting = CSharpTemplatesSettings.Get;
            _allMenus.Clear();
            _allMenus.AppendLine("using UnityEditor;");
            _allMenus.AppendLine("namespace LurkingNinja.CSharpTemplates.Editor {");
            _allMenus.AppendLine("public static partial class MenuItems {");
            for (var index = 0; index < setting.templates.Count; index++)
            {
                var path = GenerateTemplateFilename(index);
                using var writer = new StreamWriter(path, false);
                writer.WriteLine(setting.templates[index].template);
                _allMenus.AppendLine(GenerateMenu(index, setting.templates[index], path));
                AssetDatabase.ImportAsset(path);
            }
            _allMenus.AppendLine("}}");
            return _allMenus.ToString();
        }
        
        private static void GenerateFile()
        {
            if (!Directory.Exists(CSharpTemplatesSettings.C_SHARP_TEMPLATES_CONFIG_PATH))
                Directory.CreateDirectory(CSharpTemplatesSettings.C_SHARP_TEMPLATES_CONFIG_PATH);
            using var writer = new StreamWriter(CSharpTemplatesSettings.C_SHARP_TEMPLATES_MENU_FILE, false);
            writer.WriteLine(GenerateAllMenus());
            AssetDatabase.ImportAsset(CSharpTemplatesSettings.C_SHARP_TEMPLATES_MENU_FILE);
        }
        
        // To detect asset save and creation.
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
                if (AssetDatabase.LoadAssetAtPath<Object>(path) is CSharpTemplatesSettings)
                    GenerateFile();
        }
    }

    // To detect asset removal.
    public class CustomAssetModificationProcessor : AssetModificationProcessor
    {
        private static void DeleteFile()
        {
            for (var index = 0; index < CSharpTemplatesSettings.Get.templates.Count; index++)
                AssetDatabase.DeleteAsset(OnCSharpTemplateConfigPostProcessor.GenerateTemplateFilename(index));
            AssetDatabase.DeleteAsset(CSharpTemplatesSettings.C_SHARP_TEMPLATES_MENU_FILE);
        }

        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions rao)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(path) is CSharpTemplatesSettings)
                DeleteFile();
            return AssetDeleteResult.DidNotDelete;
        }
    }
}

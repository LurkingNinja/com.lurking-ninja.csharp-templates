using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

namespace LurkingNinja.CSharpTemplates.Editor
{
    public class OnCSharpTemplateConfigPostProcessor : AssetPostprocessor
    {
        private static readonly StringBuilder OneMenu = new ();
        private static readonly StringBuilder AllMenus = new ();

        internal static string GenerateTemplateFilename(int index) =>
            $"{CSharpTemplatesSettings.C_SHARP_TEMPLATES_CONFIG_PATH}/template{index}.cs.txt";
        
        private static string GenerateMenu(int index, TemplateEntry entry, string path)
        {
            OneMenu.Clear();
            OneMenu.AppendLine($"[MenuItem(\"{MenuItems.BASE_MENU_CS}{entry.templateName}\", false, {10013 + index})]");
            OneMenu.Append(
                $"public static void CreateMenu{index}() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(");
            OneMenu.AppendLine($"\"{path}\", \"{entry.defaultFilename}\");");
            return OneMenu.ToString();
        }

        private static string GenerateAllMenus()
        {
            var setting = CSharpTemplatesSettings.Get;
            AllMenus.Clear();
            AllMenus.AppendLine("using UnityEditor;");
            AllMenus.AppendLine("namespace LurkingNinja.CSharpTemplates.Editor {");
            AllMenus.AppendLine("public static partial class MenuItems {");
            for (var index = 0; index < setting.templates.Count; index++)
            {
                var path = GenerateTemplateFilename(index);
                using var writer = new StreamWriter(path, false);
                writer.WriteLine(setting.templates[index].template);
                AllMenus.AppendLine(GenerateMenu(index, setting.templates[index], path));
                AssetDatabase.ImportAsset(path);
            }
            AllMenus.AppendLine("}}");
            return AllMenus.ToString();
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

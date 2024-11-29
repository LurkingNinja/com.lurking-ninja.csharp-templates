/***
 * C# Templates
 * Copyright (c) 2022-2024 Lurking Ninja.
 *
 * MIT License
 * https://github.com/LurkingNinja/com.lurking-ninja.csharp-templates
 */
namespace LurkingNinja.CSharpTemplates.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;

    public static class CSharpTemplatesMenuGenerator
    {
        private const string _TEMPLATE_FILE_SEARCH = "template t:textAsset";
        private static readonly string[] _TEMPLATE_PATH_TO_SEARCH =
                { CSharpTemplatesSettings._C_SHARP_TEMPLATES_CONFIG_PATH };
        
        private static readonly StringBuilder _ONE_MENU = new();
        private static readonly StringBuilder _ALL_MENUS = new();

        private static string GenerateTemplateFilename(int index) =>
            $"{CSharpTemplatesSettings._C_SHARP_TEMPLATES_CONFIG_PATH}/template{index}.cs.txt";
        
        private static string GenerateMenu(int index, CSharpTemplatesSettings.TemplateEntry entry, string path)
        {
            _ONE_MENU.Clear();
            _ONE_MENU.AppendLine($"[MenuItem(\"{MenuItems._BASE_MENU_CS}{entry.templateName}\", false, {10100 + index})]");
            _ONE_MENU.Append(
                $"public static void CreateMenu{index}() => ProjectWindowUtil.CreateScriptAssetFromTemplateFile(");
            _ONE_MENU.AppendLine($"\"{path}\", \"{entry.defaultFilename}\");");
            return _ONE_MENU.ToString();
        }

        private static string GenerateAllMenus()
        {
            DeleteTemplateFiles();

            _ALL_MENUS.Clear();
            _ALL_MENUS.AppendLine("using UnityEditor;");
            _ALL_MENUS.AppendLine("namespace LurkingNinja.CSharpTemplates.Editor {");
            _ALL_MENUS.AppendLine("public static partial class MenuItems {");
            for (var index = 0; index < CSharpTemplatesSettings.Count; index++)
            {
                var entry = CSharpTemplatesSettings.Get(index); 
                if (!entry.enabled) continue;
                
                var path = GenerateTemplateFilename(index);
                
                using var writer = new StreamWriter(path, false);
                writer.WriteLine(entry.template);
                _ALL_MENUS.AppendLine(GenerateMenu(index, entry, path));
                AssetDatabase.ImportAsset(path);
            }
            _ALL_MENUS.AppendLine("}}");
            return _ALL_MENUS.ToString();
        }

        private static void DeleteTemplateFiles()
        {
            var paths = new List<string>();

            Array.ForEach(AssetDatabase.FindAssets(_TEMPLATE_FILE_SEARCH, _TEMPLATE_PATH_TO_SEARCH), guid => {
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));
            });
            AssetDatabase.DeleteAssets(paths.ToArray(), new List<string>());
        }

        internal static void GenerateMenuFile()
        {
            if (!Directory.Exists(CSharpTemplatesSettings._C_SHARP_TEMPLATES_CONFIG_PATH))
                Directory.CreateDirectory(CSharpTemplatesSettings._C_SHARP_TEMPLATES_CONFIG_PATH);
            using var writer = new StreamWriter(CSharpTemplatesSettings._C_SHARP_TEMPLATES_MENU_FILE, false);
            writer.WriteLine(GenerateAllMenus());
            AssetDatabase.ImportAsset(CSharpTemplatesSettings._C_SHARP_TEMPLATES_MENU_FILE);
        }
    }
}

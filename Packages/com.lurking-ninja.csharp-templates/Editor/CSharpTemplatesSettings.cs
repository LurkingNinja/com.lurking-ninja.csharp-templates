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
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public struct TemplateEntry
    {
        public string templateName;
        public string defaultFilename;
        [TextArea(10, 20)]
        public string template;
    }
    
    public class CSharpTemplatesSettings : ScriptableObject
    {
        internal const string C_SHARP_TEMPLATES_CONFIG_PATH = "Assets/Plugins/LurkingNinja/Editor";
        internal const string C_SHARP_TEMPLATES_MENU_FILE =
            C_SHARP_TEMPLATES_CONFIG_PATH + "/MenuItems.cs";

        private const string _C_SHARP_TEMPLATES_CONFIG_FILE =
            C_SHARP_TEMPLATES_CONFIG_PATH + "/CSharpTemplatesConfig.asset";

        public List<TemplateEntry> templates = new()
        {
            new TemplateEntry
            {
                templateName = "C# Script %&n",
                defaultFilename = MenuItems.DEFAULT_BEHAVIOR_FILENAME,
                template = @"using UnityEngine;

#ROOTNAMESPACEBEGIN#
public class #SCRIPTNAME# : MonoBehaviour
{
    // Awake is called before the first frame update
    private void Awake()
    {
        #NOTRIM#
    }
}
#ROOTNAMESPACEEND#"
            },
            new TemplateEntry
            {
                templateName = "New Interface %#i",
                defaultFilename = "NewInterface.cs",
                template = @"using UnityEngine;

#ROOTNAMESPACEBEGIN#
public interface #SCRIPTNAME#
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new TemplateEntry
            {
                templateName = "New ScriptableObject %&s",
                defaultFilename = "NewScriptableObject.cs",
                template = @"using UnityEngine;

#ROOTNAMESPACEBEGIN#
[CreateAssetMenu(fileName = ""New#SCRIPTNAME#"", menuName = ""#SCRIPTNAME#"")]
public class #SCRIPTNAME# : ScriptableObject
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new TemplateEntry
            {
                templateName = "New Editor ScriptableSingleton",
                defaultFilename = "NewScriptableObject.cs",
                template = @"using UnityEditor;

#ROOTNAMESPACEBEGIN#
[FilePath(""#SCRIPTNAME#.save"", FilePathAttribute.Location.ProjectFolder)]
public class #SCRIPTNAME# : ScriptableSingleton<#SCRIPTNAME#>
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new TemplateEntry
            {
                templateName = "New Struct",
                defaultFilename = "NewStruct.cs",
                template = @"using System;

#ROOTNAMESPACEBEGIN#
public struct #SCRIPTNAME#
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new TemplateEntry
            {
                templateName = "New Text File",
                defaultFilename = "NewTextFile.txt",
                template = @" "
            },
            new TemplateEntry
            {
                templateName = "New JSON",
                defaultFilename = "NewJson.json",
                template = @"{}"
            },
            new TemplateEntry
            {
                templateName = "New XML",
                defaultFilename = "NewXmlFile.json",
                template = @"<?xml version=""1.0"" encoding=""utf-8""?>"
            },
            new TemplateEntry
            {
                templateName = "New Package Manifest",
                defaultFilename = "package.json",
                template = @"{
  ""name"": ""com.default-company.default-package"",
  ""displayName"": ""Default Package"",
  ""description"": ""Default Package Details"",
  ""version"": ""0.0.1"",
  ""unity"": ""2021.3"",
  ""documentationUrl"": ""URL to your documentation (shows up in Package Manager)"",
  ""changelogUrl"": ""URL to your Changelog file (shows up in Package Manager)"",
  ""license"": ""Name of the License (eg.  MIT)"",
  ""licensesUrl"": ""URL to your license file (shows up in Package Manager)"",
  ""author"": {
    ""name"": ""Your  (author) name"",
    ""email"": ""Your (author) email"",
    ""url"": ""URL to your chosen website""
  }
}"
            }
        };

        private static CSharpTemplatesSettings _config;
        internal static CSharpTemplatesSettings Get => GenerateConfigFile();

        [InitializeOnLoadMethod]
        private static void BootUp()
        {
            GenerateConfigFile();
            OnCSharpTemplateConfigPostProcessor.GenerateAllMenus();
        }

        private static CSharpTemplatesSettings GenerateConfigFile()
        {
            if (_config != null) return _config;
            _config = AssetDatabase.LoadAssetAtPath<CSharpTemplatesSettings>(_C_SHARP_TEMPLATES_CONFIG_FILE);
            if (_config != null) return _config;
            if (!Directory.Exists(C_SHARP_TEMPLATES_CONFIG_PATH))
                Directory.CreateDirectory(C_SHARP_TEMPLATES_CONFIG_PATH);
            _config = CreateInstance<CSharpTemplatesSettings>();
            AssetDatabase.CreateAsset(_config, _C_SHARP_TEMPLATES_CONFIG_FILE);
            return _config;
        }
        
        [MenuItem("Tools/LurkingNinja/C# Templates Config", false)]
        private static void CSharpTemplatesConfig()
        {
            GenerateConfigFile();
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(_config);
            Selection.activeObject = _config;
        }
    }
}
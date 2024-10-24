/***
 * Lurking Ninja CodeGen
 * Copyright (c) 2022-2024 Lurking Ninja.
 *
 * MIT License
 * https://github.com/LurkingNinja/com.lurking-ninja.codegen
 */
namespace LurkingNinja.CSharpTemplates.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [FilePath("ProjectSettings/CSharpTemplatesSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class CSharpTemplatesSettings : ScriptableSingleton<CSharpTemplatesSettings>
    {
        internal const string C_SHARP_TEMPLATES_CONFIG_PATH = "Assets/Plugins/LurkingNinja/Editor";
        internal const string C_SHARP_TEMPLATES_MENU_FILE =
                C_SHARP_TEMPLATES_CONFIG_PATH + "/MenuItems.cs";
        
        [Serializable]
        public struct TemplateEntry
        {
            public bool enabled;
            public string templateName;
            public string defaultFilename;
            public string template;
        }

        public static TemplateEntry Get(int index) => instance.templates[index];
        
        public static int Count => instance.templates.Count;

        [SerializeField]
        private List<TemplateEntry> templates = GetDefaultTemplates(); 
        
        internal static void ResetTemplates() => instance.templates = GetDefaultTemplates();
        
        private static List<TemplateEntry> GetDefaultTemplates() => new()
        {
            new TemplateEntry
            {
                enabled = true,
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
                enabled = true,
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
                enabled = true,
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
                enabled = true,
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
                enabled = true,
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
                enabled = true,
                templateName = "New Text File",
                defaultFilename = "NewTextFile.txt",
                template = @" "
            },
            new TemplateEntry
            {
                enabled = true,
                templateName = "New JSON",
                defaultFilename = "NewJson.json",
                template = @"{}"
            },
            new TemplateEntry
            {
                enabled = true,
                templateName = "New XML",
                defaultFilename = "NewXmlFile.json",
                template = @"<?xml version=""1.0"" encoding=""utf-8""?>"
            },
            new TemplateEntry
            {
                enabled = true,
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
        
        internal static SerializedObject GetSerializedSettings() => new(instance);

        internal static void Save() => instance.Save(true);
        
        private void OnDisable() => Save();
    }
}
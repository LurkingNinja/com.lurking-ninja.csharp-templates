using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LurkingNinja.CSharpTemplates.Editor
{
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

        private const string CSharpTemplatesConfigFile =
            C_SHARP_TEMPLATES_CONFIG_PATH + "/CSharpTemplatesConfig.asset";
        private const string DefaultInterfaceFilename = "NewInterfaceScript.cs";
        private const string DefaultScriptableFilename = "NewScriptableObject.cs";

        public List<TemplateEntry> templates = new List<TemplateEntry>
        {
            new()
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
            new()
            {
                templateName = "Create Interface %#i",
                defaultFilename = DefaultInterfaceFilename,
                template = @"using UnityEngine;

    #ROOTNAMESPACEBEGIN#
public interface #SCRIPTNAME#
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new()
            {
                templateName = "Create ScriptableObject %&s",
                defaultFilename = DefaultScriptableFilename,
                template = @"using UnityEngine;

    #ROOTNAMESPACEBEGIN#
[CreateAssetMenu]
public class #SCRIPTNAME# : ScriptableObject
{
    #NOTRIM#
}
#ROOTNAMESPACEEND#"
            },
            new()
            {
                templateName = "Create Editor ScriptableSingleton",
                defaultFilename = DefaultScriptableFilename,
                template = @"using UnityEditor;

#ROOTNAMESPACEBEGIN#
[FilePath(""#SCRIPTNAME#.save"", FilePathAttribute.Location.ProjectFolder)]
                public class #SCRIPTNAME# : ScriptableSingleton<#SCRIPTNAME#>
                {
#NOTRIM#
            }
#ROOTNAMESPACEEND#"
            }
        };

        private static CSharpTemplatesSettings _config;
        internal static CSharpTemplatesSettings Get => GenerateConfigFile();
        
        private static CSharpTemplatesSettings GenerateConfigFile()
        {
            if (_config != null) return _config;
            _config = AssetDatabase.LoadAssetAtPath<CSharpTemplatesSettings>(CSharpTemplatesConfigFile);
            if (_config != null) return _config;
            if (!Directory.Exists(C_SHARP_TEMPLATES_CONFIG_PATH))
                Directory.CreateDirectory(C_SHARP_TEMPLATES_CONFIG_PATH);
            _config = CreateInstance<CSharpTemplatesSettings>();
            AssetDatabase.CreateAsset(_config, CSharpTemplatesConfigFile);
            AssetDatabase.SaveAssets();
            return _config;
        }
        
        [MenuItem("Tools/LurkingNinja/CSharp Templates Config", false)]
        private static void CSharpTemplatesConfig()
        {
            GenerateConfigFile();
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(_config);
            Selection.activeObject = _config;
        }
    }
}
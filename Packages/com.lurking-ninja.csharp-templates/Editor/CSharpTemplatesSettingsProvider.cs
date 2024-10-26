/***
 * Lurking Ninja CodeGen
 * Copyright (c) 2022-2024 Lurking Ninja.
 *
 * MIT License
 * https://github.com/LurkingNinja/com.lurking-ninja.codegen
 */
namespace LurkingNinja.CSharpTemplates.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class CSharpTemplatesSettingsProvider : SettingsProvider
    {
        private const string _SETTINGS_PATH = "Project/Lurking Ninja/C# Templates";

        private SerializedObject _serializedSettings;

        private readonly GUIContent _templateNameGui = new("Template name");
        private readonly GUIContent _defaultFilenameGui = new("File name");
        private readonly GUIContent _templateGui = new("Template");
        private readonly GUIContent _deleteTemplate = new("Delete template");
        private readonly GUIContent _newTemplate = new("New template");
        private readonly GUIContent _saveGui = new("Save changes");
        private readonly GUIContent _resetGui = new("Reset templates");

        private readonly GUILayoutOption _buttonWidth = GUILayout.Width(100);
        private readonly GUILayoutOption _textAreaMaxHeight = GUILayout.MaxHeight(100);
        
        private const string _TEMPLATES = "templates";
        private const string _ENABLED = "enabled";
        private const string _TEMPLATE_NAME = "templateName";
        private const string _DEFAULT_FILENAME = "defaultFilename";
        private const string _TEMPLATE = "template";
        
        private const string _DEFAULT_NEW_TEMPLATE_NAME = "New Template";
        private const string _DEFAULT_NEW_DEFAULT_FILENAME = "NewTemplate.cs";
        private const string _DEFAULT_NEW_TEMPLATE = "/* new template */";
        
        private SerializedProperty _templates;

        private CSharpTemplatesSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) {}

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            CSharpTemplatesSettings.Save();
            _serializedSettings = CSharpTemplatesSettings.GetSerializedSettings();
        }

        private Vector2 _scroll = Vector2.zero;
        
        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            AddLine();
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUIStyle.none);

            _templates = _serializedSettings.FindProperty(_TEMPLATES);
            for (var index = 0; index < _templates.arraySize; index++)
            {
                var property = _templates.GetArrayElementAtIndex(index);
                
                var tempEnabled = property.FindPropertyRelative(_ENABLED);
                var templateName = property.FindPropertyRelative(_TEMPLATE_NAME);
                var defaultFilename = property.FindPropertyRelative(_DEFAULT_FILENAME);
                
                tempEnabled.boolValue = EditorGUILayout.BeginToggleGroup(templateName.stringValue, tempEnabled.boolValue);
                if (tempEnabled.boolValue)
                {
                    EditorGUILayout.PropertyField(templateName, _templateNameGui);
                    EditorGUILayout.PropertyField(defaultFilename, _defaultFilenameGui);
                    EditorGUILayout.PrefixLabel(_templateGui);
                    var template = property.FindPropertyRelative(_TEMPLATE);
                    template.stringValue = EditorGUILayout.TextArea(template.stringValue, _textAreaMaxHeight);
                
                    AddDeleteButton(index);
                }
                
                EditorGUILayout.EndToggleGroup();
                if (index < _templates.arraySize - 1) AddLine();
            }
            EditorGUILayout.EndScrollView();
            AddLine();

           
            AddButtons();
            
            AddSeparator();
            
        }

        private string _txt;
        
        private void Apply()
        {
            _serializedSettings.ApplyModifiedPropertiesWithoutUndo();
            _serializedSettings.Update();
            Save();
        }

        private static void Save()
        {
            CSharpTemplatesSettings.Save();
            CSharpTemplatesMenuGenerator.GenerateMenuFile();
        }

        private void AddButtons()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button(_newTemplate, _buttonWidth))
            {
                _templates.arraySize++;
                var newItem = _templates.GetArrayElementAtIndex(_templates.arraySize - 1);
                newItem.FindPropertyRelative(_ENABLED).boolValue = true;
                newItem.FindPropertyRelative(_TEMPLATE_NAME).stringValue = _DEFAULT_NEW_TEMPLATE_NAME;
                newItem.FindPropertyRelative(_DEFAULT_FILENAME).stringValue = _DEFAULT_NEW_DEFAULT_FILENAME;
                newItem.FindPropertyRelative(_TEMPLATE).stringValue = _DEFAULT_NEW_TEMPLATE;
                _scroll = new Vector2(0, float.MaxValue);
            }

            if (GUILayout.Button(_resetGui, _buttonWidth))
            {
                CSharpTemplatesSettings.ResetTemplates();
                Save();
            }
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(_saveGui, _buttonWidth)) Apply();

            GUILayout.EndHorizontal();
        }

        private void AddDeleteButton(int index)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_deleteTemplate, _buttonWidth))
            {
                _templates.DeleteArrayElementAtIndex(index);
                Apply();
            }

            GUILayout.EndHorizontal();
        }
        
        private static void AddSeparator()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
        }
        
        private static void AddLine(float padding = 0f)
        {
            GUILayout.Space(padding / 2);
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(padding / 2);
        }

        [SettingsProvider]
        public static SettingsProvider CreateCSharpTemplatesSettingsProvider() =>
                new CSharpTemplatesSettingsProvider(_SETTINGS_PATH);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeSettings settings;
    BehaviourTree tree;
    BehaviourTreeView treeView;
    ToolbarMenu toolbarMenu;
    VisualElement overlay;
    InspectorView inspectorView;
    IMGUIContainer blackboardView;
    TextField treeNameField;
    TextField locationPathField;
    Button createNewTreeButton;
    Button exitNewTreeMenuButton;

    SerializedObject treeObject;
    SerializedProperty blackboardProperty;

    [MenuItem("Window/BehaviourTreeEditor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOnpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    List<T> LoadAssets<T>() where T : UnityEngine.Object
    {
        string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        List<T> assets = new List<T>();
        foreach (var assetId in assetIds)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetId);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            assets.Add(asset);
        }
        return assets;
    }

    public void CreateGUI()
    {
        settings = BehaviourTreeSettings.GetOrCreateSettings();

        VisualElement root = rootVisualElement;

        var visualTree = settings.behaviourTreeXml;
        visualTree.CloneTree(root);

        var styleSheet = settings.behaviourTreeStyle;
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        treeView.OnNodeSelected = OnNodeSelectionChange;

        inspectorView = root.Q<InspectorView>();

        blackboardView = root.Q<IMGUIContainer>();
        blackboardView.onGUIHandler = () =>
        {
            if (treeObject != null)
            {
                treeObject.Update();
                EditorGUILayout.PropertyField(blackboardProperty);
                treeObject.ApplyModifiedProperties();

                if (blackboardProperty != null && blackboardProperty.objectReferenceValue is Blackboard)
                {
                    SerializedObject blackboardObject = new SerializedObject(blackboardProperty.objectReferenceValue);
                    blackboardObject.Update();
                    SerializedProperty iterator = blackboardObject.GetIterator();

                    iterator.NextVisible(true);

                    while (iterator.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }

                    blackboardObject.ApplyModifiedProperties();
                }
            }
        };

        toolbarMenu = root.Q<ToolbarMenu>();
        toolbarMenu.menu.AppendAction("New Tree...", (a) => overlay.style.visibility = Visibility.Visible);
        toolbarMenu.menu.AppendSeparator();
        var behaviourTrees = LoadAssets<BehaviourTree>();
        behaviourTrees.ForEach(tree =>
        {
            toolbarMenu.menu.AppendAction($"{tree.name}", (a) =>
            {
                Selection.activeObject = tree;
            });
        });

        treeNameField = root.Q<TextField>("TreeName");
        locationPathField = root.Q<TextField>("LocationPath");
        overlay = root.Q<VisualElement>("Overlay");
        createNewTreeButton = root.Q<Button>("CreateButton");
        createNewTreeButton.clicked += () => CreateNewTree(treeNameField.value);
        exitNewTreeMenuButton = root.Q<Button>("ExitButton");
        exitNewTreeMenuButton.clicked += () => overlay.style.visibility = Visibility.Hidden;
        overlay.style.visibility = Visibility.Hidden;

        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if (runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                treeView.PopulateView(tree);
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }

        if (tree != null)
        {
            treeObject = new SerializedObject(tree);
            blackboardProperty = treeObject.FindProperty("blackboardRef");
        }
    }

    void OnNodeSelectionChange(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeState();
    }

    void CreateNewTree(string assetName)
    {
        string path = System.IO.Path.Combine(locationPathField.value, $"{assetName}.asset");
        BehaviourTree tree = ScriptableObject.CreateInstance<BehaviourTree>();
        tree.name = treeNameField.ToString();
        AssetDatabase.CreateAsset(tree, path);
        AssetDatabase.SaveAssets();
        Selection.activeObject = tree;
        EditorGUIUtility.PingObject(tree);
        overlay.style.visibility = Visibility.Hidden;
    }
}

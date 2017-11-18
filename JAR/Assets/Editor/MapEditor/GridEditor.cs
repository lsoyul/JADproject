using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{

    Grid grid;

    private int oldIndex = 0;

    void OnEnable()
    {
        grid = (Grid)target;
    }


    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        var asset = ScriptableObject.CreateInstance<TileSet>();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //Debug.Log(path);

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        else
        {
            path += "/";
        }

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        // <HideFlags.DontSave>
        // The object will not be saved to the scene. It will not be destroyed when a new scene is loaded.
        // It is your responsibility to cleanup the object manually using DestroyImmediate, otherwise it will leak.
        asset.hideFlags = HideFlags.DontSave;
    }


    public override void OnInspectorGUI()
    {
        grid.tileWidth = createSlider("Grid Width", grid.tileWidth);
        grid.tileHeight = createSlider("Grid Height", grid.tileHeight);

        if (GUILayout.Button("Open Grid Window"))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
            window.init();
        }

        // Tile prefab
        EditorGUI.BeginChangeCheck();
        var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);

        if (EditorGUI.EndChangeCheck())
        {
            grid.tilePrefab = newTilePrefab;
            Undo.RecordObject(target, "Grid Changed");
        }


        // Tile Map
        EditorGUI.BeginChangeCheck();
        var newTileSet = (TileSet)EditorGUILayout.ObjectField("Tile set", grid.tileSet, typeof(TileSet), false);

        if (EditorGUI.EndChangeCheck())
        {
            grid.tileSet = newTileSet;
            Undo.RecordObject(target, "Grid Changed");
        }


        if (grid.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            var names = new string[grid.tileSet.prefabs.Length];
            var values = new int[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = grid.tileSet.prefabs[i] != null ? grid.tileSet.prefabs[i].name : "Empty";
                values[i] = i;
            }

            var index = EditorGUILayout.IntPopup("Select Tile!", oldIndex, names, values);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Grid Changed");
                if (oldIndex != index)
                {
                    oldIndex = index;
                    grid.tilePrefab = grid.tileSet.prefabs[index];

                    float width = grid.tilePrefab.GetComponent<Renderer>().bounds.size.x;
                    float height = grid.tilePrefab.GetComponent<Renderer>().bounds.size.y;

                    grid.tileWidth = width;
                    grid.tileHeight = height;
                }
            }
        }


    }

    private float createSlider(string labelName, float sliderPosition)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(labelName);
        grid.tileWidth = EditorGUILayout.Slider(sliderPosition, 0.1f, 100f, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }
    
    void OnSceneGUI()
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));

        Vector3 mousePos = ray.origin;

        // Mouse Left down
        if(e.isMouse && e.type == EventType.MouseDown && e.button == 0)
        {
            GUIUtility.hotControl = controlID;
            e.Use();

            GameObject gObj;
            Transform prefab = grid.tilePrefab;

            if(prefab)
            {
                Undo.IncrementCurrentGroup();

                gObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);

                Vector3 aligned = new Vector3(
                    Mathf.Floor(mousePos.x / grid.tileWidth) * grid.tileWidth + grid.tileWidth / 2.0f,
                    Mathf.Floor(mousePos.y / grid.tileHeight) * grid.tileHeight + grid.tileHeight / 2.0f,
                    0.0f);

                // If there is something at clicked position, do nothing.
                if (GetTransformFromPosition(aligned) != null) return;

                // aligned to center position of the cell
                gObj.transform.position = aligned;
                gObj.transform.parent = grid.transform;

                Undo.RegisterCreatedObjectUndo(gObj, "Create " + gObj.name);
            }
        }

        // Mouse Right down
        if(e.isMouse && e.type == EventType.MouseDown && e.button == 1)
        {
            GUIUtility.hotControl = controlID;
            e.Use();
            Vector3 aligned = new Vector3(
                    Mathf.Floor(mousePos.x / grid.tileWidth) * grid.tileWidth + grid.tileWidth / 2.0f,
                    Mathf.Floor(mousePos.y / grid.tileHeight) * grid.tileHeight + grid.tileHeight / 2.0f,
                    0.0f);

            Transform transform = GetTransformFromPosition(aligned);

            if (transform != null)
            {
                DestroyImmediate(transform.gameObject);
            }
        }

        if (e.isMouse && e.type == EventType.MouseUp)
        {
            // release hotcontrol
            GUIUtility.hotControl = 0;
        }
    }


    Transform GetTransformFromPosition(Vector3 aligned)
    {
        int i = 0;
        while (i < grid.transform.childCount)
        {
            Transform trans = grid.transform.GetChild(i);
            if(trans.position == aligned)
            {
                return trans;
            }

            i++;
        }

        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class AssetBundleCreator : EditorWindow {
    static string directoryPath = "Assets/AssetBundles";
    static string metaDirectoryPath = "Assets/BundlesMeta";

    static public string subject = "";
    static public string subscription = "";
    static public string contentType = "";

    static private string[] AllFiles;
    static private List<string> files;
    static private int currentFileIndex;
    static private bool waitForUserInput;
    static private string currFile;

    [MenuItem("Bundles/Generate Meta Files")]
    public static void GetFiles() {

        files = new List<string>();

        if (Directory.Exists(directoryPath)) {
            AllFiles = Directory.GetFiles(directoryPath);
            foreach (var file in AllFiles) {
                if (!file.Contains("meta") && !metaFileAreadyExists(file)) {
                    files.Add(file);
                }
            }
            currentFileIndex = 0;
            waitForUserInput = true;

            EditorCoroutineUtility.StartCoroutineOwnerless(ProcessFiles());

        }
        else {
            Debug.LogError("Directory does not exist: " + directoryPath);
        }
    }

    private static  IEnumerator ProcessFiles() {
        while (currentFileIndex < files.Count) {
            var file = new FileInfo(files[currentFileIndex]);
            currFile = file.Name;

            byte[] fileData = File.ReadAllBytes(file.FullName);

            Debug.Log("Processing file: " + currFile);

            waitForUserInput = true;
            //AssetBundleCreator window = ScriptableObject.CreateInstance<AssetBundleCreator>();
            //window.position = new Rect(Screen.width / 2, Screen.height / 2, 650, 450);
            //window.Show();
            GetWindow<AssetBundleCreator>("Meta File Generator");

            while (waitForUserInput) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    waitForUserInput = false;
                }

                yield return null;
            }

            currentFileIndex++;
        }

        Debug.Log("All files processed");
    }

    private void OnGUI() {

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Asset Bundle [ " + currFile + " ]", EditorStyles.toolbarButton);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        subject = EditorGUILayout.TextField("Enter the subject:", subject);
        GUILayout.Space(10);
        subscription = EditorGUILayout.TextField("Enter the subscription:", subscription);
        GUILayout.Space(10);
        contentType = EditorGUILayout.TextField("Enter the content type:", contentType);
        GUILayout.Space(2);
        GUILayout.Label("( The extension of the file use in the bundle's creation )");
        GUILayout.Space(10);

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal(style: EditorStyles.inspectorDefaultMargins);
        GUILayout.Label("Generate Bundle Meta Files [ " + currFile + " ]", EditorStyles.toolbarButton);
        
        if (GUILayout.Button("OK")) {
            writeMetaData(subject, subscription, contentType);
            Close();
            waitForUserInput = false;
        }
        if (GUILayout.Button("Cancel")) {
            Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(4);
    }

    private static void writeMetaData(string subject, string subscription, string contentType) {
        Debug.Log("Subject: " + subject);
        Debug.Log("Subscription: " + subscription);
        Debug.Log("Content Type: " + contentType);

        string filePath = files.ElementAt<string>(currentFileIndex);

        var guid = Guid.NewGuid();

        var fileName = new FileInfo(filePath);
        Dictionary<string, string> metaValues = new Dictionary<string, string> {
                { "id", guid.ToString()},
                { "ContentName", fileName.Name },
                { "Subject", subject },
                { "Subscription", subscription },
                { "ContentType", contentType }
            };
        string json = JsonConvert.SerializeObject(metaValues, Formatting.Indented);
        var jsonPath = metaDirectoryPath + "/" + fileName.Name + ".json";
        File.WriteAllText(jsonPath, json);
        Debug.Log("Writing meta data to: " + jsonPath);
    }

    private static bool metaFileAreadyExists(string file) {
        var fileInfo = new FileInfo(file); 
        var directory = Directory.GetFiles(metaDirectoryPath);
        foreach(var dirFile in directory) {
            var dirFileInfo = new FileInfo(dirFile);
            if(dirFileInfo.Name.Contains(fileInfo.Name)) {
                return true;
            }
        }
        return false;
    }
}
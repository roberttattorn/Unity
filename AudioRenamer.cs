using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class AudioRenamer : EditorWindow
{
    [MenuItem("Tools/Rename Methos Audio ONLY")]
    public static void RenameFiles()
    {
        string folderPath = "Assets/Dialogue";
        
        if (!Directory.Exists(folderPath)) {
            Debug.LogError("Folder not found at: " + folderPath);
            return;
        }

        string[] filePaths = Directory.GetFiles(folderPath);

        // Sort them numerically to prevent renaming a file to a name that's still in use
        System.Array.Sort(filePaths, delegate(string a, string b) {
            return GetMethosNumber(Path.GetFileNameWithoutExtension(a)).CompareTo(GetMethosNumber(Path.GetFileNameWithoutExtension(b)));
        });

        int renameCount = 0;
        foreach (string path in filePaths)
        {
            if (path.EndsWith(".meta")) continue;

            string fileName = Path.GetFileNameWithoutExtension(path);
            
            // CRITICAL CHANGE: Only proceed if the file starts with "methos"
            if (fileName.StartsWith("methos")) 
            {
                int currentNumber = GetMethosNumber(fileName);

                // Target ONLY methos19 through methos100
                if (currentNumber >= 19 && currentNumber <= 100)
                {
                    int newNumber = currentNumber - 1;
                    string newName = "methos" + newNumber;
                    
                    string error = AssetDatabase.RenameAsset(path, newName);
                    
                    if (string.IsNullOrEmpty(error))
                        renameCount++;
                    else
                        Debug.LogError(string.Format("Failed to rename {0}: {1}", fileName, error));
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log(string.Format("Process complete. Renamed {0} 'methos' files.", renameCount));
    }

    private static int GetMethosNumber(string input)
    {
        // This regex looks for "methos" followed by digits
        Match match = Regex.Match(input, @"^methos(\d+)$");
        if (match.Success)
        {
            int num;
            if (int.TryParse(match.Groups[1].Value, out num)) return num;
        }
        return -1;
    }
}




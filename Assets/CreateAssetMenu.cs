using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetMenu
{
    [MenuItem("Asset/Click TO Generate Bundle")]
    private static void CreateBundle()
    {
        Debug.Log("Hello");

        string path = Application.dataPath + "/../AssetData";

      //  BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        BuildPipeline.BuildAssetBundles(path,
                                       BuildAssetBundleOptions.None,
                                       BuildTarget.StandaloneWindows);
    }
}

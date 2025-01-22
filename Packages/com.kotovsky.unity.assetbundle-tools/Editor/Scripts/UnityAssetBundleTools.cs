using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kotovsky.UnityAssetBundleTools
{
    public class UnityAssetBundleTools
    {
        private const string MENU_PATH = "Tools/Kotovsky Asset Bundle Tools/";
        private static Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();
        private static string previousDirectory = "";

        [MenuItem(MENU_PATH + "Unload assets")]
        public static void UnloadAssetBundles()
        {
            // Unload previous bundles
            foreach (var bundle in loadedAssetBundles.Values)
            {
                try
                {
                    bundle?.Unload(true);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            loadedAssetBundles.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
        }

        [MenuItem(MENU_PATH + "Runtime/Load Scene from AssetBundle")]
        private static void LoadSceneFromAssetBundle()
        {
            string bundlePath = EditorUtility.OpenFilePanel("Select AssetBundle", previousDirectory, "*");
            if (string.IsNullOrEmpty(bundlePath))
                return;
            previousDirectory = bundlePath;

            // Unload previous bundles
            UnloadAssetBundles();

            // Load the new AssetBundle
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

            if (assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle.");
                return;
            }

            loadedAssetBundles[bundlePath] = assetBundle;

            foreach (var scenePath in assetBundle.GetAllScenePaths())
            {
                Debug.Log($"Found scene in AssetBundle: {scenePath}");
                SceneManager.LoadScene(scenePath);
                Debug.Log($"Successfully loaded scene: {scenePath}");
                return;
            }

            Debug.LogError($"Scene not found in the AssetBundle.");
        }


        [MenuItem(MENU_PATH + "Editor/Load Scene from AssetBundle")]
        private static void LoadSceneFromAssetBundleEditor()
        {
            string bundlePath = EditorUtility.OpenFilePanel("Select AssetBundle", previousDirectory, "*");
            if (string.IsNullOrEmpty(bundlePath))
                return;
            previousDirectory = bundlePath;

            // Unload previous bundles
            UnloadAssetBundles();

            // Load the new AssetBundle
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

            if (assetBundle == null)
            {
                Debug.LogError($"Failed to load AssetBundle '{bundlePath}'.");
                return;
            }

            loadedAssetBundles[bundlePath] = assetBundle;

            string[] scenePaths = assetBundle.GetAllScenePaths();

            if (scenePaths.Length == 0)
            {
                Debug.Log($"There are no scenes in asset bundle '{bundlePath}'.");
                return;
            }

            foreach (var scenePath in assetBundle.GetAllScenePaths())
            {
                Debug.Log($"Found scene in AssetBundle: '{scenePath}'.");
                EditorSceneManager.OpenScene(scenePath);
                Debug.Log($"Successfully opened scene: '{scenePath}'.");
                return;
            }

            Debug.LogError($"Scene not found in the AssetBundle '{bundlePath}'.");
        }


        [MenuItem(MENU_PATH + "Editor/Load Assets from AssetBundle")]
        private static void LoadAssetsFromAssetBundleEditor()
        {
            string bundlePath = EditorUtility.OpenFilePanel("Select AssetBundle", previousDirectory, "*");
            if (string.IsNullOrEmpty(bundlePath))
                return;
            previousDirectory = bundlePath;

            // Unload previous bundles
            UnloadAssetBundles();

            // Load the new AssetBundle
            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

            if (assetBundle == null)
            {
                Debug.LogError($"Failed to load AssetBundle '{bundlePath}'.");
                return;
            }

            loadedAssetBundles[bundlePath] = assetBundle;

            string[] assetNames = assetBundle.GetAllAssetNames();

            if (assetNames.Length == 1)
            {
                var asset = assetBundle.LoadAsset(assetNames[0]);
                GameObject.Instantiate(asset);
                Debug.Log($"Loaded asset '{asset}' from AssetBundle '{bundlePath}'.");
                return;
            }

            if (assetNames.Length > 0)
            {
                Debug.Log($"There are multiple assets in the AssetBundle '{bundlePath}'.");
                return;
            }

            Debug.LogError($"Assets not found in the AssetBundle '{bundlePath}'.");
        }
    }

}
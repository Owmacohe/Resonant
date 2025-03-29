#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using Resonant.Runtime;

namespace Resonant.Editor
{
    /// <summary>
    /// Static class for managing the creation and editing of new Resonant files
    /// </summary>
    public static class ResonantFileHandler
    {
        /// <summary>
        /// Gets a ScriptableObject from the instanceID of a file
        /// </summary>
        /// <param name="instanceID">The instanceID of the file being checked</param>
        static T GetAssetFromInstanceID<T>(int instanceID) where T : ScriptableObject
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceID, out string guid, out long _);
            
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));

            if (asset.GetType() == typeof(ResonantBehaviour)) return (T) asset;
            return null;
        }
        
        /// <summary>
        /// Checks to see whether the file with the supplied instanceID is a ResonantBehaviour
        /// </summary>
        /// <param name="instanceID">The instanceID of the file being checked</param>
        /// <returns>Whether the file is a ResonantBehaviour</returns>
        static bool IsResonantBehaviourFile(int instanceID)
        {
            return GetAssetFromInstanceID<ResonantBehaviour>(instanceID) != null;
        }
        
        /// <summary>
        /// Project view contextual menu edit option for ResonantBehaviour files
        /// </summary>
        [MenuItem("Assets/Edit Resonant Behaviour")]
        static void EditGraphFile() {
            EditorWindow.GetWindow<ResonantEditor>("Resonant Editor").Load(
                GetAssetFromInstanceID<ResonantBehaviour>(Selection.activeObject.GetInstanceID()));
        }
 
        /// <summary>
        /// Method to confirm that the edit option only shows up for ResonantBehaviour files
        /// </summary>
        /// <returns>Whether the selected file is a ResonantBehaviour file</returns>
        [MenuItem("Assets/Edit Resonant Behaviour", true)]
        static bool ConfirmEditGraphFile() {
            return IsResonantBehaviourFile(Selection.activeObject.GetInstanceID());
        }
    }
}

#endif
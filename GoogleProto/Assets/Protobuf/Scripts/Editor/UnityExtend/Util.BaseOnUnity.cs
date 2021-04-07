using System.IO;
using UnityEditor;
using UnityEngine;

namespace DA.Protobuf
{
    public static partial class Util
    {
        public static void Init()
        {
            LogAction = Debug.Log;
            LogErrorAction = Debug.LogError;
            LoadConfig();
        }
        public static void LoadConfig()
        {
            if (File.Exists(ProtobufConfigData.GetAssetPath()))
            {
                Config = AssetDatabase.LoadAssetAtPath<ProtobufConfigData>(ProtobufConfigData.GetAssetPath());
            }
            else
            {
                Config = ScriptableObject.CreateInstance<ProtobufConfigData>();
                AssetDatabase.CreateAsset(Config, ProtobufConfigData.GetAssetPath());
                Config.InitDefautPath();
            }
        }
    }
}
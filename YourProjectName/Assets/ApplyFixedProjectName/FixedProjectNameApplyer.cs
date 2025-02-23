#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace YourProjectName.Editor
{
    public class FixedProjectNameApplyer : ScriptableObject
    {
        const string OriginalProjectName = "YourProjectName";

        [InfoBox("This script is used to apply fixed project name to all .cs and .asmdef files under Assets/YourProjectName/")]
        [SerializeField] string fixedProjectName = "YourFixedProjectName";

        [Button("apply fixed project name")]
        private void ApplyFix()
        {
            // Assets/YourProjectName/ 配下の.csと.asmdef内の YourProjectName を修正
            var asmdefGuids = AssetDatabase.FindAssets("t:asmdef", new[] {"Assets/YourProjectName"});
            foreach (var asmdefGuid in asmdefGuids)
            {
                var asmdefPath = AssetDatabase.GUIDToAssetPath(asmdefGuid);
                var asmdefText = System.IO.File.ReadAllText(asmdefPath);
                asmdefText = asmdefText.Replace(OriginalProjectName, fixedProjectName);
                System.IO.File.WriteAllText(asmdefPath, asmdefText);
                // ファイル名の YourProjectName 部分を修正
                var asmdefFileInfo = new System.IO.FileInfo(asmdefPath);
                var fixedAsmdefPath = System.IO.Path.Combine(asmdefFileInfo.DirectoryName ?? "", asmdefFileInfo.Name.Replace(OriginalProjectName, fixedProjectName));
                System.IO.File.Move(asmdefPath, fixedAsmdefPath);
            }
            var csFiles = System.IO.Directory.GetFiles("Assets/YourProjectName", "*.cs", System.IO.SearchOption.AllDirectories);
            foreach (var csFile in csFiles)
            {
                var csText = System.IO.File.ReadAllText(csFile);
                csText = csText.Replace(OriginalProjectName, fixedProjectName);
                System.IO.File.WriteAllText(csFile, csText);
            }
            AssetDatabase.Refresh();
        }
    }
}
#endif

#if UNITY_ANDROID
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;


/// <summary>
/// boo.lang 이 제네릭보다 먼저 뜨는 상황을 막음.
/// https://gist.github.com/Fogsight/d8dd87315ff12245614334dfdcff5b28#file-referenceremovalprojecthook-cs
/// </summary>
[InitializeOnLoad]
public class ReferenceRemovalProjectHook {

    static ReferenceRemovalProjectHook() => ProjectFilesGenerator.ProjectFileGeneration += (string name, string content) => GetAmmendedProjectFile(content);

    private static string GetAmmendedProjectFile(string content) {
        string start = "\r\n    <Reference Include=\"Boo.Lang\">";
        string end = "</Reference>";
        int startIndex = content.IndexOf(start);
        string temp = content.Substring(startIndex); //Get tail
        temp = temp.Substring(0, temp.IndexOf(end) + end.Length); // Get replacement string
        return content.Replace(temp, "");
    }
}
#endif

using UnityEditor;

namespace Tests.Editor.Tools
{
    public partial class DependenciesResolver
    {
        [MenuItem("Tools/Tests/Resolve Dependencies")]
        public static void PerformAndroidBuild()
        {
            ResolveDependencies();
        }
    }
}
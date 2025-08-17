using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Tests.Editor.Tools
{
    public partial class DependenciesResolver
    {
        private static string _generatedPath;
        private static string _packageDependenciesPath;
        private static string _projectName;

        private const string ShellExecutableName = "sh";
        private const string PathToPackageDependencies = "Packages/com.rm.customextensions/Tests/Dependencies";
        private const string GeneratedFolderName = "Generated";
        private const string ScriptFileName = "tests_get_dependencies_local.sh";
        
        public static void ResolveDependencies()
        {
            GetPackagePath();
            BuildDependenciesProject();
            CleanUpGeneratedFiles();
            
            AssetDatabase.Refresh();
            SetupImportSettings();
            AssetDatabase.Refresh();
            
            Debug.Log("resolved");
        }

        private static void BuildDependenciesProject()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = ShellExecutableName,
                WorkingDirectory = _packageDependenciesPath,
                Arguments = $@"-c ""{ShellExecutableName} {ScriptFileName} {_generatedPath}"" ",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = true,
                LoadUserProfile = true
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            
            var result = process.StandardOutput.ReadToEnd();    
            Debug.Log(result);
            process.StandardInput.Flush();
            
            process.StandardOutput.ReadLine();
            process.WaitForExit();
        }

        private static void GetPackagePath()
        {
            _projectName = Path
                .GetFileNameWithoutExtension(Directory
                    .GetFiles(Path.GetFullPath(PathToPackageDependencies), "*.csproj")
                    .First());
            _packageDependenciesPath = Path.GetFullPath(PathToPackageDependencies);
            _generatedPath = Path
                .Combine(Application.dataPath, GeneratedFolderName)
                .Replace('\\', Path.AltDirectorySeparatorChar);
        }

        private static void CleanUpGeneratedFiles()
        {
            var files = Directory.GetFiles(_generatedPath, $"{_projectName}.*");
            foreach (var file in files) 
                File.Delete(file);
        }

        private static void SetupImportSettings()
        {
            var plugins = Directory.GetFiles(_generatedPath, "*.dll.meta");
            foreach (var plugin in plugins)
            {
                var meta = File.ReadAllText(plugin);
                var edited = meta.Replace("isExplicitlyReferenced: 0", "isExplicitlyReferenced: 1");
                File.WriteAllText(plugin, edited);
            }
        }
    }   
}
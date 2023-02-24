using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Hyperfiction.Editor
{
    public class BuildHelper : EditorWindow
    {
        int major, minor, revision;
        bool initialized = false;
        string _buildPath;

        string Version => $"{major}.{minor}.{revision}";
        BuildTarget _buildTarget;

        [MenuItem( "Tools/Build Helper" )]
        public static void ShowWindow() {
            var window = GetWindow<BuildHelper>();
            window.titleContent = new GUIContent( "Build helper" );
            window.Initialize();
            window.Show();
        }

        private void Initialize() {
            _buildPath = PlayerPrefs.GetString( $"{PlayerSettings.productName}_buildPath" );
            _buildTarget = (BuildTarget) Enum.Parse( typeof(BuildTarget), PlayerPrefs.GetString( $"{PlayerSettings.productName}_buildTarget", BuildTarget.NoTarget.ToString() ));
            CacheVersion();

            initialized = true;
        }

        void CacheVersion() {
            string[] v = PlayerSettings.bundleVersion.Split( '.' );
            if( v.Length != 3 ) {
                PlayerSettings.bundleVersion = "1.0.0";
                major = 1;
                minor = revision = 0;
            } else {
                major = int.Parse( v[0] );
                minor = int.Parse( v[1] );
                revision = int.Parse( v[2] );
            }
        }

        private void OnEnable() => EditorApplication.update += Repaint;
        private void OnDisable() => EditorApplication.update -= Repaint;

        private void OnGUI() {
            if( !initialized )
                Initialize();

            GUILayout.Space( 2f );

            DrawBuildPath();

            GUILayout.Space( 2f );
            
            GUILayout.Space( 2f );

            DrawBuild();
            
            GUILayout.Space( 2f );
            
            GUILayout.Space( 2f );

            DrawVersion();

            GUILayout.Space( 2f );
        }

        bool Button(string text, int width = 25) => GUILayout.Button(text,  GUILayout.Width(width), GUILayout.Height(25));

        private void DrawBuildPath() {
            using( new EditorGUILayout.HorizontalScope() ) {
                if (Button("\u2237")) {
                    EditorUtility.RevealInFinder( _buildPath );
                }
                GUILayout.Space( 4 );
                 if( Button( "...") ) {
                    var path = EditorUtility.SaveFolderPanel( "Save build path", "", "" );
                    if( path.Length != 0 ) {
                        _buildPath = path;
                        PlayerPrefs.SetString( $"{PlayerSettings.productName}_buildPath", path );
                    }
                }
                GUILayout.Space( 4 );
                using( new GUILayout.VerticalScope( GUILayout.Height( 25 ) ) ) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( $"Build path: {_buildPath}" );
                    GUILayout.FlexibleSpace();
                }
                
                GUILayout.Space( 4f );
            }
        }

        private void DrawVersion() {
            using( new EditorGUILayout.HorizontalScope() ) {
                GUILayout.Space( 4f );
                if( Button( "Build Major", 100 ) ) {
                    if( EditorUtility.DisplayDialog( "Build Major", "Are you sure you want to increment Major version ?", "Build Major", "cancel" ) ) {
                        major++;
                        minor = 0;
                        revision = 0;
                        Build( false, false );
                    }
                }
                GUILayout.Space( 4 );
                if( Button( "Build Minor", 100 ) ) {
                    if( EditorUtility.DisplayDialog( "Build Minor", "Are you sure you want to increment Minor version ?", "Build Minor", "cancel" ) ) {
                        minor++;
                        revision = 0;
                        Build( false, false );
                    }
                }
                GUILayout.FlexibleSpace();

                using( new GUILayout.VerticalScope( GUILayout.Height( 25 ) ) ) {
                    GUILayout.FlexibleSpace();
                    using( new EditorGUILayout.HorizontalScope() ) {
                        using( new GUILayout.VerticalScope( GUILayout.Height( 25 ) ) ) {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label( $"Version: {Version}" );
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.Space( 6f );
                        if( Button( "\u21ba", 25 ) ) {
                            CacheVersion();
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();
            }
        }

        private void DrawBuild() {
            var newBuildTarget = (BuildTarget) EditorGUILayout.EnumPopup(_buildTarget);
            if (newBuildTarget != _buildTarget) {
                _buildTarget = newBuildTarget;
                PlayerPrefs.SetString ($"{PlayerSettings.productName}_buildTarget", _buildTarget.ToString());
            }

            using( new EditorGUILayout.HorizontalScope() ) {
                GUILayout.FlexibleSpace();
                if( Button( "Build", 100 ) ) {
                    Build(false);
                }
                GUILayout.Space( 4f );
                if( Button( "Build And Run", 150 ) ) {
                    Build( true );
                }
            }
        }

        private void Build(bool run, bool incrementRevision = true) {
            if( incrementRevision ) revision++;

            PlayerSettings.bundleVersion = Version;
            string dir = Path.Combine(_buildPath, Version);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string buildPath = Path.Combine( dir, $"{PlayerSettings.productName}.exe" );

            BuildPlayerOptions options = new BuildPlayerOptions {
                scenes = EditorBuildSettings.scenes.Select( o => o.path ).ToArray(),
                locationPathName = buildPath,
                target = _buildTarget,
                options = run ? BuildOptions.AutoRunPlayer : BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer( options );
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Failed) {
                revision--;
                Debug.LogError( summary.result );
            }

            if (summary.result == BuildResult.Succeeded) {
                CopyChangelog(dir);
                CreateVersion();

                Debug.Log( $"Build {summary.result} in {summary.totalTime}" );
                EditorUtility.RevealInFinder( buildPath );
            }
        }
        private void CreateVersion() {
            string path = Path.Combine(_buildPath, Version, "version.txt");
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(Version);
            }
        }

        private void CopyChangelog(string buildDir) {
            string sourcePath = Path.Combine(Application.dataPath, "../Changelog.md");
            FileInfo fi = new FileInfo(sourcePath);
            if (fi.Exists) {
                string dist = Path.Combine(buildDir, "Changelog.md");
                fi.CopyTo(dist);
                Debug.Log("Changelog copied !");
            }
        }
    }
}
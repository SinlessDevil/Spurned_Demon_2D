//---------------------------------------------------------------------------
// <summary>
// An extension for the Unity Editor to open recent scenes of projects.
// </summary>
// <copyright file="OpenRecentScene.cs" company="bosoniq.com">
// Copyright (c) 2017 Bosoniq (tools@bosoniq.com)
// </copyright>
//---------------------------------------------------------------------------
#if (!(UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2))
#define USE_SCENE_API_5_3  // Unity 5.3 and above with EditorSceneManager.
#endif

namespace Bosoniq.Tools
{
	// The using directives are placed here inside the namespace
	// to help avoid name collisions with other code in the project.
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
#if USE_SCENE_API_5_3
	using UnityEditor.SceneManagement;
	using UnityEngine.SceneManagement;
#endif

	/// <summary>
	/// Implements a Unity Editor window to load a scene of the project.
	/// The scenes are sorted by recent use, and shortcuts can be used
	/// to quickly make the scene selection.
	/// </summary>
	[InitializeOnLoad]
	internal abstract partial class OpenRecentScene : EditorWindow
	{
		public static readonly System.Version Version = new System.Version("1.7.0");

		public const int SeperatorMenuItemPriority = 154;

		private Vector2 scrollPosition;
		private bool? closeDialogWhenSceneLoads;
		private bool wasCompleteSceneSearchPerformed;
		private string displayMessage;
		private System.Action delayedAction;
		private Texture2D backgroundTexture;
		private GUIStyle sceneButtonStyle;
		private GUIStyle highlightButtonStyle;
		private GUIStyle indexLabelStyle;
		private int indexLabelMaxLength;
		private string[] previousCachedScenePaths;
		private List<string> cachedScenePaths;
		private string[] cachedSceneTexts;
		private double cachedSceneTime;
		private static double refreshedSceneTime;
		private static string sceneRestoreOnPlayStop;

		private const string SceneFileExtension = ".unity";
		private const string ScenePathsPreference =
			"BosoniqTools.OpenRecentScene.RecentlyUsedScenes";
		private const string CloseDialogPreference =
			"BosoniqTools.OpenRecentScene.CloseDialogWhenSceneLoads";
		private const string SceneRestorePreference =
			"BosoniqTools.OpenRecentScene.SceneRestore";
		private const string HighlightText = "\u2022";
		private const string CloseDialogText = "Close dialog after load";
		private const string SearchSceneText = "Search for more...";

		private static readonly char PreferenceDataSeparator =
			Path.PathSeparator;

		private static readonly System.IFormatProvider InvariantCultureFormat =
			System.Globalization.CultureInfo.InvariantCulture;

		private static readonly System.StringComparer ProjectPathComparer =
			System.StringComparer.Ordinal;

		public static string CurrentAssets
		{
			get
			{
				return Path.GetFullPath(Application.dataPath);
			}
		}

		public static string CurrentProject
		{
			get
			{
				return Path.GetFullPath(Path.Combine(CurrentAssets, ".."));
			}
		}

		public static string CurrentScene { get; protected set; }

		protected virtual bool CloseDialogWhenSceneLoads
		{
			get
			{
				if (!this.closeDialogWhenSceneLoads.HasValue)
				{
					this.closeDialogWhenSceneLoads =
						EditorPrefs.GetBool(CloseDialogPreference, true);
				}

				return this.closeDialogWhenSceneLoads.Value;
			}

			set
			{
				if ((!this.closeDialogWhenSceneLoads.HasValue)
					|| (this.closeDialogWhenSceneLoads.Value != value))
				{
					this.closeDialogWhenSceneLoads = value;
					EditorPrefs.SetBool(CloseDialogPreference, value);
				}
			}
		}

		protected static string SceneRestoreOnPlayStop
		{
			get
			{
				if (sceneRestoreOnPlayStop == null)
				{
					sceneRestoreOnPlayStop =
						(GetSceneRestoreData() ?? string.Empty);
				}

				return sceneRestoreOnPlayStop;
			}

			set
			{
				if ((sceneRestoreOnPlayStop == null)
					|| (sceneRestoreOnPlayStop != (value ?? string.Empty)))
				{
					sceneRestoreOnPlayStop = (value ?? string.Empty);
					SetSceneRestoreData(sceneRestoreOnPlayStop);
				}
			}
		}

		static OpenRecentScene()
		{
			// Monitor any changes to the Hierarchy Window to detect scene changes.
			EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;

			// Monitor play-mode changes to restore the scene when needed.
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
		}

		private static void OnHierarchyWindowChanged()
		{
			DetectSceneChange();
		}

		protected static void DetectSceneChange()
		{
			// Detect all scene changes to keep track of the most recent scenes.
			if ((CurrentScene ?? "") != (GetCurrentScenePath() ?? ""))
			{
				OnSceneChanged();
			}
		}

		private static void OnPlaymodeStateChanged()
		{
			if ((!EditorApplication.isPlaying)
				&& (!EditorApplication.isPlayingOrWillChangePlaymode)
				&& (!string.IsNullOrEmpty(SceneRestoreOnPlayStop)))
			{
				// Restore the previous scene when playing terminates.
				string scenePath = SceneRestoreOnPlayStop;
				SceneRestoreOnPlayStop = null;
				OpenScene(scenePath, false);
			}
		}

		protected static void OnSceneChanged()
		{
			refreshedSceneTime = EditorApplication.timeSinceStartup;
			CurrentScene = GetCurrentScenePath();
			string currentProject = CurrentProject;
			string currentScene = CurrentScene;
			if (!string.IsNullOrEmpty(currentProject))
			{
				// Load the list of most-recently-used scenes.
				Dictionary<string, List<string>> projectScenes =
					LoadProjectScenePaths(true);
				List<string> scenePaths = projectScenes[currentProject];

				// Place the current scene at the top of the recent-usage list.
				bool isListChanged = false;
				if (!string.IsNullOrEmpty(currentScene))
				{
					int currentSceneIndex = scenePaths.IndexOf(currentScene);
					if (currentSceneIndex != 0)
					{
						isListChanged = true;
						if (currentSceneIndex > 0)
						{
							scenePaths.RemoveAt(currentSceneIndex);
						}

						scenePaths.Insert(0, currentScene);
					}
				}

				// Remove scenes that no longer exists.
				for (int sceneIndex = (scenePaths.Count - 1); sceneIndex >= 0; sceneIndex--)
				{
					string scene = scenePaths[sceneIndex];
					if (!string.IsNullOrEmpty(scene))
					{
						if ((File.Exists(Path.Combine(currentProject, scene)))
							|| (scene == currentScene))
						{
							continue;
						}
					}

					isListChanged = true;
					scenePaths.RemoveAt(sceneIndex);
				}

				// Save the list of most-recently-used scenes.
				if (isListChanged)
				{
					projectScenes[currentProject] = scenePaths;
					SaveProjectScenePaths(projectScenes);
				}
			}
		}

		/// <summary>
		/// Load the names of recent scenes from recent projects.
		/// </summary>
		/// <returns>
		/// A dictionary that is keyed by project path, and for each project
		/// it contains a list of scenes that is ordered by most recent use.
		/// </returns>
		protected static Dictionary<string, List<string>> LoadProjectScenePaths(
			bool includeEntryForCurrentProject = false)
		{
			var projectScenes =
				new Dictionary<string, List<string>>(ProjectPathComparer);
			string savedData = EditorPrefs.GetString(ScenePathsPreference);
			string[] data = ((!string.IsNullOrEmpty(savedData)) ?
				savedData.Split(PreferenceDataSeparator) : null);
			int dataCount = ((data != null) ? data.Length : 0);
			int dataIndex = 0;
			while (dataIndex < dataCount)
			{
				string projectPath = data[dataIndex++];
				if (dataIndex >= dataCount)
				{
					Debug.LogWarning(
						"Incomplete " + ScenePathsPreference + " project data!");
					break;
				}

				int sceneCount;
				bool isParseSuccess = int.TryParse(
					data[dataIndex++],
					System.Globalization.NumberStyles.Integer,
					InvariantCultureFormat,
					out sceneCount);
				if (!isParseSuccess)
				{
					Debug.LogWarning(string.Format(
						"Invalid " + ScenePathsPreference + " count data! ({0})",
						data[dataIndex - 1]));
					break;
				}

				var scenePaths = new List<string>();
				if (sceneCount <= 0)
				{
					if (dataIndex >= dataCount)
					{
						Debug.LogWarning(
							"Invalid " + ScenePathsPreference + " scene data!");
						break;
					}

					if (!string.IsNullOrEmpty(data[dataIndex++]))
					{
						Debug.LogWarning(string.Format(
							"Bad " + ScenePathsPreference + " scene data! ({0})",
							data[dataIndex - 1]));
						break;
					}
				}
				else
				{
					for (int sceneIndex = 0; sceneIndex < sceneCount; sceneIndex++)
					{
						if (dataIndex >= dataCount)
						{
							Debug.LogWarning(
								"Incomplete " + ScenePathsPreference + " scene data!");
							break;
						}

						scenePaths.Add(data[dataIndex++]);
					}
				}

				projectScenes[projectPath] = scenePaths;
			}

			if (includeEntryForCurrentProject)
			{
				string currentProject = CurrentProject;
				List<string> scenePaths;
				if ((!projectScenes.TryGetValue(currentProject, out scenePaths))
					|| (scenePaths == null))
				{
					scenePaths = new List<string>();
					projectScenes[currentProject] = scenePaths;
				}
			}

			return projectScenes;
		}

		/// <summary>
		/// Save the names of recent scenes from recent projects.
		/// </summary>
		/// <param name="projectScenes">
		/// A dictionary that is keyed by project path, and for each project
		/// it contains a list of scenes that is ordered by most recent use.
		/// </param>
		/// <param name="discardOldProjects">
		/// True to discard data of old projects, otherwise False to save all data.
		/// </param>
		protected static void SaveProjectScenePaths(
			Dictionary<string, List<string>> projectScenes,
			bool discardOldProjects = true)
		{
			if (discardOldProjects)
			{
				var recentProjectPaths = new HashSet<string>(
					Enumerable.Range(0, 20).
					Select(i => EditorPrefs.GetString("RecentlyUsedProjectPaths-" + i)).
					Where(projectPath => (!string.IsNullOrEmpty(projectPath))).
					Select(projectPath => Path.GetFullPath(projectPath)));
				string currentProject = CurrentProject;
				foreach (string projectPath in projectScenes.Keys.ToArray())
				{
					if ((!recentProjectPaths.Contains(projectPath))
						&& (currentProject != projectPath))
					{
						projectScenes.Remove(projectPath);
					}
				}
			}

			string separator =
				PreferenceDataSeparator.ToString(InvariantCultureFormat);
			string[] projectData = projectScenes.
				Select(dictionaryItem => string.Format(
					InvariantCultureFormat,
					"{1}{0}{2}{0}{3}",
					separator,
					dictionaryItem.Key,
					dictionaryItem.Value.Count,
					string.Join(separator, dictionaryItem.Value.ToArray()))).
				ToArray();
			string data = string.Join(separator, projectData);
			EditorPrefs.SetString(ScenePathsPreference, data);
		}

		protected static string GetSceneRestoreData()
		{
			string scenePath;
			var sceneRestoreData = LoadSceneRestoreData(true)[CurrentProject];
			if (sceneRestoreData.Key < GetSceneRestoreTimestampAtStartup())
			{
				scenePath = string.Empty;
			}
			else
			{
				scenePath = (sceneRestoreData.Value ?? string.Empty);
			}

			return scenePath;
		}

		protected static void SetSceneRestoreData(
			string scenePath)
		{
			var sceneRestoreData = LoadSceneRestoreData(false);
			sceneRestoreData[CurrentProject] = new KeyValuePair<long, string>(
				GetSceneRestoreTimestampAtPresent(),
				(scenePath ?? string.Empty));
			SaveSceneRestoreData(sceneRestoreData);
		}

		private static Dictionary<string, KeyValuePair<long, string>> LoadSceneRestoreData(
			bool includeEntryForCurrentProject = false)
		{
			var sceneRestoreData =
				new Dictionary<string, KeyValuePair<long, string>>(ProjectPathComparer);
			string savedData = EditorPrefs.GetString(SceneRestorePreference);
			string[] data = ((!string.IsNullOrEmpty(savedData)) ?
				savedData.Split(PreferenceDataSeparator) : null);
			int dataCount = ((data != null) ? data.Length : 0);
			int dataIndex = 0;
			while (dataIndex < dataCount)
			{
				long timestamp;
				bool isParseSuccess = long.TryParse(
					data[dataIndex++],
					System.Globalization.NumberStyles.Integer,
					InvariantCultureFormat,
					out timestamp);
				if (!isParseSuccess)
				{
					Debug.LogWarning(string.Format(
						"Invalid " + SceneRestorePreference + " time-stamp data! ({0})",
						data[dataIndex - 1]));
					break;
				}

				if (dataIndex > (dataCount - 2))
				{
					Debug.LogWarning(
						"Incomplete " + SceneRestorePreference + " project data!");
					break;
				}

				string projectPath = data[dataIndex++];
				string scenePath = data[dataIndex++];
				sceneRestoreData[projectPath] =
					new KeyValuePair<long, string>(timestamp, scenePath);
			}

			if (includeEntryForCurrentProject)
			{
				string currentProject = CurrentProject;
				KeyValuePair<long, string> sceneData;
				if (!sceneRestoreData.TryGetValue(currentProject, out sceneData))
				{
					sceneData = new KeyValuePair<long, string>(
						GetSceneRestoreTimestampAtPresent(), string.Empty);
					sceneRestoreData[currentProject] = sceneData;
				}
			}

			return sceneRestoreData;
		}

		private static void SaveSceneRestoreData(
			Dictionary<string, KeyValuePair<long, string>> sceneRestoreData,
			bool discardOldData = true)
		{
			if (discardOldData)
			{
				const int MaxItemCount = 10;
				const long ExpirationSeconds = (7 * 24 * 60 * 60);
				long timeNow = GetSceneRestoreTimestampAtPresent();

				// Remove items that are very old.
				var expiredDataKeys = sceneRestoreData.
					Where(item => (System.Math.Abs(timeNow - item.Value.Key) > ExpirationSeconds)).
					Select(item => item.Key).ToList();
				foreach (var expiredDataKey in expiredDataKeys)
				{
					sceneRestoreData.Remove(expiredDataKey);
				}

				// If still to many entries, remove the oldest ones.
				if (sceneRestoreData.Count > MaxItemCount)
				{
					expiredDataKeys.Clear();
					expiredDataKeys.AddRange(sceneRestoreData.
						OrderBy(item => item.Value.Key).  // Timestamp.
						Take(sceneRestoreData.Count - MaxItemCount).
						Select(item => item.Key));
					foreach (var expiredDataKey in expiredDataKeys)
					{
						sceneRestoreData.Remove(expiredDataKey);
					}
				}
			}

			string separator =
				PreferenceDataSeparator.ToString(InvariantCultureFormat);
			string[] restoreData = sceneRestoreData.
				Select(item => string.Format(
					InvariantCultureFormat,
					"{1}{0}{2}{0}{3}",
					separator,
					item.Value.Key,  // Timestamp.
					item.Key,  // Project path.
					item.Value.Value)).
				ToArray();
			string data = string.Join(separator, restoreData);
			EditorPrefs.SetString(SceneRestorePreference, data);
		}

		protected static long GetSceneRestoreTimestampAtPresent()
		{
			return GetSceneRestoreTimestamp(System.DateTime.UtcNow);
		}

		protected static long GetSceneRestoreTimestampAtStartup()
		{
			System.DateTime timestampUtc = System.DateTime.UtcNow;
			return GetSceneRestoreTimestamp(
				timestampUtc.AddSeconds(-EditorApplication.timeSinceStartup));
		}

		protected static long GetSceneRestoreTimestamp(
			System.DateTime timestampUtc)
		{
			return ((timestampUtc.Ticks - 635241096000000000L) / 10000000L);
		}

		protected virtual GUIStyle GetIndexLabelStyle(int indexMaximum)
		{
			GUIStyle indexLabelStyle = this.indexLabelStyle;
			if (indexLabelStyle == null)
			{
				indexLabelStyle = new GUIStyle((GUIStyle)"label");
				indexLabelStyle.alignment = TextAnchor.MiddleRight;
				indexLabelStyle.padding.top += 2;
				this.indexLabelStyle = indexLabelStyle;
			}

			if ((this.indexLabelMaxLength != indexMaximum)
				|| (this.indexLabelMaxLength <= 0))
			{
				GUIStyle sceneButtonStyle = this.GetSceneButtonStyle();
				this.indexLabelMaxLength = indexMaximum;
				indexLabelStyle.fixedWidth = (sceneButtonStyle.CalcSize(new GUIContent(new string('X', ((indexMaximum < 10) ? 1 : (1 + Mathf.FloorToInt(Mathf.Log10(indexMaximum))))))).x);
			}

			return indexLabelStyle;
		}

		protected virtual GUIStyle GetHighlightButtonStyle()
		{
			GUIStyle highlightButtonStyle = this.highlightButtonStyle;
			if (highlightButtonStyle == null)
			{
				highlightButtonStyle = new GUIStyle((GUIStyle)"button");
				highlightButtonStyle.fixedWidth =
					highlightButtonStyle.CalcSize(new GUIContent(HighlightText)).x;
				this.highlightButtonStyle = highlightButtonStyle;
			}

			return highlightButtonStyle;
		}

		protected virtual GUIStyle GetSceneButtonStyle()
		{
			GUIStyle sceneButtonStyle = this.sceneButtonStyle;
			if (sceneButtonStyle == null)
			{
				sceneButtonStyle = new GUIStyle((GUIStyle)"button");
				sceneButtonStyle.alignment = TextAnchor.MiddleLeft;
				this.sceneButtonStyle = sceneButtonStyle;
			}

			return sceneButtonStyle;
		}

		protected virtual void OnGUI()
		{
			DetectSceneChange();

			// Check when a numeric key is pressed as a scene shortcut. Use keys
			// 0 through 9 for index 0 to 9, or Shift+0 through Shift+9 for index 10
			// to 19.  The spacebar can be used to toggle the close-window option.
			int selectedButtonIndex = -1;
			bool isHelpRequested = false;
			bool isCompleteSceneSearchRequested = false;
			var currentEvent = global::UnityEngine.Event.current;
			if (currentEvent.type == EventType.KeyDown)
			{
				if ((currentEvent.keyCode >= KeyCode.Alpha0)
					&& (currentEvent.keyCode <= KeyCode.Alpha9))
				{
					selectedButtonIndex = (currentEvent.keyCode - KeyCode.Alpha0);
				}
				else if ((currentEvent.keyCode >= KeyCode.Keypad0)
					&& (currentEvent.keyCode <= KeyCode.Keypad9))
				{
					selectedButtonIndex = (currentEvent.keyCode - KeyCode.Keypad0);
				}

				if (selectedButtonIndex >= 0)
				{
					EventModifiers eventModifiers = this.GetCurrentEventModifiers();
					if ((eventModifiers & EventModifiers.Shift) != 0)
					{
						selectedButtonIndex += 10;
					}
				}
				else
				{
					switch (currentEvent.keyCode)
					{
						case KeyCode.Space:
							this.CloseDialogWhenSceneLoads =
								(!this.CloseDialogWhenSceneLoads);
							this.Repaint();
							break;
						case KeyCode.F1:
							isHelpRequested = true;
							break;
						case KeyCode.Return:
						case KeyCode.KeypadEnter:
							isCompleteSceneSearchRequested = true;
							break;
					}
				}
			}

			// Get the latest list of scenes in the project.
			string currentProject = CurrentProject;
			var scenePaths = this.cachedScenePaths;
			if ((scenePaths == null)
				|| (this.cachedSceneTime < refreshedSceneTime))
			{
				this.cachedSceneTime = refreshedSceneTime;
				scenePaths = this.cachedScenePaths =
					LoadProjectScenePaths(true)[currentProject];
				this.wasCompleteSceneSearchPerformed = false;
				this.AddExtraScenePaths(false);
				this.RefreshScenePathTexts();
			}

			// Render the GUI for this scene-selection editor-window.
			this.DrawBackground();

			string displayMessage = this.displayMessage;
			if (!string.IsNullOrEmpty(displayMessage))
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					EditorGUILayout.HelpBox(displayMessage, MessageType.None);
					GUILayout.Space(2);
				}
				GUILayout.EndVertical();

				// Display nothing else if an important message must be shown.
				return;
			}

			var sceneTexts = this.cachedSceneTexts;
			this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);
			{
				GUILayout.BeginVertical();
				{
					GUIStyle highlightButtonStyle = this.GetHighlightButtonStyle();
					int scenePathsMaxIndex = (scenePaths.Count - 1);
					if (scenePathsMaxIndex < 0)
					{
						EditorGUILayout.HelpBox(
							"0 scenes found.", MessageType.None);
					}
					else
					{
						GUIStyle sceneButtonStyle = this.GetSceneButtonStyle();
						GUIStyle indexLabelStyle = this.GetIndexLabelStyle(scenePathsMaxIndex);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(2);
							GUILayout.BeginVertical();
							{
								for (int sceneIndex = 0; sceneIndex <= scenePathsMaxIndex; sceneIndex++)
								{
									bool isActionRequested;
									string scenePath = scenePaths[sceneIndex];

									GUILayout.BeginHorizontal();
									{
										GUILayout.Label(string.Format("{0}.", sceneIndex), indexLabelStyle);
										isActionRequested = GUILayout.Button(sceneTexts[sceneIndex], sceneButtonStyle);
										if (GUILayout.Button(new GUIContent(HighlightText, scenePath), highlightButtonStyle))
										{
											EventModifiers eventModifiers = this.GetCurrentEventModifiers();
											this.HighlightOrSelectScene(scenePath, eventModifiers);
										}
									}
									GUILayout.EndHorizontal();

									if ((!isActionRequested)
										&& (selectedButtonIndex == sceneIndex))
									{
										// Allow a scene to be selected using a shortcut.
										isActionRequested = true;
									}

									if (isActionRequested
										&& (EditorApplication.isPlaying
											|| SaveCurrentModifiedScenesIfUserWantsTo()))
									{
										// Indicate that the new scene must be
										// loaded. Although one can do so right here
										// for Unity 4.x, the older Unity 3.5 versions
										// needs OpenScene and Close to be called
										// elsewhere to prevent an error message.
										EventModifiers eventModifiers = this.GetCurrentEventModifiers();
										this.ScheduleDelayedAction(
											() => this.LoadScene(scenePath, eventModifiers),
											string.Format("Loading scene...\n{0}", scenePath));
										this.Repaint();
									}
								}

								if (!this.wasCompleteSceneSearchPerformed)
								{
									GUILayout.BeginHorizontal();
									{
										GUILayout.Label("", indexLabelStyle);
										if (GUILayout.Button(new GUIContent(SearchSceneText, "Find all scenes in project (can be slow)"), sceneButtonStyle)
											|| isCompleteSceneSearchRequested)
										{
											this.AddExtraScenePaths(true);
											this.RefreshScenePathTexts();
											this.Repaint();
										}

										GUILayout.Space(highlightButtonStyle.fixedWidth + sceneButtonStyle.margin.right);
									}
									GUILayout.EndHorizontal();
								}
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
					}

					GUILayout.FlexibleSpace();
					GUILayout.Space(8);

					GUILayout.BeginHorizontal();
					{
						this.CloseDialogWhenSceneLoads = GUILayout.Toggle(
							this.CloseDialogWhenSceneLoads, CloseDialogText);
						GUILayout.FlexibleSpace();
						var helpContent = new GUIContent("?", "Help (F1)");
						isHelpRequested = (GUILayout.Button(helpContent, EditorStyles.miniButton)
							|| isHelpRequested);
						if (Application.platform == RuntimePlatform.OSXEditor)
						{
							// On Mac OS, add some space to the right of the button
							// as a workaround for not being able to click on it
							// near the resizing grip in the bottom-right corner.
							GUILayout.Space(highlightButtonStyle.fixedWidth * 0.65f);
						}

						if (isHelpRequested)
						{
							this.DisplayHelpInfo();
						}
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(2);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();
		}

		protected virtual void DrawBackground()
		{
			Texture backgroundTexture = this.GetBackgroundTexture();
			if (backgroundTexture
				&& (backgroundTexture.width > 0)
				&& (backgroundTexture.height > 0))
			{
				var drawArea = new Rect(0, 0, this.position.width, this.position.height);
				const bool alphaBlend = true;
				if (backgroundTexture.wrapMode == TextureWrapMode.Repeat)
				{
					var uvCoordinated = new Rect(
						0,
						0,
						(drawArea.width / backgroundTexture.width),
						(drawArea.height / backgroundTexture.height));
					GUI.DrawTextureWithTexCoords(
						drawArea,
						backgroundTexture,
						uvCoordinated,
						alphaBlend);
				}
				else
				{
					GUI.DrawTexture(
						drawArea,
						backgroundTexture,
						ScaleMode.ScaleAndCrop,
						alphaBlend);
				}
			}
		}

		protected virtual Texture GetBackgroundTexture()
		{
			var backgroundTexture = this.backgroundTexture;
			if (!backgroundTexture)
			{
				const float alpha = 0.125f;
				backgroundTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
				backgroundTexture.hideFlags = HideFlags.HideAndDontSave;
				backgroundTexture.wrapMode = TextureWrapMode.Repeat;
				float lineScale = backgroundTexture.width;
				for (int x = 0, width = backgroundTexture.width; x < width; x++)
				{
					for (int y = 0, height = backgroundTexture.height; y < height; y++)
					{
						float greyscale = (((x - y + height) / lineScale) % 1.0f);
						greyscale = Mathf.Abs((greyscale * 2.0f) - 1.0f);
						Color color = new Color(greyscale, greyscale, greyscale, alpha);
						backgroundTexture.SetPixel(x, y, color);
					}
				}

				backgroundTexture.Apply();
				this.backgroundTexture = backgroundTexture;
			}

			return backgroundTexture;
		}

		protected virtual void OnDestroy()
		{
			var backgroundTexture = this.backgroundTexture;
			if (backgroundTexture)
			{
				this.backgroundTexture = null;
				Object.DestroyImmediate(backgroundTexture, false);
			}
		}

		/// <summary>
		/// Ensure that all scenes in the current project are part of the list
		/// of available scenes that the user will be able to choose from.
		/// </summary>
		protected virtual void AddExtraScenePaths(bool isFullSearchRequested)
		{
			List<string> scenePaths = this.cachedScenePaths;
			string currentProject = CurrentProject;
			int currentProjectLength = currentProject.Length;
			System.Action<string> addScenePathIfMissing = (string scenePath) =>
			{
				if (!string.IsNullOrEmpty(scenePath))
				{
					if (!Path.IsPathRooted(scenePath))
					{
						scenePath = Path.Combine(currentProject, scenePath);
					}

					scenePath = Path.GetFullPath(scenePath);
					if ((scenePath.Length > currentProjectLength)
						&& File.Exists(Path.Combine(currentProject, scenePath)))
					{
						scenePath = scenePath.Substring(currentProjectLength).
							Replace('\\', '/').TrimStart('/');
						if ((!string.IsNullOrEmpty(scenePath))
							&& (!scenePaths.Contains(scenePath)))
						{
							scenePaths.Add(scenePath);
						}
					}
				}
			};

			// Check for scenes that are part of the build settings.
			var buildScenes = EditorBuildSettings.scenes;
			if ((buildScenes != null)
				&& (buildScenes.Length > 0))
			{
				// Starting with the scenes that are to be included in the build.
				foreach (var scene in buildScenes)
				{
					if ((scene != null)
						&& scene.enabled)
					{
						addScenePathIfMissing(scene.path);
					}
				}

				// Then scan scenes that are likely to be included in a build.
				foreach (var scene in buildScenes)
				{
					if ((scene != null)
						&& (!scene.enabled))
					{
						addScenePathIfMissing(scene.path);
					}
				}
			}

			// Check for any missing scenes that were previously known.
			var previousCachedScenePaths = this.previousCachedScenePaths;
			if ((previousCachedScenePaths != null)
				&& (previousCachedScenePaths.Length > 0))
			{
				foreach (string scenePath in previousCachedScenePaths)
				{
					addScenePathIfMissing(scenePath);
				}
			}

			// Check for any missing scenes that are available on disk.
			if (isFullSearchRequested
				|| (scenePaths.Count <= 0))
			{
				this.wasCompleteSceneSearchPerformed = true;
				string[] allSceneFiles = Directory.GetFiles(
					CurrentAssets,
					("*" + SceneFileExtension),
					SearchOption.AllDirectories);
				if ((allSceneFiles != null)
					&& (allSceneFiles.Length > 0))
				{
					foreach (string scenePath in allSceneFiles)
					{
						addScenePathIfMissing(scenePath);
					}
				}
			}

			this.previousCachedScenePaths = scenePaths.ToArray();
		}

		protected virtual void Update()
		{
			var delayedAction = this.delayedAction;
			if (delayedAction != null)
			{
				this.delayedAction = null;
				delayedAction();
			}
		}

		protected virtual void ScheduleDelayedAction(
			System.Action delayedAction, string displayMessage = null)
		{
			if (string.IsNullOrEmpty(displayMessage))
			{
				this.delayedAction = delayedAction;
			}
			else
			{
				this.delayedAction = () =>
				{
					// Schedule the message to be displayed.
					this.displayMessage = displayMessage;
					this.Repaint();

					// Schedule the actual action to be performed.
					this.delayedAction = () =>
					{
						// Reset the display message.
						if (string.CompareOrdinal(this.displayMessage, displayMessage) == 0)
						{
							this.displayMessage = null;
						}

						// Perform the actual action.
						if (delayedAction != null)
						{
							delayedAction();
						}
					};
				};
			}
		}

		protected virtual void LoadScene(
			string scenePath, EventModifiers eventModifiers)
		{
			if (!string.IsNullOrEmpty(scenePath))
			{
				eventModifiers &= (EventModifiers.Alt | EventModifiers.Control | EventModifiers.Command);
				bool isModeAdditive = ((eventModifiers & (EventModifiers.Control | EventModifiers.Command)) != 0);
				bool isPlayRequest = ((eventModifiers & EventModifiers.Alt) != 0);

				if (isPlayRequest)
				{
					if (EditorApplication.isPlaying)
					{
						EditorApplication.isPlaying = false;
					}

					// Prepare to restore this scene when it terminates.
					if ((scenePath != GetCurrentScenePath())
						&& string.IsNullOrEmpty(SceneRestoreOnPlayStop))
					{
						SceneRestoreOnPlayStop = GetCurrentScenePath();
					}
				}

				// Ensure that the scene-path cache will be refreshed.
				this.cachedScenePaths = null;

				// Load the requested scene, either additively or by itself.
				OpenScene(scenePath, isModeAdditive);

				if (isPlayRequest)
				{
					// Start play mode.
					EditorApplication.isPlaying = true;
				}

				if (this.CloseDialogWhenSceneLoads)
				{
					this.Close();
				}
				else
				{
					this.Repaint();
				}

				// Force a refresh of some standard Editor views, since
				// these don't always repaint automatically after a scene
				// load, for example when loading a new scene while in play
				// mode the Playmode Tint color may not clear automatically.
				RepaintAllViews();
			}
		}

		protected virtual void HighlightOrSelectScene(
			string scenePath, EventModifiers eventModifiers)
		{
			var sceneAsset = AssetDatabase.LoadMainAssetAtPath(scenePath);
			if (sceneAsset)
			{
				Object[] selection = null;
				eventModifiers &= (EventModifiers.Alt | EventModifiers.Control | EventModifiers.Command);
				switch (eventModifiers)
				{
					case 0:
						// Highlight the scene.
						EditorGUIUtility.PingObject(sceneAsset);
						break;

					case EventModifiers.Alt:
						// Single-select the scene.
						selection = new Object[] { sceneAsset };
						break;

					case EventModifiers.Control:
					case EventModifiers.Command:
						// Multi-select the scene.
						selection = Selection.objects;
						if ((selection == null)
							|| (selection.Length <= 0))
						{
							selection = new Object[] { sceneAsset };
						}
						else
						{
							var selectionList = new List<Object>(selection);
							int i = selectionList.IndexOf(sceneAsset);
							if (i < 0)
							{
								// Select the scene.
								selectionList.Add(sceneAsset);
							}
							else
							{
								// Unselect the scene.
								selectionList.RemoveAt(i);
							}

							selection = selectionList.ToArray();
						}

						break;
				}

				if (selection != null)
				{
					Selection.objects = selection;
				}
			}
		}

		protected virtual void DisplayHelpInfo()
		{
			string title = ("About Open Recent Scene v." + Version);
			const string Bullet = "\u2022\u00A0";
			string message =
				Bullet + "Click on the named scene buttons to open the " +
				"various scenes, which are ordered by most recent use.\n" +
				Bullet + "Use keys 0 through 9 as shortcuts for " +
				"scenes 0 to 9, respectively, and Shift+0 through " +
				"Shift+9 for scenes 10 through 19.\n" +
				Bullet + "Ctrl-click the scene buttons to additively " +
				"load a specific scene into the current scene.\n" +
				Bullet + "Alt-click the scene buttons to play that scene " +
				"but restore to the current scene when playing stops; " +
				"Ctrl-Alt-click does an additive load followed by playing.\n" +
				Bullet + "Click on the small dotted buttons to highlight " +
				"the scene in the Project window. Alt-click those buttons " +
				"to select the scene asset, or Ctrl-click them for " +
				"multi-selection of scene assets. Hover over them for a " +
				"tooltip with the path to the scene asset in edit mode.\n" +
				Bullet + "Use F1 to open this help dialog, spacebar to " +
				"toggle \"" + CloseDialogText.ToLower() + "\", or Enter " +
				"to \"" + SearchSceneText.ToLower().TrimEnd('.') + "\".";
			EditorUtility.DisplayDialog(title, message, "OK");
		}

		protected virtual EventModifiers GetCurrentEventModifiers()
		{
			global::UnityEngine.Event currentEvent = global::UnityEngine.Event.current;
			EventModifiers eventModifiers =
				((currentEvent != null) ? currentEvent.modifiers : 0);
			return eventModifiers;
		}

		/// <summary>
		/// Create the label text that appear on the scene-loading buttons.
		/// </summary>
		protected virtual void RefreshScenePathTexts()
		{
			List<string> scenePaths = this.cachedScenePaths;
			string[] sceneTexts = this.cachedSceneTexts;
			if (scenePaths == null)
			{
				sceneTexts = null;
			}
			else
			{
				int sceneCount = scenePaths.Count;
				if ((sceneTexts == null)
					|| (sceneTexts.Length != sceneCount))
				{
					sceneTexts = new string[sceneCount];
				}

				if (sceneCount > 0)
				{
					if (scenePaths.Any(path => string.IsNullOrEmpty(path)))
					{
						sceneTexts = scenePaths.ToArray();
					}
					else if (sceneCount == 1)
					{
						sceneTexts[0] = Path.GetFileNameWithoutExtension(scenePaths[0]);
					}
					else
					{
						for (int i = 0; i < sceneCount; i++)
						{
							sceneTexts[i] = scenePaths[i];
						}

						// Trim all the same parent folder names from the text.
						const char PathSeparator = '/';
						while (true)
						{
							int trimLength = sceneTexts[0].IndexOf(PathSeparator);
							if (trimLength <= 0)
							{
								break;
							}

							string trimText = sceneTexts[0].Substring(0, trimLength);
							if (sceneTexts.Any(text => (trimLength != text.IndexOf(PathSeparator)))
								|| sceneTexts.Any(text => (!text.StartsWith(trimText))))
							{
								break;
							}

							trimLength++;
							for (int i = 0; i < sceneCount; i++)
							{
								sceneTexts[i] = sceneTexts[i].Substring(trimLength);
							}
						}

						// Trim all the scene filename extension from the text.
						if (sceneTexts.All(IsSceneFile))
						{
							for (int i = 0; i < sceneCount; i++)
							{
								sceneTexts[i] = string.Format(
									"{0}{1}{2}",
									Path.GetDirectoryName(sceneTexts[i]),
									PathSeparator,
									Path.GetFileNameWithoutExtension(sceneTexts[i]));
								sceneTexts[i] = sceneTexts[i].TrimStart(PathSeparator);
							}
						}
					}
				}
			}

			this.cachedSceneTexts = sceneTexts;
		}

		protected static bool IsSceneFile(string assetPath)
		{
			return ((assetPath != null)
				&& assetPath.EndsWith(SceneFileExtension));
		}

		/// <summary>
		/// Detect when any change occurs to a scene asset.
		/// </summary>
		protected partial class SceneAssetChangeDetector : AssetPostprocessor
		{
			protected static void OnPostprocessAllAssets(
				string[] importedAssets,
				string[] deletedAssets,
				string[] movedAssets,
				string[] movedFromAssetPaths)
			{
				bool isSceneInvolved = false;

				if ((movedAssets != null)
					&& (movedAssets.Length > 0)
					&& (movedFromAssetPaths != null)
					&& (movedFromAssetPaths.Length == movedAssets.Length))
				{
					// Preserve the ranking of known scenes when moved or renamed.
					int[] sceneIndices =
						movedFromAssetPaths.
						Where(IsSceneFile).
						Select((assetPath, index) => index).
						ToArray();
					isSceneInvolved = ((sceneIndices != null)
						&& (sceneIndices.Length > 0));
					if (isSceneInvolved)
					{
						string currentProject = CurrentProject;
						if (!string.IsNullOrEmpty(currentProject))
						{
							// Load the list of most-recently-used scenes.
							Dictionary<string, List<string>> projectScenes =
								LoadProjectScenePaths(true);
							List<string> scenePaths = projectScenes[currentProject];

							// Replace the paths of recent scenes when known.
							bool isListChanged = false;
							for (int i = (sceneIndices.Length - 1); i >= 0; i--)
							{
								int sceneRank =
									scenePaths.IndexOf(movedFromAssetPaths[i]);
								if (sceneRank >= 0)
								{
									scenePaths[sceneRank] = movedAssets[i];
									isListChanged = true;
								}
							}

							// Save the list of most-recently-used scenes.
							if (isListChanged)
							{
								projectScenes[currentProject] = scenePaths;
								SaveProjectScenePaths(projectScenes);
							}
						}
					}
				}

				if ((!isSceneInvolved)
					&& (importedAssets != null)
					&& (importedAssets.Length > 0))
				{
					isSceneInvolved = importedAssets.Any(IsSceneFile);
				}

				if ((!isSceneInvolved)
					&& (deletedAssets != null)
					&& (deletedAssets.Length > 0))
				{
					isSceneInvolved = deletedAssets.Any(IsSceneFile);
				}

				if (isSceneInvolved)
				{
					// Refresh the scene data.
					OnSceneChanged();
				}
			}
		}

		/// <summary>
		/// Implements a helper method that determines the path of the
		/// current scene in a way that works on multiple versions of Unity.
		/// </summary>
		/// <returns>Returns the path of the current (active) scene.</returns>
		protected static string GetCurrentScenePath()
		{
#if USE_SCENE_API_5_3
			// When using Unity 5.3 and above supporting EditorSceneManager:
			var activeScene = EditorSceneManager.GetActiveScene();
			return (activeScene.IsValid() ? activeScene.path : string.Empty);
#else
			// When using Unity 5.2 and below without EditorSceneManager:
			return EditorApplication.currentScene;
#endif
		}

		/// <summary>
		/// Implements a helper method that loads a scene in a way that
		/// works on multiple versions of Unity, despite API changes.
		/// </summary>
		/// <param name="scenePath">Path of the scene to be opened.</param>
		/// <param name="isModeAdditive">Whether to load scene additively.</param>
		protected static void OpenScene(string scenePath, bool isModeAdditive)
		{
#if USE_SCENE_API_5_3
			// When using Unity 5.3 and above supporting EditorSceneManager:
			if (EditorApplication.isPlaying
				&& (scenePath != null))
			{
				// During play-mode in the Editor (i.e., not regular editing):
				var ignoreCase = System.StringComparison.OrdinalIgnoreCase;
				string sceneName = scenePath;
				if (sceneName.EndsWith(SceneFileExtension, ignoreCase))
				{
					sceneName = sceneName.Substring(0, (sceneName.Length - SceneFileExtension.Length));
				}

				if (sceneName.StartsWith("Assets/", ignoreCase)
					|| sceneName.StartsWith(@"Assets\", ignoreCase))
				{
					sceneName = sceneName.Substring(7);
				}

				var sceneMode = (isModeAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
				SceneManager.LoadScene(sceneName, sceneMode);
			}
			else
			{
				// During regular editing mode (i.e., not in play mode):
				var sceneMode = (isModeAdditive ? OpenSceneMode.Additive : OpenSceneMode.Single);
#if true  // Load scenes in a way that exposes multi-scene features.
				EditorSceneManager.OpenScene(scenePath, sceneMode);
#else  // Backwards compatible scene loading similar to Unity 5.2 and below.
				// Load the requested scene and merge it with the active scene.
				var newScene = EditorSceneManager.OpenScene(scenePath, sceneMode);
				if (newScene.IsValid()
					&& isModeAdditive)
				{
					var activeScene = SceneManager.GetActiveScene();
					if (activeScene.IsValid())
					{
						SceneManager.MergeScenes(newScene, activeScene);
						EditorSceneManager.MarkSceneDirty(activeScene);
					}
				}
#endif
			}
#else
			// When using Unity 5.2 and below without EditorSceneManager:
			if (isModeAdditive)
			{
				EditorApplication.OpenSceneAdditive(scenePath);

				// Mark the scene as modified (workaround implementation).
				Object.DestroyImmediate(new GameObject());
			}
			else
			{
				EditorApplication.OpenScene(scenePath);
			}
#endif
		}

		protected static bool SaveCurrentModifiedScenesIfUserWantsTo()
		{
#if USE_SCENE_API_5_3
			return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
			return EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif
		}

		/// <summary>
		/// Forces a repaint of (almost) all standard views in the Unity Editor.
		/// </summary>
		protected static void RepaintAllViews()
		{
#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017_1)
			// Since the RepaintAllViews method exists on an undocumented
			// internal class, the availability and behavior of the code
			// below needs to be verified in each new version of Unity.
			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#else
			// NOTE: The Inspector, Animation and Console windows can
			// also do with a refresh, but RepaintAnimationWindow is
			// not available on Unity 3.x or older, and it is not yet
			// clear  how to refresh the other views in a general way.
			EditorApplication.RepaintHierarchyWindow();
			EditorApplication.RepaintProjectWindow();
#endif
		}
	}
}

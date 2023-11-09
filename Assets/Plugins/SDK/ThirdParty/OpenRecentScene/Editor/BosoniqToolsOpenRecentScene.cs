//---------------------------------------------------------------------------
// <summary>
// An extension for the Unity Editor to open recent scenes of projects.
// </summary>
// <copyright file="BosoniqToolsOpenRecentScene.cs" company="bosoniq.com">
// Copyright (c) 2017 Bosoniq (tools@bosoniq.com)
// </copyright>
//---------------------------------------------------------------------------

/// <summary>
/// Implements a Unity Editor menu item to load a scene of the project.
/// </summary>
/// <remarks>
/// This code is defined in the global namespace so that
/// EditorWindow.GetWindow can find the data type.
/// </remarks>
[global::UnityEditor.InitializeOnLoad]
internal partial class BosoniqToolsOpenRecentScene
	: global::Bosoniq.Tools.OpenRecentScene
{
#if (UNITY_5_5_OR_NEWER || UNITY_5_4_0 || (!UNITY_5_4_OR_NEWER))
	// Only add the line separator before the custom menu-item when using
	// Unity 5.4.0 or older, or again from Unity 5.5.0 onwards that fixed the
	// regression bug, since from Unity 5.4.1 to 5.4.3 it causes the error:
	//   "Ignoring menu item File because it is in no submenu!"
	// Also, ideally the custom menu-item would have always been placed
	// beneath the "File | Open Scene" item, and luckily that may now finally
	// be possible from Unity 5.4.1 onwards (using priority values 151 to 159)
	// although apparently currently only available on the Unity Linux Editor.
	[global::UnityEditor.MenuItem(
		"File/",
		false,
		SeperatorMenuItemPriority)]
	private static void DummyMethodToGenerateCosmeticMenuItemSeparator()
	{
		// This method does not contain any code, but its applied
		// attribute causes the Unity Editor to draw a seperator line
		// between the regular file menu items and the custom one.
	}
#endif

	[global::UnityEditor.MenuItem(
		"File/Open Recent Scene... %#o",
		false,
		(SeperatorMenuItemPriority + 1))]
	protected static void OpenRecentScene()
	{
		var window = global::UnityEditor.EditorWindow.GetWindow<BosoniqToolsOpenRecentScene>(
			true, "Open Recent Scene");
		window.autoRepaintOnSceneChange = true;
	}
}

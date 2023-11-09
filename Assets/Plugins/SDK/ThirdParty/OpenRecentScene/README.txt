==============================================================================
Open Recent Scene
by Bosoniq Tools
tools@bosoniq.com
==============================================================================

The Open Recent Scene extension is a productivity tool for the Unity Editor
that enables you to quickly open and switch between the scenes of a project.
Scenes are generally presented in the order that they were opened most recently.
Various shortcuts and User Interface buttons provide access to scene-related
features.


==============================================================================
Usage
------------------------------------------------------------------------------

To access the Open Recent Scene dialog, simply press the Ctrl+Shift+O hotkey
on PC or Cmd+Shift+O on Mac, or use the File | Open Recent Scene menu item.
Once the dialog box is visible, several shortcuts and buttons can be used to
access your scenes:

 * Use the keys 0 through 9 to access the top-10 most recent scene, for
   example, press Ctrl+Shift+O followed by 0 to reload the current scene,
   press Ctrl+Shift+O followed by 1 to load the previous scene, or
 * Simply click on the named button that represents each of the scenes.
 * Use the shortcuts Shift+0 through Shift+9 for scenes 10 through 19.
 * Ctrl-click the scene buttons to additively load another specific scene
   into the current scene (which can then be saved as a combined scene).
 * Alt-click the scene buttons to play the selected scene but restore to the
   current scene when playing stops, which can speed up your testing workflow
   when you need to start a different scene from the one you're editing.
   Similarly, Ctrl-Alt-click does an additive load followed by play mode.
 * Click on the small dotted buttons on the right-hand side to highlight the
   specific scene in the Project window, which allows you to quickly find
   any scene in large projects. Alternatively, Alt-click those buttons to
   select the scene asset, or Ctrl-click them for multi-selection of scene
   assets. Hover over them for a tooltip with the path to the scene asset.
 * Use F1 to open a help dialog when you need to check the version number of
   the Open Recent Scene extension or to refresh your memory about shortcuts.

NOTE: For the main Ctrl/Cmd+Shift+O hotkey to become available immediately
      after you initially import the Open Recent Scene package into a project,
      you may need to restart Unity or simply open the main dialog manually
      using its menu item. Thereafter, the hotkey should always work for that
      project. Also, whenever the other shortcut keys do not seem to work,
      verify that the Open Recent Scene dialog box does indeed have input focus.


==============================================================================
Version History
------------------------------------------------------------------------------

Version 1.7.0

 * Update for Unity 2017.1 support.

------------------------------------------------------------------------------

Version 1.6.0 (Apr 25, 2017)

 * Update for Unity 5.6 support.

------------------------------------------------------------------------------

Version 1.5.1 (Dec 28, 2016)

 * Since Unity 5.5.0 fixed the regression bug that caused the menu error in
   Unity 5.4.1 and 5.4.2, the workaround is now ignored for newer versions.

------------------------------------------------------------------------------

Version 1.5.0 (Sep 19, 2016)

 * Update for Unity 5.5 (beta) support.

------------------------------------------------------------------------------

Version 1.4.1

 * Update for Unity 5.4.1 support (including Unity Linux Editor). This also
   circumvents the error "Ignoring menu item File because it is in no submenu!"
 * Move the File | Open Recent Scene menu item to beneath File | Open Scene,
   although apparently currently only possible on the Unity Linux Editor.

------------------------------------------------------------------------------

Version 1.4.0 (Jul 25, 2016)

 * Update for Unity 5.4 (beta) support.

------------------------------------------------------------------------------

Version 1.3.1 (Jan 14, 2016)

 * Update for initial Unity 5.3 support (just a few minor issues still remain
   with multi-scene use-cases, for example, only one scene is reloaded after
   playing stops, and the transient DontDestroyOnLoad scene is shown in the
   scene list during play mode).
 * Update for Unity 4.7 (beta) support.

------------------------------------------------------------------------------

Version 1.2.3 (Oct 28, 2015)

 * Update for Unity 5.2 support.

------------------------------------------------------------------------------

Version 1.2.2 (Jun 16, 2015)

 * Update for Unity 5.1 support.

------------------------------------------------------------------------------

Version 1.2.1 (Sep 02, 2014)

 * Update for Unity 5.0 (beta) support.

------------------------------------------------------------------------------

Version 1.2.0 (Aug 12, 2014)

 * Display a "Loading scene..." message while scenes are being loaded, which
   improves the user experience for large scenes that take a long time to load.

------------------------------------------------------------------------------

Version 1.1.8

 * Fix error on Mac OS when no scene exists in the project.

------------------------------------------------------------------------------

Version 1.1.7 (Aug 07, 2014)

 * Improve informational message being displayed when no scene exists in the
   project, after considering user feedback and additional design discussions.

------------------------------------------------------------------------------

Version 1.1.6 (Jul 07, 2014)

 * Fix issue about "leaked objects in scene" that also flagged scenes as
   modified, which was caused by background styling.
 * Update for Unity 4.6 (beta) support.

------------------------------------------------------------------------------

Version 1.1.5 (Jul 02, 2014)

 * Add background styling to the scene dialog.

------------------------------------------------------------------------------

Version 1.1.4 (May 29, 2014)

 * Update for Unity 4.5 support.

------------------------------------------------------------------------------

Version 1.1.3

 * Improve performance when searching for more scenes.
 * Fix issue where problematic scenes could have been listed from folders
   outside of the Assets folder. This should not be allowed since such scenes
   cannot be highlighted or selected in the Project view of Unity. Furthermore,
   Unity would anyway refuse to save such scenes to their original locations
   if they were to be loaded, because it requires scenes to be under Assets.

------------------------------------------------------------------------------

Version 1.1.2 (May 20, 2014)

 * Fix problem with help button on Mac OS.
 * Improve documentation.

------------------------------------------------------------------------------

Version 1.1.1 (May 12, 2014)

 * Fix problem with scene history when you delete all scenes from a project.

------------------------------------------------------------------------------

Version 1.1.0

 * Fix the menu-item shortcut on Mac OS.
 * Improve performance when the scene dialog is opened by providing an
   additional button to explicitly search for more scenes in the project,
   instead of always performing the search automatically.

------------------------------------------------------------------------------

Version 1.0.0

 * Initial version.


==============================================================================

## Contributing

* Install [**Git**](https://git-scm.com/).
* Install [**Git LFS**](https://git-lfs.github.com/) to work with large files.
* Once Git LFS is installed, activate it with:
    ```sh
    git lfs install
    ```
* Clone this repository:
    ```sh
    git clone https://github.com/Comic-ConMuseum/ccm-augmented-reality.git
    ```

### Setting up Smart Merge

Set up the project to use **UnityYAMLMerge** for resolving merge conflicts by adding the following to your `.git/config` file:
```
[merge]
    tool = unityyamlmerge

[mergetool "unityyamlmerge"]
    trustExitCode = false
    keepTemporaries = true
    cmd = '<path_to_executable>' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```
Replace `<path_to_executable>` with the path to the UnityYAMLMerge tool.

On OS X, this is `/Applications/Unity/Unity.app/Contents/Tools/UnityYAMLMerge`.

On Windows, assuming Unity is installed under `C:\Program Files\Unity`, this is `C:\\Program Files\\Unity\\Editor\\Data\\Tools\\UnityYAMLMerge.exe`.

When you merge or rebase and there is a conflict, run `git mergetool` instead of manually fixing it. The UnityYAMLMerge tool should resolve it in a semantically correct way. If human input is required, it will open the conflict in a fallback merge tool. 

To configure the fallback merge tool, open `mergespecfile.txt`, located in the same directory as the UnityYAMLMerge executable, and follow the instructions in the comments.

### Android Build Instructions

To build the project as an `aar` library that can be consumed by a native Android app:

* Make sure you have **JDK** and the **Andoid SDK** installed and configured under Unity Preferences.
* In Unity, go to **File > Build Settings...** and select Android. 
    * Set Texture Compression to **ETC2**
    * Set Build System to **Gradle**
    * Select **Export Project**
    * Unselect **Development Build**
    * Click **Export** and set the output path. Make sure the generated files are not added to the repository. One possible path is a `Build` directory in the project root, which will be ignored by Git.
* Import the generated project in Android Studio. Accept any Gradle updates you're prompted for. 
* Open the Manifest file (`AndroidManifest.xml`) and comment out/remove the `intent-filter` element.
* In the module `build.gradle` file:
    * Remove the Unity-generated comment at the top to prevent overwriting in subsequent builds.
    * Make sure `google()` and `jcenter()` are included as repositories under both the `buildscript` and `allprojects` properties.
    * Change `apply plugin: 'com.android.application'` to `apply plugin: 'com.android.library'`.
    * Remove the `applicationId` under `defaultConfig` in the `android` property.
* Build the project to generate an `aar` file under `<project_root>/build/outputs/aar/`.
* Import the generated file and any `aar` files under `<project_root>/libs/` in the native Android app.

---

The official Virtual Exhibit app for the Comic-Con Museum can be found at [Comic-Con-Museum/ccm-android](https://github.com/Comic-Con-Museum/ccm-android).

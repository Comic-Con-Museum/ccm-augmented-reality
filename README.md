# Comic-Con Museum Augmented Reality

The Comic-Con Museum's Augmented Reality experience. Built using the Unity 3D engine and the Vuforia Augmented Reality SDK.

## Getting Started

Follow these instructions to get the Unity project up and running on your system for development and testing.

### Prerequisites

Download the latest version of [**Unity**](https://unity3d.com/get-unity/download) for your platform.

During installation, select **Vuforia Augmented Reality Support** and build support for your target platform(s).

### Setup

* Clone or download this repository.
* Launch Unity and select **Open** on the **Projects** tab. Select the root of this repository.
* Under the **Help** menu, select **Vuforia > Check for Updates** and install any updates to the Vuforia Engine.
* Use the **Play** button in the Editor to launch the application in Unity, or select **Build Settings...** under the **File** menu to build it for your target platform.

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

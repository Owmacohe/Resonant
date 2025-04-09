![License: MIT](https://img.shields.io/badge/License-MIT-yellow)
![GitHub stars](https://img.shields.io/github/stars/owmacohe/resonant)
![GitHub downloads](https://img.shields.io/github/downloads/owmacohe/resonant/total)

![Concordant logo](Media/Resonant_Logo_200.png)

# Resonant

*rez • on • ant*

> 1. continuing to sound.
> 2. capable of inducing.
> 3. marked by grandiloquence.

## Overview

**Resonant** is a powerful modular audio manager and sound randomizer. Leveraging capabilities for randomized, looping sound clips, randomized volume and pitch modulation, and customizable audio events/reactions, **Resonant** is extendable and multi-purposed. Though it's powerful trigger/reaction system, design audio events and reactions to real-time code.

## Installation

1. Install the latest release from the [GitHub repository](https://github.com/Owmacohe/Resonant/releases), unzip it, and place the folder into your Unity project's `Packages` folder.
2. Return to Unity, and the package should automatically be recognized and visible in the **Package Manager**.
3. A sample scene can be found at: `Resonant/Example/Example.unity`.
4. Opening this scene may prompt you to install **Text Mesh Pro**. Simply click on **Import TMP Essentials** to do so.

## Usage - General

- **Resonant** is built on top of Unity's `AudioSource`/`AudioClip` system. `AudioSource`s can be controlled through the **Resonant** system using `ResonantSource`, `ResonantRandomizer`, or `ResonantSoundbank` `Components`.
- All three **Resonant** components require an `AudioSource`, and may modify it's parameters *(e.g. volume)* at runtime. However, other `AudioClip` settings *(e.g. 3D sound, mixers, etc.)* can still be changed to improve or customize your audio setup.
- The only **Resonant** component that does not require a `ResonantBehaviour`/`ResonantManager` to function is a `ResonantRandomizer`.
- `ResonantBehaviours` can be used to create complex reaction setups when certain states are triggered. All reactions can be found in `Resonant/Runtime/Reactions`, and are fully documented.

## Usage - ResonantRandomizers
1. Add a `ResonantRandomizer` `Component` to a `GameObject`.
2. Assign it the `AudioCLips` you wish to play.
3. `Delay Modulation` represents the randomly-choen amount of time between each looping clip.
4. `Volume Modulation` represents the range away from the starting volume that the `AudioSource`'s volume can be randomly set to when `AudioClips` are randomly played.
5. `Pitch Modulation` represents the range away from the starting pitch that the `AudioSource`'s pitch can be randomly set to when `AudioClips` are randomly played.
6. If you want the `ResonantRandomizer` to start playing when the game begins, turn on `Loop On Start`.

## Usage - ResonantBehaviours
1. `ResonantBehaviour` data is saved as `ScriptableObjects`, which can be created with `Create/Resonant Behaviour`.
2. Rename your newly created behaviour, then right click on it and select `Edit Resonant Behaviour`. This will open the **Resonant Editor** window.
3. Click on the **Add trigger** button to add a new trigger, and give it an ID. This trigger will be called by you through your code. *(Click on the '-' button to delete this trigger)*
4. Select the `ResonantReaction` of your choice from the dropdown next to the **Add reaction** button, and add it. *(Click on the '-' button to delete this reaction)*
5. `ResonantReactions` can have many fields and functionalities. All reactions can be found in `Resonant/Runtime/Reactions`, and are fully documented.
6. Assign the `ResonantReaction`'s `ID`. This ID should match the ID(s) of the `ResonantSource`s, `ResonantRandomizer`s, and/or `ResonantSoundbank`s in your scene that you want it to affect.
7. Don't forget to save with the **Save** button!
8. In your scene add a `ResonantManager` to a `GameObject`, and assign it the `ResonantBehaviour` that you just created.
9. At runtime, call `ResonantManager.Trigger`, and pass it the ID of the trigger you created. *(Note: `ResonantTrigger` IDs are not the same as `ResonantReaction` IDs)*

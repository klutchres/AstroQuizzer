# AstroQuizzer

A Unity-based space flight game where you pilot a spaceship through an asteroid field and answer astronomy questions to earn fuel.

## Gameplay

Fly through space, dodge debris, and answer quiz questions to keep your fuel tank from running dry. Correct answers refill fuel. Wrong answers (and collisions with space trash) drain it. Run out of fuel and it's game over.

### Controls

| Key | Action |
| --- | --- |
| Mouse / A D | Steer left / right |
| Space | Boost (consumes extra fuel) |
| Escape | Pause |
| Right Click | Hold to slow down time (when answering questions) |

## Features

- Lobby with free-roam pilot movement (click to walk, approach the spaceship to launch)
- Timeline cutscenes for transitions
- Flight with camera shake, roll tilt, and FOV changes based on speed
- Random astronomy trivia questions from a pool of ScriptableObject assets
- Space trash obstacles that respawn in waves
- Audio system with engine pitch, boost SFX, and low-pass music ducking
- Settings menu (VSync, framerate, anti-aliasing, shadows, render scale) saved to PlayerPrefs

## Requirements

- Unity 2019.4.1f1
- Universal Render Pipeline 7.3.1

## Project Structure

| Directory | Contents |
| --- | --- |
| `Assets/Scripts/` | All C# behaviour scripts |
| `Assets/Scenes/` | Game scenes (`Lo.unity`, `Main.unity`) |
| `Assets/Questions/` | Question data assets (ScriptableObjects) |
| `Assets/Prefabs/` | Prefab game objects |
| `Assets/Animations/` | Animation clips |
| `Assets/Audio/` | Sound effects and music |
| `Assets/Fonts/` | Font files |
| `Assets/Materials/` | Materials and shaders |

## Build

Open the project in Unity 2019.4.1f1, open `Assets/Scenes/Lo.unity`, and build via File > Build Settings.

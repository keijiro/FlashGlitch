# FlashGlitch

![GIF](https://github.com/user-attachments/assets/755858f3-56a0-45ed-85af-cfee07c3eb1f)
![GIF](https://github.com/user-attachments/assets/7a643d7c-0972-4bc5-a632-4d393d479b3f)

**FlashGlitch** is a Unity URP renderer feature that provides a trigger-based
glitch effect. It was originally created to add flashing visuals synchronized
with musical rhythm.

## System requirements

- Unity 6000.0 or later
- Universal Render Pipeline (URP)

## How to install

Clone or download this repository and copy `Packages/jp.keijiro.flashglitch`
into your project's `Packages` directory.

## How to use

1. Add **FlashGlitchRendererFeature** to your URP Renderer Asset.
2. Add **FlashGlitchController** to a camera.

The **FlashGlitchController** component has the following public methods:

#### `TriggerEffect1(float strength)`

Triggers the primary effect, which adds a colored glitch overlay to the screen.

#### `TriggerEffect2(float strength)`

Triggers the secondary effect, which adds a brighter glitch overlay to the
screen.

#### `RandomizeHue()`

Randomly changes the hue used by the glitch effects.

## Properties

![inspector](https://github.com/user-attachments/assets/73b805ac-28d5-45db-b1c6-217738b7a517)

#### ReleaseTime1/2

Adjusts how quickly each effect fades out.

#### Hue

The hue used by the glitch effects. You can set it manually with this property
or randomize it with `RandomizeHue()`.

#### Seed

The random seed used by the effects.

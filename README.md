# FlashGlitch

![GIF](https://user-images.githubusercontent.com/343936/176086913-d83ced87-6675-42ca-af02-a84dfda7ff19.gif)

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

### `TriggerEffect1(float strength)`

Triggers the primary effect, which adds a colored glitch overlay to the screen.

### `TriggerEffect2(float strength)`

Triggers the secondary effect, which adds a brighter glitch overlay to the
screen.

### `RandomizeHue()`

Randomly changes the hue used by the glitch effects.

## Properties

### ReleaseTime1/2

Adjusts how quickly each effect fades out.

### Hue

The hue used by the glitch effects. You can set it manually with this property
or randomize it with `RandomizeHue()`.

### Seed

The random seed used by the effects.

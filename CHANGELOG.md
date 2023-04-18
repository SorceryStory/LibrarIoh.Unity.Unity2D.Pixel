# Changelog

## [0.3.0] - 2023-04-18

### [0.3.0] Changed

- Ran a Code Analysis Tool, made fixes to clean code.

### [0.3.0] Removed

- `Unity.UI` code related to pixel classes and operations.

## [0.2.1] - 2023-03-11

### Added

- `Unity.UI` code related to pixel classes and operations.

## [0.2.0] - 2023-01-30

### [0.2.0] Changed

- `PixelCamera`: Changed the way the viewport rect is calculated, now uses Camera's `PixelRect`.

### [0.2.0] Removed

- `PixelCamera`: Removed `ClearColor`.

## [0.1.1] - 2023-01-23

### [0.1.1] Changed

- Solved a bug where loading the script for the first time after playing or stopping would trigger a division by zero by not persisting `PixelSizeMax`.

## [0.1.0] - 2023-01-22

### [0.1.0] Added

- Moved the code for `PixelCamera` from `LibrarIoh.Unity.Unity2D.Camera2D` to here.
- Added a custom editor for `PixelCamera`.
- Created the helper class `PixelOperations`.
- Created `Vector2Extensions`.
- Created `Vector3Extensions`.

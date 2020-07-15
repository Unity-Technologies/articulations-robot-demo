# Changelog

## [0.0.10-preview.8] - 2020-05-15

- Fixed depth support. Depth can now be captured in all formats, but in order to save it to file, 
  you either need to use a raw format, or one of the supported image formats.
  This will result in depth being 8 bits for PNG/JPG etc.
  When consumed at runtime, full precision of R32_SFloat is supported as a capture format.
  Depth is normalized for the distance between the near and far clipping planes, so scale accordingly.

## [0.0.10-preview.7] - 2020-05-04

- Support for Batch readback for both synchronous and asynchronous readback paths. Improves image readback performance by ~20% on player build with GPU support and by ~5-10% on CPU based build. **
- End to end test coverage on usim
- Use NativeArray to pass for encoding (2020.1 and above)
- Support for 2018.4 restored.

(** : Subject to batch size, image resolution and time taken to render the frame i.e if render time is a bottleneck or the readback time)

## [0.0.10-preview.6] - 2020-03-20

Fix uses of deprecated image encoding API.

## [0.0.10-preview.5] - 2020-03-20

Add package dependency to com.unity.simulation.core@0.0.10-preview.8

## [0.0.10-preview.4] - 2020-03-20

Add package dependency to com.unity.simulation.core@0.0.10-preview.6

## [0.0.10-preview.3] - 2020-03-11

Update third party notification text per legal.

## [0.0.10-preview.2] - 2020-03-10

Add Third Party Notices.md for libturbojpeg use.
Fix malformed comment docs in CaptureRenderTexture.

## [0.0.10-preview.1] - 2020-03-09

This package contains funtionality to perform screen capture and data logging.
First package release based on unity package version 1.0.11.
Update license file.
Validation suite fixes.
Add CI files.

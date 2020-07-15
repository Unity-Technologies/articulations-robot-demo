# Changelog

## [0.0.10-preview.14] - 2020-06-16
- Fix for regression issue caused by updating the core tests asmdef file making it incompatible with unity version 2019.1 and below.
- Fix for c# job scheduling off of main thread (ESIM-1338, ESIM-1337).

## [0.0.10-preview.13] - 2020-05-14

- Fix for regression, uploading the player log to logs location.
- Remove Debug.Log statement causing unintended output.

## [0.0.10-preview.12] - 2020-05-04

C# Job system support
Retry attempt for failed uploads in Signlynx.
End to end tests on USim
TimeLogger: FPS calculated correctly.
TimeLogger: Elapsed times aligned correctly from start.
Added TimeLimit component (exits when time limit is up)
Added FrameLimit component (exits application after a number of frames)
Added Hardware Specs for Mac and Linux.
Added ContinuousEvents (piecewise aggregated signal for given metric)
Bug: ChunkedStream flushes on shutdown.
Removed FakeDataConsumer to reduce noise.

## [0.0.10-preview.11] - 2020-04-28

Update .npmignore to include upm-ci.log

## [0.0.10-preview.10] - 2020-04-27

Treat full Player.Log and profiler log as artifact

## [0.0.10-preview.9] - 2020-04-10

Support new storage structure that splits data and logs

## [0.0.10-preview.8] - 2020-03-16

Fix warnings in package.

## [0.0.10-preview.7] - 2020-03-16

Add QAReport.md and README.md to .npmignore

## [0.0.10-preview.6] - 2020-03-06

Fixed Obsolete messages to work for validation.

## [0.0.10-preview.5] - 2020-03-06

Fix date format.
Fix changelog format.

## [0.0.10-preview.4] - 2020-03-06

Reverted rename of LICENSE.md file which caused package validation to fail.

## [0.0.10-preview.3] - 2020-03-05

Renamed LICENSE.md to LICENSE_S.md
Updated license with final text.

## [0.0.10-preview.2] - 2020-03-05

Updated for testing continuous integration.

## [0.0.10-preview.1] - 2020-02-11

This package contains core funtionality required by unity simulations.
First package release based on unity package version 1.0.11.

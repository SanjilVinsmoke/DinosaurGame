# EPIC 0 — Project Cleanup & Technical Baseline

## Outcome
The template now boots toward the dinosaur product Home flow instead of automatically preparing the Pill/Hole level prototype. Existing template services remain available while new product code is introduced behind `DinosaurGame.*` namespaces and assembly boundaries.

## Inventory
- Product scenes: `LoadingScene`, `GameplayScene`.
- Reused template services: save/player data, popup flow, sound, vibration, currencies, daily reward, shop, loading, settings.
- Legacy prototype: `Gameplay/Level/Level.cs`, `Pill.cs`, `Hole.cs`, `LevelController`, level win/lose UI.
- Vendor areas currently present include Polyart, GrigoriyArx, Cartoon GUI content, CustomInspector, CustomTween and other imported packages.
- Unity version: 6000.6.0f1.

## Implemented baseline
- Added product assembly boundaries: Core, Gameplay, UI, Editor and EditMode Tests.
- Added `DinosaurGame.*` root namespaces for all new product code.
- Home boot no longer prepares or instantiates legacy Pill/Hole levels.
- Legacy level loading no longer uses `Resources.Load`; it uses an explicit `LevelCatalog` reference.
- Removed runtime `DestroyImmediate` from `LevelController`; transform clearing uses `Destroy` in play mode and editor-only `DestroyImmediate` outside play mode.
- Loading flow no longer depends on CustomTween; progress follows the actual async scene operation with a bounded minimum display time.
- Debug-console creation is excluded from non-development player builds.
- Added null-safe startup guards for Home presentation and testing config.
- Added an Editor baseline validator and an initial EditMode test.
- Added an Editor PackageManager bootstrap for required Polyart dependencies discovered during the Unity 6.6 baseline audit.

## Legacy policy
Legacy level classes are retained temporarily to avoid breaking serialized scene/prefab references. They are no longer part of the default product boot path. Delete them only after dependent prefabs/scenes have been migrated or removed.

## Remaining acceptance verification
- Unity compile must be clean after Polyart package dependencies resolve.
- Run `Dinosaur Game > Validate Epic 0 Baseline`.
- Verify Loading -> Home -> Settings -> save/load smoke flow in Play Mode.
- Capture steady-idle Profiler baseline in the product Home scene and confirm project-code GC allocation target.
- Create/curate the dedicated performance test scene once the starter dinosaur/environment vertical-slice assets are wired, so the scene measures representative content rather than an empty template.

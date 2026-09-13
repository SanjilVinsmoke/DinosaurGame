# Dinosaur Pet Game — Product Requirements Document

**Project:** DinosaurGame  
**Engine:** Unity 6.6 / URP  
**Primary Platforms:** iOS + Android  
**Genre:** Casual 3D dinosaur pet simulation / collection  
**Audience:** Broad casual audience, family-friendly, short-session mobile play  
**Product Goal:** Build a polished, performant dinosaur pet game by reusing the existing mobile-game template where it is valuable and replacing the current prototype gameplay with a clean, scalable pet-sim architecture.

## 1. Product Vision

The player adopts, raises, plays with, customizes, and collects stylized dinosaurs in a colorful natural sanctuary. The experience should feel warm, tactile, readable, and immediately understandable on mobile. The game is not a deep survival simulator; it is a lightweight pet fantasy built around emotional attachment, short recurring activities, visual progression, and collecting new dinosaurs.

The product should combine four pillars:
1. **Bond** — the dinosaur visibly reacts to care, touch, feeding, play, and neglect.
2. **Progress** — pets level up, unlock needs, tricks, cosmetics, habitats, and growth stages.
3. **Collection** — multiple dinosaur species and babies create a long-term collection goal.
4. **Sanctuary** — the player gradually improves a scenic home using Dreamscape Nature Mountains and stylized environmental dressing.

## 2. Product Principles

- Mobile-first, 60 FPS target on mid-range devices.
- One obvious primary interaction per screen.
- Short loops: 30 seconds to 5 minutes.
- Pet reactions must communicate state without requiring text.
- No unnecessary runtime systems, reflection-heavy frameworks, or scene-wide polling.
- Prefer data-driven ScriptableObjects over hard-coded content.
- Reuse the current template for save data, popup flow, settings, sound, rewards, and currencies only where it remains clean.
- Keep third-party assets isolated behind project-owned adapters and prefabs.
- Gameplay code belongs under a project namespace and project-owned folders.
## 3. Confirmed Asset Stack

The product plan assumes these user-selected assets are available in the project or will be imported:

- **URP Toon Shader** — primary rendering look for dinosaurs and selected props.
- **Dinosaurus Pack with Babies** — core dinosaur characters and baby variants.
- **AllSky Free** — sky and atmosphere presets.
- **Cartoon GUI Pack** — runtime UI visual kit.
- **DOTween (HOTween v2)** — UI motion, feedback, lightweight authored sequences.
- **Dreamscape Nature Mountains** — sanctuary terrain and scenic level dressing.

Asset rule: imported vendor content stays untouched wherever practical. Create project-owned prefabs/material variants/config assets that wrap vendor content so package upgrades do not destroy game-specific work.

## 4. Current Project Baseline

The existing project already contains a reusable casual-mobile framework: GameManager, level flow, PlayerData persistence, currencies, energy, daily rewards, shop, sound, vibration, VFX, popup infrastructure, settings, debug UI, and reusable prefabs. The current gameplay layer is still a small drag-and-drop pill/hole prototype and is not the target game.

### Keep / Adapt
- PlayerData persistence and save lifecycle.
- Popup controller and popup base architecture after cleanup.
- Sound, vibration, loading and settings controllers.
- Gold / diamond resource patterns if monetization later uses them.
- Daily reward and shop framework after product-specific redesign.
- Existing mobile safe-area and canvas scaling helpers.

### Replace / Remove
- Pill, Hole and current drag-and-drop gameplay.
- Level-number win/lose loop as the primary progression model.
- `Resources.Load("Levels/...")` level spawning architecture.
- Generic template screens or systems that are unused by the dinosaur product.
- Obsolete duplicate tween systems once DOTween migration is complete.
- Dead sample/debug/vendor code from the shipping assembly path.
## 5. Core Gameplay Loop

1. Enter sanctuary/home.
2. See active dinosaur and its current needs.
3. Interact through touch: pet, feed, wash, play, train, decorate.
4. Need meters and mood improve; dinosaur responds with animation, sound, particles, and facial/body feedback.
5. Earn bond XP, player XP, coins and occasional premium rewards.
6. Unlock new foods, toys, cosmetics, habitat props, tricks, species and growth milestones.
7. Return later for refreshed needs, daily goals, gifts and collection progress.

### Session Rhythm
- **10–30 sec:** check pet mood, tap reaction, collect gift.
- **1–3 min:** feed/wash/play interaction.
- **3–5 min:** training/minigame + reward + upgrade.
- **Long-term:** unlock species, complete collection, grow babies, expand sanctuary.

## 6. Pet State Model

Each owned dinosaur has persistent state:
- Dinosaur definition ID / species.
- Unique pet ID.
- Display name.
- Growth stage: baby / juvenile / adult where supported.
- Level and bond XP.
- Hunger.
- Happiness.
- Cleanliness.
- Energy.
- Optional affection / trust score.
- Equipped cosmetic IDs.
- Unlocked tricks.
- Last-care timestamps for offline decay.
- Favorite food / toy modifiers where configured.

Needs should decay using timestamps, not per-frame timers, so offline progression is cheap and deterministic.

## 7. Dinosaur Behaviour

The active pet uses a lightweight state machine rather than large MonoBehaviour Update graphs.

Core states:
- Idle
- Wander
- LookAtPlayer
- Curious
- Hungry
- Happy
- Sad
- Sleep
- Eat
- Drink
- PetReaction
- WashReaction
- Play
- Trick
- Celebrate
- Transition / locomotion

Behaviour must support animation-event hooks but gameplay timing should be owned by code/data, not hidden exclusively inside animation clips.

Dinosaur definitions are ScriptableObjects describing model prefab, animator/controller, icon, species metadata, animation map, audio set, stat tuning, growth links and presentation settings.
## 8. Interaction Systems

### Petting
- Swipe/touch on pet hit regions.
- Track stroke distance and direction, not raw tap spam.
- Trigger escalating reactions without restarting animation every frame.
- Positive feedback: face/body animation, particles, sound, small UI response.

### Feeding
- Select food from a lightweight tray/inventory.
- Drag/tap food to pet.
- Food resolves through data: hunger value, preference bonus, XP, cooldown/cost.
- Pet transitions into eat animation and consumes one logical item only once.

### Washing / Cleaning
- Dirty state is represented visually with controlled overlays/decals/particles.
- Touch interaction fills cleaning progress.
- Effects must be pooled; no Instantiate/Destroy loop per swipe.

### Play / Toys
- Toys are data-driven interactables.
- Initial toys: ball, tug/chew toy, simple target/chase object.
- Toy sessions are bounded mini-interactions, not open-ended physics chaos.

### Training / Tricks
- Unlock a small command set per dinosaur.
- Tap/gesture prompt -> animation -> success feedback -> bond reward.
- Tricks become collection/progression goals.

### Sleep
- Energy gates some high-reward activities.
- Sleep can be an explicit interaction or offline recovery.
- Never force long real-time waits as the only way to continue playing.
## 9. UX / Screen Map

Primary runtime UI should follow the project's existing mobile Canvas/uGUI pattern unless a later audit shows a stronger reason to migrate. Cartoon GUI Pack supplies the visual language; project prefabs own layout and behavior.

Core screens:
- Loading / boot
- Sanctuary / Home HUD
- Dinosaur collection
- Dinosaur detail / switch pet
- Food inventory
- Toy inventory
- Wardrobe / cosmetics
- Habitat decoration
- Daily goals / rewards
- Shop
- Settings
- Optional lightweight photo mode later

### Home HUD
Always-visible information should be minimal:
- Active pet portrait/name/level
- Four compact need indicators
- Currency summary
- Context interaction tray
- Collection / shop / settings access

Do not place every system on-screen at once. Secondary systems open as focused popups.

### Motion Language
Use DOTween for authored UI transitions and feedback:
- 0.12–0.25 s micro-interactions
- 0.20–0.40 s popup transitions
- Punch/scale sparingly for rewards
- Kill/reuse tweens when objects are disabled
- Avoid perpetual tween loops on large UI hierarchies
## 10. Visual Direction

### Rendering
- URP is the required render pipeline.
- URP Toon Shader defines the main character/material look.
- Avoid mixing realistic PBR characters with toon-lit dinosaurs.
- Create project-owned material presets for skin, eyes, mouth, accessories and props.
- Use one consistent outline policy; avoid expensive full-screen outlines if per-material outlines achieve the look.

### Environment
Dreamscape Nature Mountains provides large-form terrain, cliffs, rocks, vegetation and distant silhouettes. The sanctuary should feel scenic but compact enough for mobile.

Environment composition layers:
- Playable foreground sanctuary.
- Mid-ground cliffs, trees and habitat structures.
- Distant mountain cards/meshes and sky.
- Controlled fog/color grading to merge layers.

AllSky Free provides sky presets. Select a small curated set rather than loading every sky at runtime.

### Character Presentation
- Dinosaurs are the visual priority.
- Babies receive slightly stronger expressive framing and camera proximity.
- Keep readable silhouettes against the environment.
- Avoid dense foliage behind interaction zones.
- Camera should preserve full-body readability during care actions.
## 11. Technical Architecture

Target project-owned structure:

```text
Assets/_Project/
  Art/
  Audio/
  Config/
  Prefabs/
  Scenes/
  Scripts/
    Core/
    Data/
    Dinosaurs/
    Interactions/
    Progression/
    Economy/
    UI/
    Environment/
    Audio/
    VFX/
    Editor/
  Settings/
  Tests/
```

Third-party packages/assets remain in clearly isolated vendor folders. Project code must not depend on vendor folder internals more than necessary.

Use assembly definitions for Core, Gameplay, UI, Editor and Tests once the cleanup epic begins. Dependencies should flow inward: vendor adapters -> project systems, never the reverse.
## 12. Data Architecture

Use ScriptableObjects for authoring static content and serializable runtime models for player-owned state.

Static definitions:
- DinosaurDefinition
- DinosaurGrowthDefinition
- FoodDefinition
- ToyDefinition
- TrickDefinition
- CosmeticDefinition
- HabitatPropDefinition
- RewardDefinition
- EconomyConfig
- PetNeedsConfig
- CameraPreset
- AudioSet

Runtime/player data:
- PlayerProfile
- OwnedDinosaurData[]
- InventoryData
- CurrencyData
- UnlockData
- DailyProgressData
- SettingsData

Do not serialize direct prefab references into save files. Save stable IDs and resolve them through catalogs/config assets.

Version all save data. Every persistent schema change needs a migration path or safe defaulting behavior.
## 13. Performance Budgets

Initial mobile targets, to be validated on representative Android and iOS devices:

- **Frame rate:** 60 FPS target; 30 FPS fallback quality tier only when required.
- **Main-thread CPU:** aim < 12 ms typical gameplay at 60 FPS.
- **GPU:** aim < 14 ms typical gameplay at 60 FPS.
- **GC allocations:** 0 B/frame during steady sanctuary idle and normal pet interaction.
- **GC spikes:** avoid > 1 MB during ordinary interaction/UI transitions.
- **Draw calls:** target < 120 visible batches on mid tier; stretch ceiling 180 in scenic views.
- **SetPass calls:** target < 80.
- **Visible triangles:** target roughly 150k–350k depending device tier and environment view.
- **Active skinned meshes:** keep active pets/NPC dinosaurs tightly bounded.
- **Dynamic lights:** one primary directional; additional realtime lights only for exceptional effects.
- **Realtime shadows:** one main light; tune cascade count/distance per quality tier.
- **Particle systems:** pooled and bounded; no unbounded emission.
- **Physics:** interaction colliders simple; avoid unnecessary MeshColliders and per-frame raycasts.
- **UI:** minimize overdraw, nested masks, layout rebuild storms and full-screen transparent layers.

These are engineering gates, not promises about final device performance; profiler captures decide final budgets.
## 14. Epic Roadmap

### EPIC 0 — Project Cleanup & Technical Baseline
**Goal:** convert the template into a stable dinosaur-game foundation before feature work.

Deliverables:
- Inventory existing systems, scenes, prefabs, packages and vendor assets.
- Remove dead prototype gameplay and unused shipping code.
- Establish project namespaces and folder conventions.
- Add asmdefs for clean compile boundaries.
- Migrate runtime motion from duplicate/custom tween usage toward DOTween where appropriate.
- Remove `DestroyImmediate` from runtime paths.
- Replace broad `Resources.Load` patterns with explicit catalogs/references; Addressables is optional only if content scale justifies it.
- Isolate debug/editor tooling from player builds.
- Add bootstrap validation and null-safe startup flow.
- Create performance test scene and baseline profiler capture.

Acceptance:
- Zero compile errors.
- No missing-reference errors on boot.
- Existing loading/home/settings/save flow still operates.
- Prototype Pill/Hole gameplay no longer drives product flow.
- Steady idle has zero recurring managed allocations from project code.
### EPIC 1 — Dinosaur Vertical Slice
**Goal:** one dinosaur, one sanctuary, complete care loop.

Deliverables:
- DinosaurDefinition + catalog.
- ActiveDinosaur runtime controller.
- Needs model: hunger, happiness, cleanliness, energy.
- Persistent OwnedDinosaurData.
- Idle/wander/react/eat/sleep states.
- Petting interaction.
- Feeding interaction with 3 food items.
- Basic wash interaction.
- One toy interaction.
- Bond XP and level-up feedback.
- Save/load of all pet state.
- Camera framing for interaction modes.

Acceptance:
- New profile receives a starter baby dinosaur.
- Player can complete pet/feed/wash/play loop without debug controls.
- Need state survives app restart.
- Pet reactions cannot double-consume rewards/items.
- No gameplay action causes repeated Instantiate/Destroy spikes.
### EPIC 2 — Art Integration & Toon Look
**Goal:** establish the shippable visual language early, not at the end.

Deliverables:
- URP Toon Shader material presets for dinosaur skin/eyes/mouth/accessories.
- Dinosaurus Pack prefab wrappers with consistent scale, colliders and animation adapters.
- AllSky curated sky profiles.
- Lighting preset and quality-tier variants.
- Toon-compatible environment material treatment where required.
- Camera, fog and grading pass to unify character/environment.
- VFX style guide for hearts, clean, food, bond XP, level-up and unlocks.

Acceptance:
- Starter dinosaur reads clearly against sanctuary background.
- No pink/missing materials in target scenes.
- Material variants do not edit vendor originals.
- Visual profile remains within mobile GPU budget.
- One-click quality tiers preserve the art direction.

### EPIC 3 — UI/UX Rebuild with Cartoon GUI Pack
**Goal:** replace generic template presentation with a cohesive pet-game interface.

Deliverables:
- Home HUD.
- Need-meter components.
- Interaction tray.
- Dinosaur collection screen.
- Pet detail screen.
- Food/toy inventory panels.
- Reward and level-up presentation.
- Settings migration.
- DOTween UI transition library.
- Safe-area and aspect-ratio validation.
Acceptance:
- All core care actions are reachable in <= 2 taps from Home.
- HUD remains readable on common phone aspect ratios.
- UI transitions do not block input longer than intended.
- No duplicate tween stacks after repeated popup open/close.
- No noticeable Canvas rebuild spikes during idle.

### EPIC 4 — Sanctuary & Environment
**Goal:** turn Dreamscape Nature Mountains into a mobile-friendly dinosaur home, not a raw asset-demo scene.

Deliverables:
- Sanctuary scene with clear interaction zone.
- Curated Dreamscape terrain/rock/vegetation set.
- Distant mountain composition.
- Lightweight water feature if budget allows.
- Habitat zones for future species.
- Prop anchors for customization.
- Occlusion/culling strategy.
- LOD setup and shadow-distance tuning.
- Navigation/wander boundaries for pets.
- Environment audio zones.

Acceptance:
- Main interaction area holds target frame rate on reference mid-tier hardware.
- Distant scenery cannot dominate draw/triangle cost.
- Pet never wanders outside the playable sanctuary.
- Interaction camera never clips through major environment pieces.
### EPIC 5 — Collection, Growth & Progression
**Goal:** create the long-term reason to return.

Deliverables:
- Dinosaur collection catalog.
- Locked/unlocked/owned states.
- Species unlock conditions.
- Baby-to-next-stage growth progression where art supports it.
- Bond levels and rewards.
- Player profile level.
- Trick unlock progression.
- Collection completion rewards.
- Duplicate-safe unlock logic.
- Content balancing tables/configs.

Acceptance:
- Progression is fully data-driven.
- Adding a new dinosaur does not require editing core gameplay classes.
- Unlock state persists safely across save version changes.
- Growth never destroys ownership/cosmetic state.

### EPIC 6 — Economy, Rewards & Retention
**Goal:** adapt the template economy around pet care rather than level completion.

Deliverables:
- Coin earning/spending loop.
- Optional premium diamond use retained only where product needs it.
- Daily rewards redesigned for pet game.
- Daily/weekly care goals.
- Streaks with forgiving recovery rules.
- Reward chest/gift system.
- Shop categories for food, toys, cosmetics and habitat props.
- Offline return gift/summary.
Acceptance:
- Economy never depends on legacy level-index progression.
- Reward grants are idempotent where double callbacks are possible.
- Care actions feel rewarding without requiring ads or purchases.
- All prices/rewards are configurable without code changes.

### EPIC 7 — Toys, Training & Mini Activities
**Goal:** deepen interaction without turning the project into many disconnected games.

Initial activities:
- Ball fetch/chase.
- Simple reaction/timing trick training.
- Follow-the-target activity.
- Food preference discovery.

Rules:
- Reuse common activity lifecycle and reward interfaces.
- Keep each activity under a bounded runtime budget.
- Avoid separate scene loads unless the activity genuinely needs them.
- Activities should reinforce bond/progression rather than be isolated score modes.

Acceptance:
- At least 3 repeatable activities share the same activity framework.
- Each has clear start, success/failure/cancel and cleanup paths.
- Exiting mid-activity restores normal pet state safely.
### EPIC 8 — Performance, Memory & Build Hardening
**Goal:** make performance a product requirement, not a final cleanup task.

Deliverables:
- Device quality tiers.
- URP asset variants for low/mid/high.
- LOD and culling audit.
- Texture compression/max-size audit.
- Shader variant audit.
- Skinned-mesh/animator cost audit.
- UI overdraw/layout audit.
- Audio import/load-type audit.
- Object pooling for recurring VFX/interactables.
- Memory Profiler captures.
- CPU/GPU Profiler baselines.
- Build-size report and stripping review.

Acceptance:
- No project-code GC allocations in steady idle.
- No uncontrolled Instantiate/Destroy loops during repeated care actions.
- Mid-tier target maintains stable frame pacing in sanctuary.
- Memory returns close to baseline after repeatedly opening/closing major screens.
- Build contains no obsolete prototype scenes or sample content required only for development.
### EPIC 9 — Audio, Haptics & Feedback
**Goal:** make the pet feel alive and responsive.

Deliverables:
- Dinosaur vocal sets per species/state.
- UI SFX set mapped to Cartoon GUI interactions.
- Feeding, washing, toy and reward SFX.
- Ambient sanctuary loop and environmental one-shots.
- Haptic feedback policy for positive actions, unlocks and warnings.
- Mixer routing and volume settings.
- Audio concurrency limits for repeated pet reactions.

Acceptance:
- Repeated touch cannot produce uncontrolled overlapping vocal/audio spam.
- Music/ambience/SFX/UI volumes are independently configurable.
- Haptics can be disabled globally.
- Audio import settings are appropriate for short SFX vs music/ambience.

### EPIC 10 — QA, Tests & Release Candidate
**Goal:** make the project safe to iterate and ship.

Deliverables:
- EditMode tests for data/config/economy/save migration.
- PlayMode tests for boot, pet spawn, interactions and persistence.
- Smoke-test scene and automated build validation.
- Missing-reference/config validation tools.
- Save corruption fallback.
- Pause/focus/background lifecycle tests.
- Low-memory/reload testing.
- Android/iOS device matrix and release checklist.
Acceptance:
- Fresh install, upgrade install and corrupted-save fallback are tested.
- Automated Unity batch compile remains clean.
- Core PlayMode smoke tests pass before release builds.
- No development-only UI/debug shortcut appears in production builds.
- Crash-free boot and first-session care loop on target devices.

## 15. Existing Template -> New Product Mapping

| Existing Template System | Decision | Dinosaur Product Use |
|---|---|---|
| GameManager | Refactor | App/game flow coordinator; remove level-win assumptions |
| PlayerDataController | Keep + modernize | Save lifecycle, versioning, pet ownership/state |
| PopupController | Keep | Focused modal/screens and overlays |
| SoundController | Keep + profile | Music/SFX routing and pet audio |
| VibrationController | Keep | Haptic policy |
| DailyRewardController | Adapt | Care-return rewards |
| ItemController | Adapt | Food/toys/cosmetics inventory |
| Gold/Diamond handlers | Adapt | Product economy only |
| EnergyController | Re-evaluate | Pet energy is not necessarily player-energy gating |
| LevelController | Remove from core | Replace with sanctuary/content progression |
| Pill/Hole | Remove | Prototype only |
| CustomTween | Phase out | DOTween for product-authored runtime/UI motion |
## 16. Code Cleanup Rules

The cleanup must preserve useful template behavior while reducing coupling.

Rules:
- No new global singleton unless there is a genuine app-wide lifetime requirement.
- Prefer explicit serialized dependencies or constructor-like initialization for plain C# services.
- MonoBehaviours own Unity lifecycle; plain C# models/services own game logic.
- Avoid static mutable gameplay state where save/profile state is the correct owner.
- Avoid scene searches in gameplay loops.
- Avoid `GetComponent` in Update; cache required references.
- Avoid `Resources.Load` for frequently accessed product content.
- No `DestroyImmediate` in runtime code.
- No hidden business logic in UI classes.
- No vendor namespace in core product domain models.
- All recurring subscriptions unsubscribe deterministically.
- All DOTween sequences are killed/recycled on disable/destroy.
- Runtime logs are gated or removed from release paths.

## 17. Proposed Core Runtime Modules

- `AppBootstrap` — initialization order and service registration.
- `GameFlowService` — boot/home/activity mode transitions.
- `SaveService` — load/save/version/migration.
- `DinosaurCatalog` — static definitions by stable ID.
- `DinosaurCollectionService` — owned pets and active pet selection.
- `PetNeedsService` — needs decay/recovery calculations.
- `PetInteractionService` — validates/executes care actions.
- `InventoryService` — food/toy/cosmetic quantities.
- `RewardService` — single grant path for all rewards.
- `EconomyService` — currencies and prices.
- `ProgressionService` — bond/player unlock progression.
- `AudioService` / existing controller adapter.
- `HapticsService` / existing vibration adapter.
## 18. Asset-Specific Integration Requirements

### URP Toon Shader
- Create a project material library; never tune every dinosaur independently.
- Support skin base color, shadow threshold/ramp, rim light, outline and optional specular controls.
- Strip unused shader features/variants for mobile builds.
- Avoid transparent toon materials except where truly required.
- Verify SRP Batcher compatibility where the shader supports it.

### Dinosaurus Pack with Babies
- Wrap each usable model in a project-owned prefab.
- Normalize scale, forward axis, root transform and collider conventions.
- Do not modify source FBX/model imports casually; use prefab overrides/adapters.
- Build an animation capability matrix per species before promising shared tricks.
- Disable off-screen/unused Animator work where safe.

### AllSky Free
- Curate only approved skies into game presets.
- Keep sky changes out of per-frame code.
- Prefer baked/static ambience contribution over extra real-time lighting complexity.

### Cartoon GUI Pack
- Build reusable project prefabs: PrimaryButton, SecondaryButton, CurrencyChip, NeedBar, Card, ModalHeader, ItemTile.
- Do not create one-off duplicated UI hierarchies for every screen.
- Atlas sprites and control texture import sizes.
### DOTween
- Use for finite authored transitions, not as a replacement for every gameplay timer.
- Centralize common UI animation recipes.
- Use recyclable tweens where practical.
- Kill tweens on owner disable/destroy.
- Avoid anonymous infinite loops that survive screen teardown.
- Never stack repeated button/popup tweens without completing/killing prior instances.

### Dreamscape Nature Mountains
- Treat source demo scenes as reference only.
- Build a new sanctuary scene from curated prefabs/materials.
- Author LOD Groups for expensive scenic meshes where absent.
- Reduce shadow casters to gameplay-relevant objects.
- Use GPU instancing/SRP Batcher-friendly materials where available.
- Limit grass/foliage density around the pet interaction space.
- Distant objects should use cheaper LODs/materials and no unnecessary colliders.

## 19. MVP Content Target

The first shippable content target should remain intentionally small:
- 3 dinosaur species minimum.
- At least 2 baby dinosaurs prominently featured.
- 1 main sanctuary.
- 4 needs: hunger, happiness, cleanliness, energy.
- 5 foods.
- 3 toys/activities.
- 3–5 tricks across the starter content set.
- 6–10 cosmetics if the models support clean attachment points.
- 8–12 habitat decorations.
- Daily reward + simple daily care goals.
- Collection book and basic progression.

Do not expand species count until the first dinosaur's full care loop is polished and profiler-clean.
## 20. MVP Non-Goals

Explicitly out of scope for the first production milestone:
- Open-world exploration.
- Multiplayer/social spaces.
- Complex breeding/genetics.
- Realistic survival simulation.
- Combat.
- Large procedural worlds.
- User-generated levels.
- Dozens of simultaneous dinosaurs.
- Full housing/building sandbox.
- Heavy backend dependency for the core offline pet loop.

These can be reconsidered only after the core pet loop, retention loop and mobile performance are proven.

## 21. Key User Stories

- As a new player, I can receive and name my first baby dinosaur within the first session.
- As a player, I can understand what my dinosaur needs without reading a tutorial paragraph.
- As a player, touching my dinosaur produces immediate expressive feedback.
- As a player, I can feed, wash and play with my dinosaur in a few taps.
- As a returning player, I can see what changed while I was away.
- As a collector, I can see locked dinosaurs and understand how to unlock them.
- As a progression player, my care actions visibly increase bond and unlock new content.
- As a decorator, I can personalize the sanctuary without breaking the pet interaction space.
- As a mobile player, menus and care interactions remain responsive on a mid-range phone.
## 22. First-Time User Experience

Target first 5 minutes:
1. Fast boot into a clean sanctuary reveal.
2. Starter baby dinosaur appears and approaches the camera/player.
3. Player performs one guided petting action.
4. A hunger cue appears; player feeds one item.
5. Bond level increases with satisfying feedback.
6. Player names the dinosaur.
7. Home HUD opens with only the next useful actions emphasized.
8. A future dinosaur silhouette/collection goal is teased.

Tutorial rules:
- Teach by interaction, not modal text walls.
- Never block every screen element with a tutorial overlay.
- Tutorial prompts disappear permanently after completion.
- Any tutorial action must be safely repeatable if interrupted by pause/backgrounding.

## 23. Camera Requirements

- Default sanctuary camera prioritizes the active dinosaur.
- Care actions can blend to authored close-up presets.
- Camera movement uses damping and bounded transitions; no abrupt snapping.
- Do not let large dinosaur species clip through the camera near plane.
- Camera presets are data-driven per species/size class.
- Photo mode is deferred, but framing architecture should not prevent it later.
## 24. Save / Offline Progression Requirements

- Save on important state changes plus app pause/focus lifecycle.
- Debounce frequent writes; petting should not write disk every stroke.
- Store UTC timestamps for offline need decay/recovery.
- Clamp offline progression to configured maximum windows to prevent absurd values.
- Validate loaded IDs against current catalogs.
- Preserve unknown/new fields where practical through schema versioning.
- Maintain a recoverable backup of the previous valid local save.
- Never trust device-clock changes blindly for premium/reward systems.

Initial product can remain offline-first. Cloud save is a future enhancement, not an MVP dependency.

## 25. Economy Guardrails

- Core care actions are always available without premium currency.
- Coins primarily buy consumables and accessible cosmetic/decor progression.
- Premium currency, if retained, is for optional cosmetics/convenience rather than pet survival.
- Avoid punitive need decay that pressures purchases.
- Do not create conflicting player-energy and pet-energy systems unless testing proves a real need.
- Every reward and price comes from authored config.
- Every spend/grant operation has a single validated code path.

## 26. Content Authoring Workflow

A designer should be able to add content without touching core code:
1. Create a definition asset.
2. Assign stable ID and presentation metadata.
3. Assign wrapped prefab/icon/audio/animation capabilities.
4. Add definition to the appropriate catalog.
5. Run editor validation.
6. Preview in a dedicated content test scene.

Catalog validators must detect duplicate IDs, missing prefabs, missing icons, invalid growth links, bad prices/rewards and unsupported animation requirements.
## 27. Risk Register

### High: Asset style mismatch
Dreamscape may be more realistic than the toon dinosaurs. Mitigation: curated materials, fog/color grading, simplified lighting, distance treatment, and selective use rather than importing demo scenes wholesale.

### High: Dinosaur animation inconsistency
Different species may not share identical animation sets. Mitigation: capability-driven definitions and fallback behaviors; never assume every pet supports every trick.

### High: Template coupling
Current systems assume level/win/lose progression. Mitigation: Epic 0 separates generic infrastructure from product flow before feature expansion.

### Medium: UI overdraw / animation cost
Cartoon GUI assets can encourage layered transparent UI. Mitigation: atlas, flatten decorative layers where possible, profile Canvas rebuild and overdraw, bound DOTween loops.

### Medium: Environment GPU cost
Mountain/vegetation assets can exceed mobile budget. Mitigation: LODs, shadow reduction, material consolidation, culling, shorter view distance and quality tiers.

### Medium: Save schema growth
Pet collection data will evolve quickly. Mitigation: versioned saves and migrations from the first pet vertical slice.

## 28. Recommended Implementation Order

1. Epic 0 — cleanup and baseline.
2. Epic 1 — one-dinosaur vertical slice.
3. Epic 2 — toon visual integration on the vertical slice.
4. Epic 3 — production Home/care UI.
5. Epic 4 — optimized sanctuary.
6. Profile and enforce budgets before adding content.
7. Epic 5 — collection/growth.
8. Epic 6 — economy/retention.
9. Epic 7 — activities.
10. Epic 9 — full feedback pass.
11. Epic 8 — continuous profiling, then final hardening.
12. Epic 10 — release candidate validation.
## 29. Definition of Done for Every Epic

An epic is not complete because the feature appears once in Editor. It is complete when:
- Code compiles cleanly in Unity 6.6 batch mode.
- Feature works from a fresh profile and an existing profile.
- Required data survives save/reload where applicable.
- Mobile input and common phone aspect ratios are validated.
- No new recurring GC allocation is introduced without justification.
- No missing references or console errors occur through the tested flow.
- Third-party source assets remain isolated from project-specific edits where practical.
- Relevant EditMode/PlayMode tests are added.
- Profiler impact is measured for performance-sensitive features.
- Debug-only hooks are excluded from production behavior.
- Acceptance criteria in this PRD are met.

## 30. Success Criteria for the First Major Milestone

The first major milestone is a polished vertical slice, not a content-complete game. It succeeds when:
- A player can boot the game and meet a baby dinosaur.
- The pet has persistent needs and an expressive idle state.
- Petting, feeding, washing and one play interaction are enjoyable and understandable.
- Bond progression provides a visible reward loop.
- URP Toon + Dinosaurus + AllSky + Dreamscape form one coherent visual scene.
- Cartoon GUI provides a finished-looking Home HUD and interaction flow.
- DOTween feedback feels responsive without creating lifecycle/performance problems.
- The sanctuary runs within agreed mobile budgets.
- The codebase is clean enough that adding dinosaur #2 mostly means authoring content, not copying systems.

---

**PRD status:** Initial production specification.  
**Next implementation document:** `EPIC_0_PROJECT_CLEANUP.md` should be written from this PRD before code cleanup begins.

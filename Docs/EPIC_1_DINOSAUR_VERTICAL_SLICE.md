# EPIC 1 — Dinosaur Vertical Slice

## Goal
Ship one starter dinosaur in one sanctuary with a complete, persistent care loop: pet, feed, wash and play.

## Implemented in this branch
- `DinosaurDefinition` and `DinosaurCatalog` authoring assets.
- `OwnedDinosaurData` persistent runtime model.
- Hunger, happiness, cleanliness and energy needs with offline UTC progression.
- Bond XP / level progression.
- `ActiveDinosaurController` with idle/react/eat/wash/play states.
- Duplicate action-token guard to prevent double rewards / double item consumption.
- Pet, wash, food and toy care APIs.
- Three food definitions + one ball toy created by the Epic 1 editor setup.
- Starter Triceratops creation for a new profile.
- Save/load through a versioned JSON envelope.
- Interaction camera controller for home/close care framing.
- EditMode coverage for need clamping, offline progression and bond leveling.
- One-click editor setup: **Dinosaur Game > Epic 1 > Setup Vertical Slice**.

## Remaining scene/UI hookup
Epic 1 logic is intentionally independent of the final Epic 3 UI. The sanctuary scene still needs lightweight buttons/gesture input wired to `ActiveDinosaurController.TryPet`, `TryWash` and `TryUseItem`, plus animation parameter mapping for the selected dinosaur prefab.

## Acceptance checklist
- [x] New profile model can create a starter dinosaur.
- [x] Hunger / happiness / cleanliness / energy persist.
- [x] Offline need progression is timestamp-based.
- [x] Care actions are duplicate-token safe.
- [x] Feeding supports three authored foods.
- [x] One toy action is supported.
- [x] Bond XP and level-up event are exposed.
- [x] Camera mode controller exists.
- [ ] Sanctuary input flow can complete pet/feed/wash/play without debug controls.
- [ ] Starter dinosaur animations are mapped and visually validated in Play Mode.
- [ ] Mobile profiler validates no recurring allocation or instantiate/destroy spikes during repeated care actions.

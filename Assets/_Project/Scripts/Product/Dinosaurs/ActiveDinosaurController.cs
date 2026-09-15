using System;
using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    public enum DinosaurRuntimeState
    {
        Idle,
        Wander,
        React,
        Eat,
        Sleep,
        Wash,
        Play
    }

    public sealed class ActiveDinosaurController : MonoBehaviour
    {
        [SerializeField] private DinosaurCatalog catalog;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float petHappinessGain = 10f;
        [SerializeField] private float washCleanlinessGain = 30f;
        [SerializeField] private int petBondXp = 5;
        [SerializeField] private int washBondXp = 8;

        private readonly DinosaurSaveRepository _saveRepository = new DinosaurSaveRepository();
        private GameObject _instance;
        private string _lastActionToken;

        public OwnedDinosaurData Pet { get; private set; }
        public DinosaurDefinition Definition { get; private set; }
        public DinosaurRuntimeState State { get; private set; } = DinosaurRuntimeState.Idle;

        public event Action<OwnedDinosaurData> PetChanged;
        public event Action<int> BondLevelChanged;
        public event Action<DinosaurRuntimeState> StateChanged;

        private void Awake() => LoadOrCreateStarter();
        private void OnApplicationPause(bool paused) { if (paused) Save(); }
        private void OnApplicationQuit() => Save();
        private void OnDisable() => Save();

        public void LoadOrCreateStarter()
        {
            if (catalog == null || catalog.Starter == null)
            {
                Debug.LogError("ActiveDinosaurController requires a catalog with a starter dinosaur.", this);
                enabled = false;
                return;
            }

            if (_saveRepository.TryLoad(out var save) && catalog.TryGet(save.activePet.definitionId, out var loadedDefinition))
            {
                Pet = save.activePet;
                Definition = loadedDefinition;
                PetNeedsService.ApplyOfflineProgress(Pet, Definition, DateTime.UtcNow);
            }
            else
            {
                Definition = catalog.Starter;
                Pet = OwnedDinosaurData.CreateStarter(Definition);
            }

            SpawnActivePet();
            Save();
            PetChanged?.Invoke(Pet);
        }

        public bool TryPet(string actionToken)
        {
            if (!BeginAction(actionToken, DinosaurRuntimeState.React)) return false;
            var beforeLevel = Pet.bondLevel;
            PetNeedsService.Pet(Pet, petHappinessGain, petBondXp);
            CompleteCare(beforeLevel);
            return true;
        }

        public bool TryWash(string actionToken)
        {
            if (!BeginAction(actionToken, DinosaurRuntimeState.Wash)) return false;
            var beforeLevel = Pet.bondLevel;
            PetNeedsService.Wash(Pet, washCleanlinessGain, washBondXp);
            CompleteCare(beforeLevel);
            return true;
        }

        public bool TryUseItem(PetCareItemDefinition item, string actionToken)
        {
            if (item == null) return false;
            var nextState = item.Type == PetCareItemType.Food ? DinosaurRuntimeState.Eat : DinosaurRuntimeState.Play;
            if (!BeginAction(actionToken, nextState)) return false;

            var beforeLevel = Pet.bondLevel;
            if (item.Type == PetCareItemType.Food)
                PetNeedsService.Feed(Pet, item.HungerGain, item.BondXp);
            else
                PetNeedsService.Play(Pet, item.HappinessGain, item.EnergyCost, item.BondXp);

            CompleteCare(beforeLevel);
            return true;
        }

        public void SetState(DinosaurRuntimeState state)
        {
            if (State == state) return;
            State = state;
            StateChanged?.Invoke(state);
        }

        private bool BeginAction(string actionToken, DinosaurRuntimeState state)
        {
            if (Pet == null) return false;
            if (!string.IsNullOrWhiteSpace(actionToken) && string.Equals(_lastActionToken, actionToken, StringComparison.Ordinal)) return false;
            _lastActionToken = actionToken;
            SetState(state);
            return true;
        }

        private void CompleteCare(int beforeLevel)
        {
            Save();
            PetChanged?.Invoke(Pet);
            if (Pet.bondLevel != beforeLevel) BondLevelChanged?.Invoke(Pet.bondLevel);
            SetState(DinosaurRuntimeState.Idle);
        }

        private void SpawnActivePet()
        {
            if (_instance != null) Destroy(_instance);
            if (Definition.Prefab == null) return;
            var parent = spawnPoint != null ? spawnPoint : transform;
            _instance = Instantiate(Definition.Prefab, parent.position, parent.rotation, parent);
            _instance.name = $"ActivePet_{Definition.Id}";
        }

        private void Save()
        {
            if (Pet != null) _saveRepository.Save(Pet);
        }
    }
}

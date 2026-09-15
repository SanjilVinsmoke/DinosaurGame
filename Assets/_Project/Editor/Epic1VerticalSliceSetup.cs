#if UNITY_EDITOR
using DinosaurGame.Product.Dinosaurs;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DinosaurGame.EditorTools
{
    public static class Epic1VerticalSliceSetup
    {
        private const string ConfigRoot = "Assets/_Project/Config/Epic1";
        private const string StarterPrefabPath = "Assets/GrigoriyArx/DinoGK/Prefabs/Dinos/01_Triceratops.prefab";

        [MenuItem("Dinosaur Game/Epic 1/Setup Vertical Slice")]
        public static void Setup()
        {
            EnsureFolder("Assets/_Project/Config", "Epic1");

            var definition = LoadOrCreate<DinosaurDefinition>($"{ConfigRoot}/StarterTriceratops.asset");
            var catalog = LoadOrCreate<DinosaurCatalog>($"{ConfigRoot}/DinosaurCatalog.asset");
            var apple = LoadOrCreate<PetCareItemDefinition>($"{ConfigRoot}/Food_Apple.asset");
            var greens = LoadOrCreate<PetCareItemDefinition>($"{ConfigRoot}/Food_Greens.asset");
            var berries = LoadOrCreate<PetCareItemDefinition>($"{ConfigRoot}/Food_Berries.asset");
            var toy = LoadOrCreate<PetCareItemDefinition>($"{ConfigRoot}/Toy_Ball.asset");

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(StarterPrefabPath);
            ConfigureDefinition(definition, prefab);
            ConfigureCatalog(catalog, definition);
            ConfigureItem(apple, "apple", "Apple", PetCareItemType.Food, 25f, 2f, 0f, 5);
            ConfigureItem(greens, "greens", "Fresh Greens", PetCareItemType.Food, 35f, 3f, 0f, 7);
            ConfigureItem(berries, "berries", "Berries", PetCareItemType.Food, 20f, 8f, 0f, 6);
            ConfigureItem(toy, "ball", "Play Ball", PetCareItemType.Toy, 0f, 25f, 12f, 10);

            var controller = Object.FindFirstObjectByType<ActiveDinosaurController>();
            if (controller == null)
            {
                var root = new GameObject("DinosaurVerticalSlice");
                controller = root.AddComponent<ActiveDinosaurController>();
            }

            var serialized = new SerializedObject(controller);
            serialized.FindProperty("catalog").objectReferenceValue = catalog;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(controller);
            EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Epic 1 vertical slice configured. Add UI buttons/input that call TryPet, TryWash and TryUseItem.", controller);
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void ConfigureDefinition(DinosaurDefinition definition, GameObject prefab)
        {
            var so = new SerializedObject(definition);
            so.FindProperty("id").stringValue = "starter_triceratops";
            so.FindProperty("displayName").stringValue = "Triceratops";
            so.FindProperty("prefab").objectReferenceValue = prefab;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);
        }

        private static void ConfigureCatalog(DinosaurCatalog catalog, DinosaurDefinition starter)
        {
            var so = new SerializedObject(catalog);
            so.FindProperty("starter").objectReferenceValue = starter;
            var definitions = so.FindProperty("definitions");
            definitions.arraySize = 1;
            definitions.GetArrayElementAtIndex(0).objectReferenceValue = starter;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(catalog);
        }

        private static void ConfigureItem(PetCareItemDefinition item, string id, string label, PetCareItemType type, float hunger, float happiness, float energy, int bondXp)
        {
            var so = new SerializedObject(item);
            so.FindProperty("id").stringValue = id;
            so.FindProperty("displayName").stringValue = label;
            so.FindProperty("type").enumValueIndex = (int)type;
            so.FindProperty("hungerGain").floatValue = hunger;
            so.FindProperty("happinessGain").floatValue = happiness;
            so.FindProperty("energyCost").floatValue = energy;
            so.FindProperty("bondXp").intValue = bondXp;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(item);
        }

        private static void EnsureFolder(string parent, string child)
        {
            var path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path)) AssetDatabase.CreateFolder(parent, child);
        }
    }
}
#endif

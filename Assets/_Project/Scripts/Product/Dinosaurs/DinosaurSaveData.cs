using System;

namespace DinosaurGame.Product.Dinosaurs
{
    [Serializable]
    public sealed class DinosaurSaveData
    {
        public int version = 1;
        public OwnedDinosaurData activePet;
    }
}

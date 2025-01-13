using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Fishing
{
    [CreateAssetMenu(menuName = "AnhPD/Fishing")]
    [Serializable]
    public class FishingData : ScriptableObject
    {
        public int levelIndex;
        public FishingLevelData[] levels;
    }

    [Serializable]
    public class FishingLevelData
    {
        public FishData[] fishes;
        public ObstacleData[] obstacles;
        public int numberOfRandomFish, numberOfRandomObstacle;
    }
    [Serializable]
    public class FishData
    {
        public Fish.ColorType type;
        public int number;
    }
    [Serializable]
    public class ObstacleData
    {
        public ObstacleType type;
        public int number;
    }
}

public enum ObstacleType
{
    Garbage = 0,
    Shark = 1,
    PufferFish = 2,
    Mine = 3,
}


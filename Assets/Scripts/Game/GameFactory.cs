using JimRunner.Tile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JimRunner
{
    public static class GameFactory
    {
        private static Dictionary<string, string> _stringResourcesPath;

        private static Dictionary<string, UnityEngine.Object> _loadedPrefab;

        public static readonly int DayGroundsCount;
        public static readonly int EveningGroundsCount;
        public static readonly int NightGroundsCount;
        public static readonly int WinterGroundsCount;

        public static int GetGroundsCount()
        {
            switch (LocationManager.CurrentLocation)
            {
                case LocationType.Day:
                    return DayGroundsCount;
                case LocationType.Evening:
                    return EveningGroundsCount;
                case LocationType.Night:
                    return NightGroundsCount;
                case LocationType.Winter:
                    return WinterGroundsCount;
                default:
                    return 0;
            }
        }

        static GameFactory()
        {
            _stringResourcesPath = new Dictionary<string, string>();
            _loadedPrefab = new Dictionary<string, UnityEngine.Object>();

            DayGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Day/Grounds").Length;
            EveningGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Evening/Grounds").Length;
            NightGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Night/Grounds").Length;
            WinterGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Winter/Grounds").Length;

            string baseDayGroundsPath = "Prefabs/Day/Grounds/Ground{0}";
            for (int i = 0; i < DayGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Day + i.ToString(), string.Format(baseDayGroundsPath, i.ToString()));

            string baseEveningGroundsPath = "Prefabs/Evening/Grounds/Ground{0}";
            for (int i = 0; i < EveningGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Evening + i.ToString(), string.Format(baseEveningGroundsPath, i.ToString()));

            string baseNightGroundsPath = "Prefabs/Night/Grounds/Ground{0}";
            for (int i = 0; i < NightGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Night + i.ToString(), string.Format(baseNightGroundsPath, i.ToString()));

            string baseWinterGroundsPath = "Prefabs/Winter/Grounds/Ground{0}";
            for (int i = 0; i < WinterGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Winter + i.ToString(), string.Format(baseWinterGroundsPath, i.ToString()));

            string baseBackGroundsPath = "Prefabs/{0}/BackGround/{1}";
            for(int i = 0; i < (int)LocationType.Size; i++)
            {
                _stringResourcesPath.Add("MainCloud" + ((LocationType)i).ToString(), string.Format(baseBackGroundsPath, ((LocationType)i).ToString(), "MainCloud"));
                _stringResourcesPath.Add("Cloud" + ((LocationType)i).ToString(), string.Format(baseBackGroundsPath, ((LocationType)i).ToString(), "Cloud"));
                _stringResourcesPath.Add("FirstRock" + ((LocationType)i).ToString(), string.Format(baseBackGroundsPath, ((LocationType)i).ToString(), "FirstRock"));
                _stringResourcesPath.Add("SecondRock" + ((LocationType)i).ToString(), string.Format(baseBackGroundsPath, ((LocationType)i).ToString(), "SecondRock"));
                _stringResourcesPath.Add("Sky" + ((LocationType)i).ToString(), string.Format(baseBackGroundsPath, ((LocationType)i).ToString(), "Sky"));
            }

            string baseTransitionGroundsPath = "Prefabs/TransitionGrounds/TransitionGround{0}";
            for (int i = 0; i < (int)LocationType.Size; i++)
                _stringResourcesPath.Add("TransitionGround" + i.ToString(), string.Format(baseTransitionGroundsPath, i.ToString()));

            string baseObstaclesPath = "Prefabs/{0}/Obstacles/{1}";
            for (int i = 0; i < (int)LocationType.Size; i++)
                for (int j = 0; j < (int)ObstacleType.Size; j++)
                {
                    _stringResourcesPath.Add(((ObstacleType)j).ToString() + ((LocationType)i).ToString(), string.Format(baseObstaclesPath, ((LocationType)i).ToString(), ((ObstacleType)j).ToString()));
                }
        }

        public static void LoadAll()
        {
            foreach (string key in _stringResourcesPath.Keys)
                GetPrefab(key);
        }

        public static GameObject GetPrefab<T>() where T : MonoBehaviour
        {
            return GetPrefab(typeof(T));
        }

        public static GameObject GetPrefab(Type type)
        {
            return GetPrefab(type.ToString()) as GameObject;
        }

        private static UnityEngine.Object GetPrefab(string key)
        {
            if (!_stringResourcesPath.ContainsKey(key))
            {
                Debug.Log(string.Format("UnitFactory does not have string key {0}", key));
                return null;
            }

            if (!_loadedPrefab.ContainsKey(key))
                _loadedPrefab[key] = Resources.Load(_stringResourcesPath[key]);

            return _loadedPrefab[key];
        }

        public static GameObject GetGround(int index)
        {
            string key = "Ground" + LocationManager.CurrentLocation + index.ToString();
            GameObject result = GetPrefab(key) as GameObject;
            return result;
        }

        public static GameObject GetMainCloud()
        {
            string key = "MainCloud" + LocationManager.PrevioustLocation;
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetCloud()
        {
            string key = "Cloud" + LocationManager.PrevioustLocation;
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetFirstRock()
        {
            string key = "FirstRock" + LocationManager.PrevioustLocation;
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetSecondRock()
        {
            string key = "SecondRock" + LocationManager.PrevioustLocation;
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetSky()
        {
            string key = "Sky" + LocationManager.PrevioustLocation;
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetTransitionGround(int index)
        {
            string key = "TransitionGround" + index.ToString();
            return GetPrefab(key) as GameObject;
        }

        public static GameObject GetObstacel(ObstacleType type)
        {
            string key = type.ToString() + LocationManager.CurrentLocation;
            return GetPrefab(key) as GameObject;
        }

        public static void Clear()
        {
            _loadedPrefab.Clear();
        }
    }
}


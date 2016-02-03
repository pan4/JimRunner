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

            //TileGroundController[] allGrounds = Resources.FindObjectsOfTypeAll<TileGroundController>();
            //foreach(TileGroundController g in allGrounds)
            //{
            //    if (g.Type == LocationType.Day)
            //        DayGroundsCount++;
            //    else if (g.Type == LocationType.Evening)
            //        EveningGroundsCount++;
            //    else if (g.Type == LocationType.Night)
            //        NightGroundsCount++;
            //    else if (g.Type == LocationType.Winter)
            //        WinterGroundsCount++;
            //}

            DayGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Day/Grounds").Length;
            EveningGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Evening/Grounds").Length;
            NightGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Night/Grounds").Length;
            WinterGroundsCount = Resources.LoadAll<TileGroundController>("Prefabs/Winter/Grounds").Length;


            string baseDayGroundsPath = "Prefabs/Day/Grounds/Ground{0}";
            for (int i = 0; i < DayGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Day + i.ToString(), string.Format(baseDayGroundsPath, i.ToString()));

            string baseEveningGroundsPath = "Prefabs/Evening/Grounds/Ground{0}";
            for (int i = 0; i < DayGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Evening + i.ToString(), string.Format(baseEveningGroundsPath, i.ToString()));

            string baseNightGroundsPath = "Prefabs/Night/Grounds/Ground{0}";
            for (int i = 0; i < DayGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Night + i.ToString(), string.Format(baseNightGroundsPath, i.ToString()));

            string baseWinterGroundsPath = "Prefabs/Winter/Grounds/Ground{0}";
            for (int i = 0; i < DayGroundsCount; i++)
                _stringResourcesPath.Add("Ground" + LocationType.Winter + i.ToString(), string.Format(baseWinterGroundsPath, i.ToString()));

            string baseDayBackGroundsPath = "Prefabs/Day/BackGround/{0}";
            _stringResourcesPath.Add("MainCloud" + LocationType.Day, string.Format(baseDayBackGroundsPath, "MainCloud"));
            _stringResourcesPath.Add("Cloud" + LocationType.Day, string.Format(baseDayBackGroundsPath, "Cloud"));
            _stringResourcesPath.Add("FirstRock" + LocationType.Day, string.Format(baseDayBackGroundsPath, "FirstRock"));
            _stringResourcesPath.Add("SecondRock" + LocationType.Day, string.Format(baseDayBackGroundsPath, "SecondRock"));
            _stringResourcesPath.Add("Sky" + LocationType.Day, string.Format(baseDayBackGroundsPath, "Sky"));

            string baseEveningBackGroundsPath = "Prefabs/Evening/BackGround/{0}";
            _stringResourcesPath.Add("MainCloud" + LocationType.Evening, string.Format(baseEveningBackGroundsPath, "MainCloud"));
            _stringResourcesPath.Add("Cloud" + LocationType.Evening, string.Format(baseEveningBackGroundsPath, "Cloud"));
            _stringResourcesPath.Add("FirstRock" + LocationType.Evening, string.Format(baseEveningBackGroundsPath, "FirstRock"));
            _stringResourcesPath.Add("SecondRock" + LocationType.Evening, string.Format(baseEveningBackGroundsPath, "SecondRock"));
            _stringResourcesPath.Add("Sky" + LocationType.Evening, string.Format(baseEveningBackGroundsPath, "Sky"));

            string baseNightBackGroundsPath = "Prefabs/Night/BackGround/{0}";
            _stringResourcesPath.Add("MainCloud" + LocationType.Night, string.Format(baseNightBackGroundsPath, "MainCloud"));
            _stringResourcesPath.Add("Cloud" + LocationType.Night, string.Format(baseNightBackGroundsPath, "Cloud"));
            _stringResourcesPath.Add("FirstRock" + LocationType.Night, string.Format(baseNightBackGroundsPath, "FirstRock"));
            _stringResourcesPath.Add("SecondRock" + LocationType.Night, string.Format(baseNightBackGroundsPath, "SecondRock"));
            _stringResourcesPath.Add("Sky" + LocationType.Night, string.Format(baseNightBackGroundsPath, "Sky"));

            string baseWinterBackGroundsPath = "Prefabs/Winter/BackGround/{0}";
            _stringResourcesPath.Add("MainCloud" + LocationType.Winter, string.Format(baseWinterBackGroundsPath, "MainCloud"));
            _stringResourcesPath.Add("Cloud" + LocationType.Winter, string.Format(baseWinterBackGroundsPath, "Cloud"));
            _stringResourcesPath.Add("FirstRock" + LocationType.Winter, string.Format(baseWinterBackGroundsPath, "FirstRock"));
            _stringResourcesPath.Add("SecondRock" + LocationType.Winter, string.Format(baseWinterBackGroundsPath, "SecondRock"));
            _stringResourcesPath.Add("Sky" + LocationType.Winter, string.Format(baseWinterBackGroundsPath, "Sky"));

            string baseTransitionGroundsPath = "Prefabs/TransitionGrounds/TransitionGround{0}";
            for (int i = 0; i < (int)LocationType.Size; i++)
                _stringResourcesPath.Add("TransitionGround" + i.ToString(), string.Format(baseTransitionGroundsPath, i.ToString()));
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
            return GetPrefab(key) as GameObject;
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

        public static void Clear()
        {
            _loadedPrefab.Clear();
        }
    }
}


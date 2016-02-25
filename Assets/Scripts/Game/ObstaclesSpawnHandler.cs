using Core;
using UnityEngine;

namespace JimRunner
{
    public enum ObstacleType
    {
        Pit,
        Stone,
        Size
    }

    public class ObstaclesSpawnHandler : BaseMonoBehaviour
    {
        float _nextObstacle = 10f;

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (Time.time > _nextObstacle)
            {
                int index = Random.Range(0, (int)ObstacleType.Size);
                GameObject obstacle = GameFactory.GetObstacel((ObstacleType)index);
                //GameObject obj = Instantiate(obstacle, spawnLocation.position, Quaternion.identity) as GameObject;
                _nextObstacle += 5f;
            }
        }
    }
}

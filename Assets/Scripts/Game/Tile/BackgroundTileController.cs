
using UnityEngine;

namespace JimRunner.Tile
{
    public class BackgroundTileController : TileController
    {
        public SpriteRenderer SpriteRenderer;

        protected override void OnCreate()
        {
            base.OnCreate();

            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}

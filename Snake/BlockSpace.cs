using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class BlockSpace : SpriteGameObject
    {
        private Point coordinate;
        public BlockSpace(Point coordinate) : base("Blocks@3x1", .5f)
        {
            this.coordinate = coordinate;
        }
    }
}

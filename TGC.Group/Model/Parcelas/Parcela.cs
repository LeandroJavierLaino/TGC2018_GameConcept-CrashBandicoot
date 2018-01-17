using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Parcela
    {
        protected TgcPlane floor;
        protected List<TgcMesh> plants = new List<TgcMesh>();
        protected List<TgcMesh> walls = new List<TgcMesh>();

        public void render()
        {
            floor.render();
            foreach(var plant in plants)
            {
                plant.render();
            }
            foreach(var wall in walls)
            {
                wall.render();
            }
        }

        public void dispose()
        {
            floor.dispose();
            foreach (var plant in plants)
            {
                plant.dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing.World.Base
{
    interface IScene
    {
        void Build();

        void RenderScene();

        string Name { get; }
    }
}

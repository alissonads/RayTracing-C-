using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;

namespace RayTracing.World.Factory.Base
{
    interface ISceneFactory
    {
        SceneBase CreateSimpleSphere(Observer.ISceneObserver obs);

        SceneBase CreateMultipleSphere(Observer.ISceneObserver obs);
        
    }
}

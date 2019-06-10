using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracing.World.Base;
using RayTracing.World.Scenes;

namespace RayTracing.World.Factory
{
    class SceneFactory : Base.ISceneFactory
    {
        public SceneBase CreateSimpleSphere(Observer.ISceneObserver obs)
        {
            SceneBase scene = new SingleSphere();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateMultipleSphere(Observer.ISceneObserver obs)
        {
            SceneBase scene = new MultipleSpheres();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateBallsScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new BallsScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateBallsScene2(Observer.ISceneObserver obs)
        {
            SceneBase scene = new BallsScene2();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateAmbientOcclusion(Observer.ISceneObserver obs)
        {
            SceneBase scene = new AmbientOcclusion();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateAreaLighting(Observer.ISceneObserver obs)
        {
            SceneBase scene = new AreaLightingScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateObjectTest(Observer.ISceneObserver obs)
        {
            SceneBase scene = new ObjectTest();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateEnvironmentLight(Observer.ISceneObserver obs)
        {
            SceneBase scene = new EnvironmentLightScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateObjectsSSFloat(Observer.ISceneObserver obs)
        {
            SceneBase scene = new ObjectsSSFloat();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateTriangleScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new TriangleScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateTorusScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new TorusScene();
            scene.RegisterObserver(obs);
            return scene;
        }
        
        public SceneBase CreateSolidCylinderScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new SolidCylinderScene();
            scene.RegisterObserver(obs);
            return scene;
        }
        
        public SceneBase CreateSolidConeScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new SolidConeScene();
            scene.RegisterObserver(obs);
            return scene;
        }
        
        public SceneBase CreateAnnulusScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new AnnulusScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateBowlScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new BowlScene();
            scene.RegisterObserver(obs);
            return scene;
        }
        public SceneBase CreateInstanceScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new InstanceScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateBeveledCylinderScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new BeveledCylinderScene();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateRandomSpheres(Observer.ISceneObserver obs)
        {
            SceneBase scene = new RandomSpheres();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateGridAndTransformedObject1(Observer.ISceneObserver obs)
        {
            SceneBase scene = new GridAndTransformedObject1();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateGridAndTransformedObject2(Observer.ISceneObserver obs)
        {
            SceneBase scene = new GridAndTransformedObject2();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateTessellateSphere(Observer.ISceneObserver obs)
        {
            SceneBase scene = new TessellateSphere();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateReflectiveObjects(Observer.ISceneObserver obs)
        {
            SceneBase scene = new ReflectiveObjects();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateGlossyReflectorTest1(Observer.ISceneObserver obs)
        {
            SceneBase scene = new GlossyReflectorTest1();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreatePathTracingTest(Observer.ISceneObserver obs)
        {
            SceneBase scene = new PathTracingTest();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreatePerfectTransmitterTest(Observer.ISceneObserver obs)
        {
            SceneBase scene = new PerfectTransmitterTest();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateGlobalTraceTest(Observer.ISceneObserver obs)
        {
            SceneBase scene = new GlobalTraceTest();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateEarthScene(Observer.ISceneObserver obs)
        {
            SceneBase scene = new Earth();
            scene.RegisterObserver(obs);
            return scene;
        }

        public SceneBase CreateGlassWithLiquid(Observer.ISceneObserver obs)
        {
            SceneBase scene = new GlassWithLiquidScene();
            scene.RegisterObserver(obs);
            return scene;
        }
    }
}

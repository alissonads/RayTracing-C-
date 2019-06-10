using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Reflection;
using RayTracing.World.Factory;
using RayTracing.World.Base;
using RayTracing.Util.Math;
using RayTracing.Observer;
using RayTracing.Util;

namespace RayTracing
{
    partial class window : Form, ISceneObserver
    {
        SceneBase scene;
        SceneFactory sf;
        Bitmap image;
        const string PATH = "../../results/";
        
        readonly object lockObj = new object();

        public window()
        {
            Thread.CurrentThread.Name = "Primary - ";
            InitializeComponent();
            sf = new SceneFactory();
            //scene = new SingleSphere(this);
            //scene = sf.CreateMultipleSphere(this);
            //scene = sf.CreateBallsScene2(this);
            //scene = sf.CreateAmbientOcclusion(this);
            //scene = sf.CreateAreaLighting(this);
            //scene = sf.CreateObjectTest(this);
            //scene = sf.CreateEnvironmentLight(this);
            //scene = sf.CreateObjectsSSFloat(this);
            //scene = sf.CreateTriangleScene(this);
            //scene = sf.CreateTorusScene(this);
            //scene = sf.CreateSolidCylinderScene(this);
            //scene = sf.CreateSolidConeScene(this);
            //scene = sf.CreateAnnulusScene(this);
            //scene = sf.CreateBowlScene(this);
            //scene = sf.CreateInstanceScene(this);
            //scene = sf.CreateBeveledCylinderScene(this);
            //scene = sf.CreateRandomSpheres(this);
            //scene = sf.CreateGridAndTransformedObject1(this);
            //scene = sf.CreateGridAndTransformedObject2(this);
            //scene = sf.CreateTessellateSphere(this);
            //scene = sf.CreateReflectiveObjects(this);
            //scene = sf.CreateGlossyReflectorTest1(this);
            //scene = sf.CreatePathTracingTest(this);
            //scene = sf.CreatePerfectTransmitterTest(this);
            //scene = sf.CreateGlobalTraceTest(this);
            //scene = sf.CreateGlassWithLiquid(this);
            scene = sf.CreateEarthScene(this);
        }

        private void ToConfigureCanvas()
        {
            //int bth = draw.Location.Y + draw.Height + 10;
            //this.Size = new Size(Size.Width,
            //                     scene.BFImage.Height + bth);
            double x = Size.Width / 2.0;
            double y = Size.Height / 2.0;
            int bth = draw.Location.Y + draw.Height;
            double iw = canvas.Image.Width / 2.0;
            double ih = canvas.Image.Height / 2.0;
            x -= iw;
            y = (y - ih);
            canvas.Location = new Point((int)x, (int)y);
        }
        
        private void draw_Click(object sender, EventArgs e)
        {
            canvas.BackColor = Color.DarkGray;
            Thread t = new Thread(new ThreadStart(RenderFile));
            t.IsBackground = true;
            t.Start();
            //RenderFile();
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        public void RayTracerStarted(int hres, int vres)
        {
            image = new Bitmap(hres, vres, PixelFormat.Format32bppArgb);
            //SetControlPropertyValue(canvas, "image", image);
            SetControlPropertyValue(draw, "enabled", false);
            SetControlPropertyValue(save, "enabled", false);
        }

        public void DisplayPixel(int x, int y, int rgb)
        {
            lock (lockObj)
            {
                image.SetPixel(x, y, Color.FromArgb(rgb));
            }
            //SetControlPropertyValue(canvas, "image", image.Clone());
        }

        private void RenderFile()
        {
            scene.Show();
            //ToConfigureCanvas();                              
        }

        private void SaveFile()
        {
            if (canvas.Image == null)
            {
                MessageBox.Show("Error: impossible of saving the image (null image)");
                return;
            }

            canvas.Image.Save(PATH + scene.Name + ".png");
            MessageBox.Show("image " + "(" + scene.Name + ".png)" + " saves with success ");
        }

        public void RayTracerFinished(ISceneObservable world)
        {
            SetControlPropertyValue(canvas, "image", image.Clone());
            SetControlPropertyValue(draw, "enabled", true);
            SetControlPropertyValue(save, "enabled", true);
            //world.RemoveObserver(this);
        }

        delegate void SetControlValueCallback(Control control, string name, object value);

        private void SetControlPropertyValue(Control control, string name, object value)
        {
            if (control.InvokeRequired)
            {
                SetControlValueCallback cb = new SetControlValueCallback(SetControlPropertyValue);
                control.Invoke(cb, new object[] { control, name, value });
            }
            else
            {
                Type t = control.GetType();
                PropertyInfo[] props = t.GetProperties();

                foreach (var p in props)
                {
                    if (p.Name.ToUpper() == name.ToUpper())
                        p.SetValue(control, value, null);
                }
            }
        }
    }
}

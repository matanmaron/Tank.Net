using System;
using System.Collections.Generic;
using System.Windows.Forms;
using myOpenGL;
using System.Drawing;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;
        public static Tank tank = new Tank(0, 0, 0);
        public float[] ScrollValue = new float[10];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        public static float worldsize = 75;
        double[] AccumulatedRotationsTraslations = new double[16];
        public bool[] keyDown = new bool[256];
        public bool leftMouseDown = false;
        public int lastMouseX = 970/2;
        public static GLUquadric obj;
        public bool fullscreen = false;
        public bool oldscreen = false;
        public static int score = 0;
        public static int enemyscore = 0;
        Form2 network = new Form2();
        public static bool networkconnected = false;
        public static Tank enemytank = new Tank(0, 0, 0);
        public static double strenemycount = 0;

        public cOGL(Control pb)
        {
            network.Hide();
            p=pb;
            Width = p.Width;
            Height = p.Height; 
            InitializeGL();
            Bullet.bulletsize = 0.06f;
            tank.RandomPosition(worldsize);
            obj = GLU.gluNewQuadric(); //!!!
            fullscreen = true;

        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj); //!!!
            WGL.wglDeleteContext(m_uint_RC);
        }

		uint m_uint_HWND = 0;

        public uint HWND
		{
			get{ return m_uint_HWND; }
		}
		
        uint m_uint_DC   = 0;

        public uint DC
		{
			get{ return m_uint_DC;}
		}
		uint m_uint_RC   = 0;

        public uint RC
		{
			get{ return m_uint_RC; }
		}

        void DrawBullets()
        {
            if (tank.bullets != null)
            {
                for (int i = 0; i < tank.bullets.Count; i++)
                {
                    if (!tank.bullets[i].boom)
                        tank.bullets[i].drawSelf();
                    else
                        tank.bullets[i].DrawBoom();
                }
            }
            if (enemytank.bullets != null)
            {
                for (int i = 0; i < enemytank.bullets.Count; i++)
                {
                    if (!enemytank.bullets[i].boom)
                        enemytank.bullets[i].drawSelf();
                    else
                        enemytank.bullets[i].DrawBoom();
                }
            }
        }

        void DrawGrid(float gridsize, bool sky)
        {
            float h = 0;
            //floor texture
            GL.glEnable(GL.GL_TEXTURE_2D);

            if (sky)
                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]);
            else
                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);

            GL.glBegin(GL.GL_QUADS);
            GL.glColor3f(1f,1f,1f);//white clear
            //left side
            if (sky)
            {
                h = 10;
                GL.glNormal3f(0.0f, -1.0f, 0.0f);
            }
            else
                GL.glNormal3f(0.0f, 1.0f, 0.0f);

            GL.glTexCoord2f(0, 0);
            GL.glVertex3f(0, h, 0);
            GL.glTexCoord2f(0, 1);
            GL.glVertex3f(-gridsize, h, 0);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(-gridsize, h, -gridsize * 2);
            GL.glTexCoord2f(1f, 0);
            GL.glVertex3f(0, h, -gridsize * 2);
            //right side
            if (sky)
                GL.glNormal3f(0.0f, -1.0f, 0.0f);
            else
                GL.glNormal3f(0.0f, 1.0f, 0.0f);
            GL.glTexCoord2f(0, 0);
            GL.glVertex3f(0, h, 0);
            GL.glTexCoord2f(0, 1);
            GL.glVertex3f(gridsize, h, 0);
            GL.glTexCoord2f(1f, 1f);
            GL.glVertex3f(gridsize, h, -gridsize * 2);
            GL.glTexCoord2f(1f, 0);
            GL.glVertex3f(0, h, -gridsize * 2);

            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);

        }
        public void DrawAxes()
        {
            GL.glPushMatrix();    
            GL.glDisable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
            GL.glEnable(GL.GL_LIGHTING);
            GL.glPopMatrix();
        }

        void DrawAll()
        {
            GL.glDisable(GL.GL_LIGHTING);
            DrawGrid(worldsize, false); //floor
            DrawGrid(worldsize, true); //sky
            GL.glEnable(GL.GL_LIGHTING);
        }
        public void Draw()
        {
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            GL.glTranslatef(0.0f, -1.5f, -6.0f);
            GL.glRotatef(10, 1.0f, 0.0f, 0.0f);
            GL.glRotatef(-tank.rotation, 0.0f, 1.0f, 0.0f);
            GL.glRotatef(-tank.turretRotation, 0.0f, 1.0f, 0.0f);
            GL.glTranslatef(-tank.posX, 0.0f, -tank.posZ);

            //DrawAxes();
            DrawAll();

            //LIGHT - before transforms
            //  hence it is in const position
            GL.glPushMatrix();
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
                GL.glTranslatef(0, 11, -35);
                float[] ambient = { 0, 0, 0.3f, 1 };
 
                    ambient[0] = 0.2f;
                
                float[] diffuse = { 1, 1, 1, 1 };
                float[] specular = { 0.5f, 0.5f, 0.5f, 1f };
                float[] pos = { 0, 1f, -0.5f, 0 };
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, ambient);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, diffuse);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, specular);
                GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);    

            GL.glPopMatrix();

            tank.drawSelf();
     
            DrawBullets();
            //REFLECTION e

            if (networkconnected)
            {
                enemytank.drawSelf();
            }

            update();
            WGL.wglSwapBuffers(m_uint_DC);
        }

        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }


            initRenderingGL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderingGL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;

            GL.glShadeModel(GL.GL_SMOOTH);
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);
            GL.glClearDepth(1.0f);


            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);


            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            //nice 3D
            GLU.gluPerspective(45.0, 1.0, 0.4, 100.0);


            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            GenerateTextures();
        }

        public static uint[] Textures = new uint[9];       // texture

        void GenerateTextures()
        {
            GL.glGenTextures(9, Textures);
            string[] imagesName = { "world.bmp", "side.bmp", "top.bmp" ,"back.bmp","ontop.bmp", "front.bmp", "sky.bmp", "fire.bmp", "target.bmp"};
            for (int i = 0; i < 9; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                //The level-of-detail number. Level 0 is the base image level
                //The number of color components in the texture. 
                //Must be 1, 2, 3, or 4, or one of the following 
                //    symbolic constants: 
                //                GL_ALPHA, GL_ALPHA4, 
                //                GL_ALPHA8, GL_ALPHA12, GL_ALPHA16, GL_LUMINANCE, GL_LUMINANCE4, 
                //                GL_LUMINANCE8, GL_LUMINANCE12, GL_LUMINANCE16, GL_LUMINANCE_ALPHA, 
                //                GL_LUMINANCE4_ALPHA4, GL_LUMINANCE6_ALPHA2, GL_LUMINANCE8_ALPHA8, 
                //                GL_LUMINANCE12_ALPHA4, GL_LUMINANCE12_ALPHA12, GL_LUMINANCE16_ALPHA16, 
                //                GL_INTENSITY, GL_INTENSITY4, GL_INTENSITY8, GL_INTENSITY12, 
                //                GL_INTENSITY16, GL_R3_G3_B2, GL_RGB, GL_RGB4, GL_RGB5, GL_RGB8, 
                //                GL_RGB10, GL_RGB12, GL_RGB16, GL_RGBA, GL_RGBA2, GL_RGBA4, GL_RGB5_A1, 
                //                GL_RGBA8, GL_RGB10_A2, GL_RGBA12, or GL_RGBA16.


                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                              //The width of the border. Must be either 0 or 1.
                                                              //The format of the pixel data
                                                              //The data type of the pixel data
                                                              //A pointer to the image data in memory
                                                              0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }

        void checkInput()
        {
            if (keyDown['W'])
            {
                tank.accelerate(true);
            }
            if (keyDown['S'])
            {
                tank.accelerate(false);
            }
            if (keyDown['A'])
            {
                tank.rotate(true);
            }
            if (keyDown['D'])
            {
                tank.rotate(false);
            }
            if (keyDown['F'])
            {
                fullscreen = !fullscreen;
            }
            if (keyDown['P'])
            {
                Cursor.Show();
                network.TopMost = true;
                network.Show();
                keyDown['P'] = false;
            }
        }

        void update()
        {
            checkInput();

            if (networkconnected)
            {
                if (enemytank.bullets != null)
                {
                    for (int i = 0; i < enemytank.bullets.Count; i++)
                    {
                        enemytank.bullets[i].move(true);
                    }
                    for (int i = 0; i < enemytank.bullets.Count; i++)
                    {
                        if (enemytank.bullets[i].isDead())
                        {
                            enemytank.bullets.Remove(enemytank.bullets[i]);
                        }
                    }
                }
                if (enemytank.isDead())
                {
                    enemytank = new Tank(0, 0, 0);
                }
            }

                if (tank.bullets != null)
            {
                for (int i = 0; i < tank.bullets.Count; i++)
                {
                    tank.bullets[i].move(false);
                }
                for (int i = 0; i < tank.bullets.Count; i++)
                {
                    if (tank.bullets[i].isDead())
                    {
                        tank.bullets.Remove(tank.bullets[i]);
                    }
                }
            }
            
            if (tank.isDead())
            {
                tank = new Tank(0, 0, 0);
                tank.RandomPosition(worldsize);
            }
           
            tank.move();
        }
    }
}



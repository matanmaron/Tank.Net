using System;
using System.Collections.Generic;
using System.Text;
using OpenGL;

namespace myOpenGL
{
    public class Bullet
    {
        public float speedX, speedZ;
        public float originalSpeedX;
        public float originalSpeedZ;
        public float originalPositionX;
        public float originalPositionY;
        public float originalPositionZ;
        public float posX;
        public float posY;
        public float posZ;
        float rotation;
        GLUquadric obj;
        public float hitsize = 0.05f;
        public bool boom;
        public float boomcolor;
        public int remainingLife;
        public static float bulletsize;
        public Bullet(float positionX, float positionY, float positionZ, float velX, float velZ, float initialRotation)
        {
            rotation = initialRotation;
            originalSpeedX = (float)(velX - System.Math.Sin(System.Math.PI / 180 * (initialRotation)));
            originalSpeedZ = (float)(velZ - System.Math.Cos(System.Math.PI / 180 * (initialRotation)));
            originalPositionX = positionX;
            originalPositionZ = positionZ;
            speedX = originalSpeedX;
            speedZ = originalSpeedZ;
	        posX = positionX;
	        posY = positionY;
	        posZ = positionZ;
	        remainingLife = 100;
            boom = false;
            boomcolor = 0.5f;
            
            obj = GLU.gluNewQuadric(); 
        }

        ~Bullet()
        {
            GLU.gluDeleteQuadric(obj); 
        }
        public void move(bool enemy)
        {
            if (!boom)
            {
                posX += speedX;
                posZ += speedZ;
                remainingLife -= 1;
                if (this.ColitionDetection(enemy))
                {
                    boom = true;
                    remainingLife = 50;
                }
            }
        }

        public void DrawBoom()
        {
        GL.glPushMatrix();

            Random random = new Random();
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glColor3f(1, 0.8f, 0.8f);
            GL.glBindTexture(GL.GL_TEXTURE_2D, cOGL.Textures[7]);
            GLU.gluQuadricTexture(obj, 1);
            GL.glTranslatef(posX, posY, posZ);

            int zavit = random.Next(1, 180);
            GL.glRotatef(zavit, 1, 0, 0);
            zavit = random.Next(1, 180);
            GL.glRotatef(zavit, 0, 1, 0);
            zavit = random.Next(1, 180);
            GL.glRotatef(zavit, 0, 0, 1);

            GLU.gluSphere(obj, hitsize, 16, 16);
            GL.glDisable(GL.GL_TEXTURE_2D);
        
        GL.glPopMatrix();
        hitsize += 0.01f;
        remainingLife -= 1;
        }

        void flagAsDead()
        {
            remainingLife = 0;
        }

        public bool isDead()
        {
            return remainingLife <= 0;
        }

        public void drawSelf()
        {
	    GL.glPushMatrix();
		    GL.glTranslatef(posX, posY, posZ);

                GL.glColor3f(0.0f, 0.0f, 0.0f);
                GLUT.glutSolidSphere(bulletsize, 8, 8);
            
	    GL.glPopMatrix();
        }

        public bool ColitionDetection(bool enemy)
        {
            float boarder = cOGL.worldsize;
            if (posZ>0) //z boarder start
                return true;
            if (posZ < -boarder*2) //z boarder finish
                return true;
            if (posX < -boarder) //x boarder start
                return true;
            if (posX > boarder) //x boarder finish
                return true;

            if (enemy)
            {
                if (!cOGL.tank.boom)
                {
                    //target
                    if (InRange(posX, OpenGL.cOGL.tank.posX - OpenGL.cOGL.tank.width, OpenGL.cOGL.tank.posX + OpenGL.cOGL.tank.width))
                    {
                        if (InRange(posZ, OpenGL.cOGL.tank.posZ - OpenGL.cOGL.tank.depth, OpenGL.cOGL.tank.posZ + OpenGL.cOGL.tank.depth))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (!cOGL.enemytank.boom)
                {
                    if (InRange(posX, OpenGL.cOGL.enemytank.posX - OpenGL.cOGL.enemytank.width, OpenGL.cOGL.enemytank.posX + OpenGL.cOGL.enemytank.width))
                    {
                        if (InRange(posZ, OpenGL.cOGL.enemytank.posZ - OpenGL.cOGL.enemytank.depth, OpenGL.cOGL.enemytank.posZ + OpenGL.cOGL.enemytank.depth))
                        {
                            OpenGL.cOGL.enemytank.TankDestroy();
                            cOGL.score++;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool InRange(float value,float min,float max)
        {
            return value >= Math.Min(min,max)&&value<=Math.Max(min,max);
        }
    }
}

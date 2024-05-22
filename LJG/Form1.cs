using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XGIS;

namespace LJG
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 存储空间点对象
        /// </summary>
        //List<XPoint> points = new List<XPoint>();

        //List<XFeature> features = new List<XFeature>();

        public XVectorLayer layer = new XVectorLayer("pointLayer", SHAPETYPE.Point);
        public XVectorLayer layerTyphoon = new XVectorLayer("pointLayer1", SHAPETYPE.Point);
        public static bool isCheckMap = false;
        public static bool isCheckTyphoon = false;

        //public static HashSet<string> names = new HashSet<string>();


        XView view;
        Bitmap backwindow;

        Point MouseDownLocation, MouseMovingLocation;
        XExploreActions currentMouseAction = XExploreActions.noaction;


        public Form1()
        {
            InitializeComponent();
            TopLevel = false;
            TopMost = false;
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);

            DoubleBuffered = true;
            view = new XView(
                new XExtent(
                    new XVertex(0, 0),
                    new XVertex(1, 1)),
                ClientRectangle);
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            XExploreActions action = XExploreActions.noaction;
            if (e.Delta > 0)
            {
                action = XExploreActions.zoomin;
            }
            else
            {
                action = XExploreActions.zoomout;
            }
            view.ChangeView(action);
            UpdateMap();
        }



        public void UpdateMap()
        {
            //如果地图窗口被最小化了，就不用绘制了
            if (ClientRectangle.Width * ClientRectangle.Height == 0) return;
            //////更新view，以确保其地图窗口尺寸是正确的
            view.UpdateMapWindow(ClientRectangle);
            //如果背景窗口不为空，则先清除
            if (backwindow != null) backwindow.Dispose();
            //根据最新的地图窗口尺寸建立背景窗口
            backwindow = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            //在背景窗口上绘图
            Graphics g = Graphics.FromImage(backwindow);
            //清空窗口
            g.FillRectangle(new SolidBrush(this.BackColor), ClientRectangle);
            //绘制空间对象
            layer.draw(g, view);

            //回收绘图工具
            g.Dispose();
            //重绘前景窗口
            Invalidate();
        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //if (backwindow != null)
            //    e.Graphics.DrawImage(backwindow, 0, 0);


            if (backwindow == null) return;

            if (currentMouseAction == XExploreActions.pan)
            {
                e.Graphics.DrawImage(backwindow,
                    MouseMovingLocation.X - MouseDownLocation.X,
                    MouseMovingLocation.Y - MouseDownLocation.Y);
            }
            else if (currentMouseAction == XExploreActions.zoominbybox ||
                currentMouseAction == XExploreActions.select)
            {
                e.Graphics.DrawImage(backwindow, 0, 0);
                int x = Math.Min(MouseDownLocation.X, MouseMovingLocation.X);
                int y = Math.Min(MouseDownLocation.Y, MouseMovingLocation.Y);
                int width = Math.Abs(MouseDownLocation.X - MouseMovingLocation.X);
                int height = Math.Abs(MouseDownLocation.Y - MouseMovingLocation.Y);

                if (currentMouseAction == XExploreActions.zoominbybox)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.FromArgb(100, 100, 0, 0)), x, y, width, height);
                }

                e.Graphics.DrawRectangle(
                    new Pen(Color.Red, 1),
                    x, y, width, height);
            }
            else
            {
                e.Graphics.DrawImage(backwindow, 0, 0);
            }



        }



        public void ExploreButton_Click(object sender, EventArgs e)
        {
            var sendname = (sender as Button).Name;
            XExploreActions action = XExploreActions.noaction;
            if (sendname == "bMoveUp") action = XExploreActions.moveup;
            else if (sendname == "bMoveDown") action = XExploreActions.movedown;
            else if (sendname == "bMoveLeft") action = XExploreActions.moveleft;
            else if (sendname == "bMoveRight") action = XExploreActions.moveright;
            else if (sendname == "bZoomIn") action = XExploreActions.zoomin;
            else if (sendname == "bZoomOut") action = XExploreActions.zoomout;

            view.ChangeView(action);

            UpdateMap();

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            UpdateMap();
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownLocation = e.Location;
            if (e.Button == MouseButtons.Right)
                currentMouseAction = XExploreActions.zoominbybox;
            else if (e.Button == MouseButtons.Left)
                currentMouseAction = XExploreActions.pan;
            if (Control.ModifierKeys == Keys.Alt ||
                Control.ModifierKeys == Keys.Control)
                currentMouseAction = XExploreActions.select;
            Cursor = Cursors.WaitCursor;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            XVertex v = view.ToMapVertex(e.Location);
            labelCoordinates.Text = e.Location.X + "," + e.Location.Y + "\r\n" +
                v.x + "," + v.y;

            MouseMovingLocation = e.Location;
            if (currentMouseAction == XExploreActions.zoominbybox ||
                currentMouseAction == XExploreActions.pan ||
                currentMouseAction == XExploreActions.select)
            {
                Invalidate();
            }

        }

        public void bReadShapefile_Click(object sender, EventArgs e)
        {
            layer = XShapefile.ReadShapefile(
"data/China/China.shp");
            layerTyphoon = new XVectorLayer("pointLayer1", SHAPETYPE.Point);
            if (sender is string)
            {
                layerTyphoon = XShapefile.ReadShapefile(
                "data/台风/" + sender.ToString() + ".shp");
                layer.Features.AddRange(layerTyphoon.Features);
            }
            else
            {
                layer = XShapefile.ReadShapefile(
"data/China/China.shp");
            }

            FullExtent();
        }

        public void bFullExtent_Click(object sender, EventArgs e)
        {

            FullExtent();
        }

        private void FullExtent()
        {
            view.Update(layer.Extent, ClientRectangle);
            UpdateMap();
        }



        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            XVertex v1 = view.ToMapVertex(MouseDownLocation);
            if (MouseDownLocation == e.Location)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //点选操作
                    if (currentMouseAction == XExploreActions.select)
                    {
                        layer.SelectByVertex(v1, view.ToMapDistance(5),
                            Control.ModifierKeys == Keys.Control);
                        //List<SelectResult> fs = XSelect.SelectFeaturesByVertex(
                        //    v1, layer.Features, view.ToMapDistance(5));
                    }
                }
                if (e.Button == MouseButtons.Middle)
                {
                    XVertex v = view.ToMapVertex(e.Location);
                    view.UpdateMapCenter(v);
                }
            }
            else
            {

                XVertex v2 = view.ToMapVertex(e.Location);

                if (currentMouseAction == XExploreActions.zoominbybox)
                {
                    XExtent extent = new XExtent(v1, v2);
                    view.Update(extent, ClientRectangle);
                }
                else if (currentMouseAction == XExploreActions.pan)
                {
                    view.OffsetCenter(v1, v2);
                }
                //框选操作
                else if (currentMouseAction == XExploreActions.select)
                {
                    //List<SelectResult> selection = XSelect.SelectFeaturesByExtent(
                    //        new XExtent(v1, v2), 
                    //        layer.Features
                    //    );
                    //MessageBox.Show("选中空间对象数量：" + selection.Count);
                    layer.SelectByExtent(new XExtent(v1, v2),
                        Control.ModifierKeys == Keys.Control);

                }
            }
            UpdateMap();
            currentMouseAction = XExploreActions.noaction;
        }
    }
}
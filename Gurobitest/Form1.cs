/* Author：张星
 * Email: zxcumt1992@163.com
 * 功能描述：
 * 11.23 10:51  生成轨迹点坐标并在窗体上动态展示生成的轨迹点
 * 使用chartcontrol进行轨迹坐标点的绘制
 * 11.24 11:40 已经完成核心工作
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using Gurobi;
namespace Gurobitest
{
    public partial class Form1 : Form
    {
        // 将车轮宽度确定为1m 那么对应的像素宽度是30，而整个轨迹带的宽度是120，车的宽度是90，同一轨迹带中相邻的轨迹相差30  轨迹分割线两侧的轨迹相差90
        #region 设置全局变量
        //public double machineWidth = 90;
        //public int boundX = 10;
        //public int lengthY = 200;
        public int ratio = 1;//表示chartcontrol的坐标下与实际距离的比例 代表一米的距离//
        public int rmin =2; //定义转弯半径 文章中小车是2，而冲击碾压机是6
        public int pointsInCircle =20; // 可以随时更改 定义U型圆弧上点的个数
        public List<double[,]> totalPointsList = new List<double[,]>();
        #region 定义关于获取轨迹两端的点的坐标
        public int zeroTrackX = 10;
        public int zeroTrackY = 150;
        public int trackLineLength = 100;
        #endregion

        public int turnLeftOrRight;
        public int pointNum = 0; // 用来定义timer读取的数据个数
        //分别定义Ω型对应的小圆弧与大圆弧上划分点的个数
        public int minCount = 5;
        public int maxCount = 45;

        public bool traditional = false;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // GRBVar\
            //设置routepanel的大小

            //
            Thread th = new Thread(new ThreadStart(initail));
            th.Start();
            
        }
        public void initail()
        {
            this.BeginInvoke(new Action(() => { initailControls(); }));
        }
        public void initailControls()
        {
            routePanel.Height = this.Height - sequenceBox.Location.Y - sequenceBox.Height - 20;
        }
        #region 优化路径并获得路径优化的结果
        private void button1_Click(object sender, EventArgs e)
        {
            string[] args = null;
            int NUM = Convert.ToInt32(trackNumBox.Text);
            TSPClass.MainMethod(args, NUM);
            int[] tour = TSPClass.finalTour;
            for (int i = 0; i < tour.Length; i++)
            {
                sequenceBox.AppendText(tour[i] + " ");
            }
            lenBox.Text = TSPClass.diss.ToString();
            timeBox.Text = TSPClass.timeStr;
        }
        #endregion

        #region 生成路径轨迹点坐标
        private void returnRoute_Click(object sender, EventArgs e)
        {
            int[] routeSequence = new int[Convert.ToInt32(trackNumBox.Text.Trim(' '))];
            if (traditional==true)
            {
                int calNumOfTrack =Convert.ToInt32( trackNumBox.Text.Trim(' '));//获得优化的轨迹条数
                int[] trackSe = new int[calNumOfTrack];
                int midNum = calNumOfTrack / 2;
                int j = 0;
                for (int i = 0; i < calNumOfTrack; i=i+2)
                {
                    trackSe[i] =j;
                    trackSe[i + 1] = midNum + j ;
                    j++;
                }
                routeSequence = trackSe;
            }
            else
            {
                routeSequence = TSPClass.finalTour;//获得碾压顺序
            }

            
            int routeCount = routeSequence.Length;
            double[,] nodeList = returnTrackNodeList(routeCount);
            int uporDown = 1; //开始的时候是在上面
            for (int i = 0; i <routeCount ; i++)
            {
                if (i<routeCount-1)
                {
                    //得到轨迹编号 并在nodelist中找到点的坐标 
                    int trackNumStart = routeSequence[i];
                    int trackNumEnd = routeSequence[i + 1];
                    if (uporDown == 1)
                    {
                        double x1 = nodeList[trackNumStart, 0];
                        double y1 = nodeList[trackNumStart, 1];

                        double x11 = nodeList[trackNumStart, 2];
                        double y11 = nodeList[trackNumStart, 3];
                        double x2 = nodeList[trackNumEnd, 2];
                        double y2 = nodeList[trackNumEnd, 3];
                        double[,] startNode = new double[1, 2];
                        double[,] startPoint = new double[1, 2];
                        double[,] endPoint = new double[1, 2];

                        if (trackNumStart < trackNumEnd)
                        {
                            turnLeftOrRight = 1;
                        }
                        else
                        {
                            turnLeftOrRight = 0;
                        }
                        startNode[0, 0] = x1;
                        startNode[0, 1] = y1;
                        startPoint[0, 0] = x11;
                        startPoint[0, 1] = y11;
                        endPoint[0, 0] = x2;
                        endPoint[0, 1] = y2;
                        List<double[,]> currentLinePoints = GetLineXY(startNode, uporDown);
                        totalPointsList.AddRange(currentLinePoints);
                        //在这里判断是Ω型还是U型  根据deltax和2rmin*ratio的关系
                        if (Math.Abs(x11 - x2) < 2 * rmin * ratio)
                        {
                            List<double[,]> currentCirclePoints = GetCircleXY2(startPoint, endPoint, uporDown, turnLeftOrRight);
                            totalPointsList.AddRange(currentCirclePoints);
                        }
                        else
                        {
                            List<double[,]> currentCirclePoints = GetCircleXY(startPoint, endPoint, uporDown, turnLeftOrRight);
                            totalPointsList.AddRange(currentCirclePoints);
                        }
                    }
                    else
                    {
                        double x1 = nodeList[trackNumStart, 2];
                        double y1 = nodeList[trackNumStart, 3];
                        double x11 = nodeList[trackNumStart, 0];
                        double y11 = nodeList[trackNumStart, 1];
                        double x2 = nodeList[trackNumEnd, 0];
                        double y2 = nodeList[trackNumEnd, 1];
                        double[,] startNode = new double[1, 2];
                        double[,] startPoint = new double[1, 2];
                        double[,] endPoint = new double[1, 2];
                        if (trackNumStart < trackNumEnd)
                        {
                            turnLeftOrRight = 0;
                        }
                        else
                        {
                            turnLeftOrRight = 1;
                        }
                        startNode[0, 0] = x1;
                        startNode[0, 1] = y1;
                        startPoint[0, 0] = x11;
                        startPoint[0, 1] = y11;
                        endPoint[0, 0] = x2;
                        endPoint[0, 1] = y2;
                        List<double[,]> currentLinePoints = GetLineXY(startNode, uporDown);
                        totalPointsList.AddRange(currentLinePoints);
                        //在这里判断是Ω型还是U型  根据deltax和2rmin*ratio的关系
                        if (Math.Abs(x11-x2)<2*rmin*ratio)
                        {
                            List<double[,]> currentCirclePoints = GetCircleXY2(startPoint, endPoint, uporDown, turnLeftOrRight);
                            totalPointsList.AddRange(currentCirclePoints);
                        }
                        else
                        {
                            List<double[,]> currentCirclePoints = GetCircleXY(startPoint, endPoint, uporDown, turnLeftOrRight);
                            totalPointsList.AddRange(currentCirclePoints);
                        }
                        
                    }
                    if (uporDown == 1)
                    {
                        uporDown = 0;
                    }
                    else
                    {
                        uporDown = 1;
                    }
                }
               
            }

            #region 添加返回初始点的轨迹点
            int trackFinalStart = routeSequence[routeCount-1];
            int trackFinalEnd = routeSequence[0];
            uporDown = 0;
            if (uporDown == 0)
            {
                double x1 = nodeList[trackFinalStart, 2];
                double y1 = nodeList[trackFinalStart, 3];
                double x11 = nodeList[trackFinalStart, 0];
                double y11 = nodeList[trackFinalStart, 1];
                double x2 = nodeList[trackFinalEnd, 0];
                double y2 = nodeList[trackFinalEnd, 1];
                double[,] startNode = new double[1, 2];
                double[,] startPoint = new double[1, 2];
                double[,] endPoint = new double[1, 2];
                if (trackFinalStart < trackFinalEnd)
                {
                    turnLeftOrRight = 0;
                }
                else
                {
                    turnLeftOrRight = 1;
                }
                startNode[0, 0] = x1;
                startNode[0, 1] = y1;
                startPoint[0, 0] = x11;
                startPoint[0, 1] = y11;
                endPoint[0, 0] = x2;
                endPoint[0, 1] = y2;
                List<double[,]> currentLinePoints = GetLineXY(startNode, uporDown);
                totalPointsList.AddRange(currentLinePoints);
                //在这里判断是Ω型还是U型  根据deltax和2rmin*ratio的关系
                if (Math.Abs(x11 - x2) < 2 * rmin * ratio)
                {
                    List<double[,]> currentCirclePoints = GetCircleXY2(startPoint, endPoint, uporDown, turnLeftOrRight);
                    totalPointsList.AddRange(currentCirclePoints);
                }
                else
                {
                    List<double[,]> currentCirclePoints = GetCircleXY(startPoint, endPoint, uporDown, turnLeftOrRight);
                    totalPointsList.AddRange(currentCirclePoints);
                }
            }
            #endregion


            //double[,] startXY = new double[1, 2];
            //startXY[0, 0] = 10;
            //startXY[0, 1] =1000;
            //double[,] endXY = new double[1, 2];
            //endXY[0, 0] = 170;
            //endXY[0, 1] = 1000;
            //totalPointsList = GetCircleXY(startXY,endXY,1,1);
            //List<double[,]> tempList = GetCircleXY(endXY, startXY, 0, 1);
            //totalPointsList.AddRange(tempList);//可以直接添加轨迹集合
            string path = trackNumBox.Text + "条轨迹.txt";
            byte[] bytes = new byte[1024];
            using (FileStream fs =new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write))
            {
                for (int k = 0; k <totalPointsList.Count; k++)
                {
                    string tempstr = totalPointsList[k][0, 0].ToString() + ";" + totalPointsList[k][0, 1].ToString()+"\r\n";
                    bytes = Encoding.UTF8.GetBytes(tempstr);
                    fs.Write(bytes, 0, bytes.Length);
                    
                }
            }
            
            MessageBox.Show("轨迹生成完成");
        }
        #endregion

        #region 动态生成轨迹点
        private void dynamicShow_Click(object sender, EventArgs e)
        {
            #region GDI+
            //Graphics gh = this.routePanel.CreateGraphics();
            //Rectangle r = new Rectangle(10, 600, 8, 8);
            //Brush br = new SolidBrush(Color.Red);
            //gh.FillEllipse(br, r);
            //gh.FillEllipse(new SolidBrush(Color.Green), new Rectangle(34, 600, 7, 7));
            //gh.FillEllipse(new SolidBrush(Color.Green), new Rectangle(50, 600, 7, 7));
            //gh.FillEllipse(new SolidBrush(Color.Green), new Rectangle(50, 120, 7, 7));
            //gh.FillEllipse(new SolidBrush(Color.Green), new Rectangle(146, 24, 7, 7));
            ////gh.FillEllipse(br, new Rectangle(130, 100, 10, 10));
            //int height = routePanel.Height;
            #endregion

            #region chartcontrol
            if (routeChartControl.Series.Count!=0)
            {
                routeChartControl.Series.Clear();
            }
            Series se = new Series("轨迹点", ViewType.Point);
            
            routeChartControl.Series.Add(se);
            showPointTimer.Enabled = true;
            #endregion

        }
        #endregion

        #region 根据两点间的坐标计算圆弧上的坐标 U型
        /// <summary>
        /// 返回两轨迹点连接线坐标集合
        /// </summary>
        /// <param name="startXY">从轨迹驶出时的坐标</param>
        /// <param name="endXY">从轨迹点驶入时的坐标</param>
        /// <param name="uporDown">区分startXY是轨迹相对开始点还是结束点 1 代表结束点 0代表开始点</param>
        /// <param name="leftorRgiht">区分从startXY向左转还是向右转 1 代表右转 0代表左转</param>
        /// <returns></returns>
        public List<double[,]> GetCircleXY(double[,] startXY, double[,] endXY, int uporDown, int leftorRgiht)
        {
            double startX = startXY[0, 0];
            double startY = startXY[0, 1];
            double endX = endXY[0, 0];
            double endY = endXY[0, 1];
            List<double[,]> circleList = new List<double[,]>();
            #region 判断是上是下还是左转右转
            if (uporDown == 1)  //1代表在上面
            {
                if (leftorRgiht == 1) //1 代表右转
                {
                    #region 右转情况下的坐标计算
                    //左边1/4圆弧的轨迹坐标点  
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    double r = rmin * ratio;
                    double centerpointX = startX + r;
                    double centerpointY = startY;
                    for (int i =1; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX - r * Math.Cos(Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX,currentY} });

                    }
                    // 添加水平直线的坐标  定义0.2m一个点
                    if (endX-startX>2*r)
                    {
                        double linkPointx = circleList[circleList.Count - 1][0,0];
                        double linkPointy= circleList[circleList.Count - 1][0, 1];
                        double length = endX - startX-2*r;
                        int countInLine =(int) Math.Floor(length / (0.2 * ratio));
                        for (int i = 1; i <= countInLine; i++)
                        {
                            double currentX = linkPointx + 0.2 * ratio * i;
                            circleList.Add(new double[,] { { currentX, linkPointy } });
                        }
                    }
                    double[,] kkk = circleList[circleList.Count - 1];
                    //右边1/4圆弧的轨迹坐标点
                    centerpointX = endX - r;
                    centerpointY = endY;
                    for (int i = 0; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
                else
                {
                    #region 左转情况下的坐标计算
                    //左边1/4圆弧的轨迹坐标点  
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    double r = rmin * ratio;
                    double centerpointX = startX - r;
                    double centerpointY = startY;
                    for (int i = 1; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX +r * Math.Cos(Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });

                    }
                    // 添加水平直线的坐标  定义0.2m一个点
                    if (startX - endX > 2 * r)
                    {
                        double linkPointx = circleList[circleList.Count - 1][0, 0];
                        double linkPointy = circleList[circleList.Count - 1][0, 1];
                        double length = startX - endX - 2 * r;
                        int countInLine = (int)Math.Floor(length / (0.2 * ratio));
                        for (int i = 1; i <= countInLine; i++)
                        {
                            double currentX = linkPointx - 0.2 * ratio * i;
                            circleList.Add(new double[,] { { currentX, linkPointy } });
                        }
                    }
                    double[,] kkk = circleList[circleList.Count - 1];
                    //右边1/4圆弧的轨迹坐标点
                    centerpointX = endX + r;
                    centerpointY = endY;
                    for (int i = 0; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX - r * Math.Cos(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }

            }
            else
            {
                if (leftorRgiht == 1) //1 代表右转
                {
                    #region 右转情况下的坐标计算
                    //左边1/4圆弧的轨迹坐标点  
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    double r = rmin * ratio;
                    double centerpointX = startX - r;
                    double centerpointY = startY;
                    for (int i = 1; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX +r * Math.Cos(Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY -r * Math.Sin(Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });

                    }
                    // 添加水平直线的坐标  定义0.2m一个点
                    if (startX-endX > 2 * r)
                    {
                        double linkPointx = circleList[circleList.Count - 1][0, 0];
                        double linkPointy = circleList[circleList.Count - 1][0, 1];
                        double length = startX - endX - 2 * r;
                        int countInLine = (int)Math.Floor(length / (0.2 * ratio));
                        for (int i = 1; i <= countInLine; i++)
                        {
                            double currentX = linkPointx - 0.2 * ratio * i;
                            circleList.Add(new double[,] { { currentX, linkPointy } });
                        }
                    }
                    double[,] kkk = circleList[circleList.Count - 1];
                    //右边1/4圆弧的轨迹坐标点
                    centerpointX = endX + r;
                    centerpointY = endY;
                    for (int i = 0; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX - r * Math.Cos(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY - r * Math.Sin(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
                else
                {
                    #region 左转情况下的坐标计算
                    //左边1/4圆弧的轨迹坐标点  
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    double r = rmin * ratio;
                    double centerpointX = startX + r;
                    double centerpointY = startY;
                    for (int i = 1; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX - r * Math.Cos(Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY -r * Math.Sin(Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });

                    }
                    // 添加水平直线的坐标  定义0.2m一个点
                    if (endX - startX > 2 * r)
                    {
                        double linkPointx = circleList[circleList.Count - 1][0, 0];
                        double linkPointy = circleList[circleList.Count - 1][0, 1];
                        double length = endX - startX - 2 * r;
                        int countInLine = (int)Math.Floor(length / (0.2 * ratio));
                        for (int i = 1; i <= countInLine; i++)
                        {
                            double currentX = linkPointx +0.2 * ratio * i;
                            circleList.Add(new double[,] { { currentX, linkPointy } });
                        }
                    }
                    double[,] kkk = circleList[circleList.Count - 1];
                    //右边1/4圆弧的轨迹坐标点
                    centerpointX = endX - r;
                    centerpointY = endY;
                    for (int i = 0; i <= pointsInCircle; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        double currentY = centerpointY - r * Math.Sin(Math.PI / 2 - Math.PI / (2 * pointsInCircle) * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
            }
            #endregion
            
            return circleList;
        }
        #endregion

        #region TUDO根据两点间的坐标计算圆弧上的坐标 Ω型
        public List<double[,]> GetCircleXY2(double[,] startXY, double[,] endXY, int uporDown, int leftorRgiht)
        {
            //确定Ω型的圆弧轨迹坐标
            //确定两点间的距离
            double len = Math.Abs(startXY[0, 0] - endXY[0, 0]);
            double r = rmin * ratio;
            //确定旋转角度
            double circleAngle = Math.Acos((len/2 + r) / (2 * r));
            double deltaY = 2 * r * Math.Sin(circleAngle); // 得到大圆弧中心点纵坐标与转弯点的纵坐标之差
            double startX = startXY[0, 0];
            double startY = startXY[0, 1];
            double endX = endXY[0, 0];
            double endY = endXY[0, 1];
            List<double[,]> circleList = new List<double[,]>();
            #region 判断是上是下还是左转右转
            if (uporDown == 1)  //1代表在上面
            {
                if (leftorRgiht == 1) //1 代表右转
                {
                    #region 右转情况下的坐标计算 
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    // 小圆弧与大圆弧中心点分别在轨迹的两侧
                    //第一个小圆弧中心
                    double centerpointX1 = startX - r;
                    double centerpointY1 = startY;
                    //第二个小圆弧中心
                    double centerpointX2 = endX +r;
                    double centerpointY2 = endY;
                    //大圆弧中心点
                    double centerpointX = startX + len/2;
                    double centerpointY = startY+deltaY;
                    //第一个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX1 + r * Math.Cos(circleAngle/minCount*i);
                        double currentY = centerpointY1 + r * Math.Sin(circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //添加大圆弧轨迹坐标
                    for (int i = 1; i <= maxCount; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(Math.PI+circleAngle- (Math.PI+2*circleAngle) / maxCount * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI + circleAngle - (Math.PI + 2 * circleAngle) / maxCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //第二个小圆弧
                    for (int i = 1; i <=minCount ; i++)
                    {
                        double currentX = centerpointX2 - r * Math.Cos(circleAngle- circleAngle / minCount * i);
                        double currentY = centerpointY2 + r * Math.Sin(circleAngle-circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
                else
                {
                    #region 左转情况下的坐标计算 
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    // 小圆弧与大圆弧中心点分别在轨迹的两侧
                    double centerpointX1 = startX + r;
                    double centerpointY1 = startY;
                    //第二个小圆弧中心
                    double centerpointX2 = endX - r;
                    double centerpointY2 =endY;
                    //大圆弧中心点
                    double centerpointX = startX - len / 2;
                    double centerpointY = startY + deltaY;
                   
                    //第一个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX1 - r * Math.Cos(circleAngle / minCount * i);
                        double currentY = centerpointY1 + r * Math.Sin(circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //添加大圆弧轨迹坐标
                    for (int i = 1; i <= maxCount; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(-circleAngle +(Math.PI + 2 * circleAngle) / maxCount * i);
                        double currentY = centerpointY + r * Math.Sin(-circleAngle + (Math.PI + 2 * circleAngle) / maxCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //第二个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX2 + r * Math.Cos(circleAngle - circleAngle / minCount * i);
                        double currentY = centerpointY2 + r * Math.Sin(circleAngle - circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }

            }
            else
            {
                if (leftorRgiht == 1) //1 代表右转
                {
                    #region 右转情况下的坐标计算 
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    // 小圆弧与大圆弧中心点分别在轨迹的两侧
                    //第一个小圆弧中心
                    double centerpointX1 = startX +r;
                    double centerpointY1 = startY;
                    //第二个小圆弧中心
                    double centerpointX2 = endX - r;
                    double centerpointY2 = endY;
                    //大圆弧中心点
                    double centerpointX = startX - len / 2;
                    double centerpointY = startY - deltaY;
                    //第一个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX1 - r * Math.Cos(circleAngle / minCount * i);
                        double currentY = centerpointY1 - r * Math.Sin(circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //添加大圆弧轨迹坐标
                    for (int i = 1; i <= maxCount; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(circleAngle - (Math.PI + 2 * circleAngle) / maxCount * i);
                        double currentY = centerpointY + r * Math.Sin(circleAngle - (Math.PI + 2 * circleAngle) / maxCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //第二个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX2 +r * Math.Cos(circleAngle - circleAngle / minCount * i);
                        double currentY = centerpointY2 - r * Math.Sin(circleAngle - circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
                else
                {
                    #region 左转情况下的坐标计算 
                    // 考虑长方形场地，且场地的宽度方向和坐标轴平行  后期可以考虑场地倾斜的方向，但是场地仍然是规则的长方形。
                    // 小圆弧与大圆弧中心点分别在轨迹的两侧
                    double centerpointX1 = startX - r;
                    double centerpointY1 = startY;
                    //第二个小圆弧中心
                    double centerpointX2 = endX+ r;
                    double centerpointY2 = endY;
                    //大圆弧中心点
                    double centerpointX = startX + len / 2;
                    double centerpointY = startY - deltaY;

                    //第一个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX1 +r * Math.Cos(circleAngle / minCount * i);
                        double currentY = centerpointY1 - r * Math.Sin(circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //添加大圆弧轨迹坐标
                    for (int i = 1; i <= maxCount; i++)
                    {
                        double currentX = centerpointX + r * Math.Cos(Math.PI-circleAngle + (Math.PI + 2 * circleAngle) / maxCount * i);
                        double currentY = centerpointY + r * Math.Sin(Math.PI -circleAngle + (Math.PI + 2 * circleAngle) / maxCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    //第二个小圆弧
                    for (int i = 1; i <= minCount; i++)
                    {
                        double currentX = centerpointX2 - r * Math.Cos(circleAngle - circleAngle / minCount * i);
                        double currentY = centerpointY2 - r * Math.Sin(circleAngle - circleAngle / minCount * i);
                        circleList.Add(new double[,] { { currentX, currentY } });
                    }
                    #endregion
                }
            }
            #endregion

            return circleList;
        }
        #endregion

        #region 获得轨迹节点两端的集合
        /// <summary>
        /// 获得直线段两端节点坐标
        /// </summary>
        /// <param name="numTrack"></param>
        /// <returns></returns>
        public double[,] returnTrackNodeList(int numTrack)
        {
            //int trackCount = Convert.ToInt32(trackNumBox.Text);
            double[,] NodeList = new double[numTrack, 4];
            for (int i = 1; i <= numTrack; i++)
            {
                if (i%2!=0)
                {
                    int m = i/2;
                    double x = zeroTrackX + m * 4 * ratio;
                    NodeList[i - 1, 0] = x;
                    NodeList[i - 1, 1] = zeroTrackY;
                    NodeList[i - 1, 2] = x;
                    NodeList[i - 1, 3] = zeroTrackY + trackLineLength;
                }
                else
                {
                    int m = i / 2;
                    double x = zeroTrackX + ratio + (m-1) * 4 * ratio;
                    NodeList[i - 1, 0] = x;
                    NodeList[i - 1, 1] = zeroTrackY; //轨迹起始节点坐标
                    NodeList[i - 1, 2] = x;
                    NodeList[i - 1, 3] = zeroTrackY + trackLineLength; //轨迹结束节点坐标
                }
            }
            return NodeList;
        }
        #endregion

        #region 计算直线坐标
        public List<double[,]> GetLineXY(double[,] startXY, int uporDown)
        {
            List<double[,]> lineXYList = new List<double[,]>();
            int count =(int)(trackLineLength / (0.5 * ratio));
            double x = startXY[0, 0];
            double y = startXY[0, 1];
            for (int i = 0; i < count; i++)
            {
                if (uporDown == 1)
                {
                    double newY = y + 0.5 * ratio * i;
                    lineXYList.Add(new double[,] { { x, newY } });
                }
                else
                {
                    double newY = y - 0.5 * ratio * i;
                    lineXYList.Add(new double[,] { { x, newY } });
                }

            }

            return lineXYList;
        }
        #endregion

        #region timer展示轨迹点
        private void showPointTimer_Tick(object sender, EventArgs e)
        {
            if (pointNum < totalPointsList.Count)
            {
                double[,] item = totalPointsList[pointNum];
                routeChartControl.Series[0].Points.Add(new SeriesPoint(item[0, 0], item[0, 1]));
                pointNum++;
            }
            else
            {
                showPointTimer.Enabled = false;
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Series se = new Series("静态轨迹", ViewType.Point);
            foreach (var item in totalPointsList)
            {
                se.Points.Add(new SeriesPoint(item[0, 0], item[0, 1]));
            }
            PointSeriesView sv = (PointSeriesView)se.View;
            sv.PointMarkerOptions.Size = 2;
            routeChartControl.Series.Add(se);
            
            //se.View.
            int xwidth = routeChartControl.Width;
            int yheight = routeChartControl.Height;
            //按照比例获取显示坐标的范围
            XYDiagram xy = (XYDiagram)routeChartControl.Diagram;
            //获取y轴的最大值。
            //int ymax=  (int) (xy.AxisY.VisualRange.MaxValue);
            //int xmax = (int)xwidth / yheight * ymax;
            xy.AxisX.WholeRange.SetMinMaxValues(0, 256 * xwidth / yheight);
            xy.AxisX.VisualRange.MaxValue = 256*xwidth/yheight;
            xy.ZoomingOptions.UseMouseWheel = true;
            xy.ZoomingOptions.ZoomInMouseAction.ModifierKeys = ChartModifierKeys.None;
            xy.EnableAxisXZooming = true;
            xy.EnableAxisXScrolling = true;
            xy.EnableAxisYZooming = true;
            xy.EnableAxisYScrolling = true;

            // xy.AxisX.WholeRange.SetMinMaxValues()

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            traditional = true;
        }
    }
}

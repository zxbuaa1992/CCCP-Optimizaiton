using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gurobi;
using System.Diagnostics;
namespace Gurobitest
{
    class  TSPClass : GRBCallback
    {
        private GRBVar[,] vars;
        private static double rmin = 2;
        private static int trackNum = 32;
        private static double wheelwidth = 0.9;
        private static double gapwidth = 1.2;
        private static double machinewidth;
        private static double trackwidth;
        public static double diss = 0;
        public static int[] finalTour;
        public static string timeStr;
        public TSPClass(GRBVar[,] xvars)
        {
            vars = xvars;
        }

        protected override void Callback()
        {
            try
            {
                if (where == GRB.Callback.MIPSOL)
                {
                    // Found an integer feasible solution - does it visit every node?

                    int n = vars.GetLength(0);
                    double[,] jjj = GetSolution(vars); // 随机给出一个方案
                    int count = 0;
                    for (int i = 0; i < jjj.GetLength(0); i++)
                    {
                        for (int j = 0; j <jjj.GetLength(0); j++)
                        {
                            if (jjj[i,j]==1)
                            {
                                count++;
                            }
                        }
                    }
                    int[] tour = findsubtour(GetSolution(vars)); 

                    if (tour.Length < n)
                    {
                        // Add subtour elimination constraint
                        GRBLinExpr expr = 0;
                        for (int i = 0; i < tour.Length; i++)
                            for (int j = i + 1; j < tour.Length; j++)
                                expr.AddTerm(1.0, vars[tour[i], tour[j]]);
                        AddLazy(expr <= tour.Length - 1);
                    }
                }
            }
            catch (GRBException e)
            {
                Console.WriteLine("Error code: " + e.ErrorCode + ". " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        protected static int[] findsubtour(double[,] sol)
        {
            // sol 是个nxn的二维数列
            int n = sol.GetLength(0);
            bool[] seen = new bool[n];
            int[] tour = new int[n];
            int bestind, bestlen;
            int i, node, len, start;

            for (i = 0; i < n; i++)
                seen[i] = false;

            start = 0;
            bestlen = n + 1;
            bestind = -1;
            node = 0;
            while (start < n)
            {
                for (node = 0; node < n; node++)
                    if (!seen[node])
                        break;
                if (node == n)
                    break;
                for (len = 0; len < n; len++)
                {
                    tour[start + len] = node;
                    seen[node] = true;
                    for (i = 0; i < n; i++)
                    {
                        if (sol[node, i] > 0.5 && !seen[i])
                        {
                            node = i;
                            break;
                        }
                    }
                    if (i == n)
                    {
                        len++;
                        if (len < bestlen)
                        {
                            bestlen = len;
                            bestind = start;
                        }
                        start += len;
                        break;
                    }
                }
            }

            for (i = 0; i < bestlen; i++)
                tour[i] = tour[bestind + i];
            System.Array.Resize(ref tour, bestlen);

            return tour;
        }

        //计算两节点之间的距离
        protected static double distance2(int track1, int track2)
        {
            int m = Math.Abs(track1 - track2) / 2;
            int n = Math.Abs(track1 - track2) % 2;
            double gapdis;
            if (n==0)
            {
                gapdis = m * trackwidth;
            }
            else
            {
                int max = 0;
                if (track1>track2)
                {
                    max = track1;
                }
                else
                {
                    max = track2;
                }
                if (max%2!=0)
                {
                    gapdis = m * trackwidth + (gapwidth + wheelwidth) / 2;
                }
                else
                {
                    gapdis = m * trackwidth + machinewidth;
                }
            }
            if (gapdis>=2*rmin)
            {
                return gapdis + (Math.PI - 2) * rmin;
            }
            else
            {
                return (Math.PI + 4 * Math.Acos((gapdis + 2 * rmin) / (4 * rmin))) * rmin;
            }
        }
        public static void MainMethod(string[] args, int NUM)
        {
            Stopwatch t1 = new Stopwatch();
            t1.Start();
            trackNum = NUM;
            machinewidth = 2 * wheelwidth + gapwidth;
            trackwidth = machinewidth * 3 / 2 - wheelwidth / 2;
            int n =trackNum; //定义轨迹数
            try
            {
                GRBEnv env = new GRBEnv("tsp1.lp");
                GRBModel model = new GRBModel(env);

                // Must set LazyConstraints parameter when using lazy constraints

                model.Parameters.LazyConstraints = 1;
                // Create variables

                GRBVar[,] vars = new GRBVar[n, n];

                //定义变量
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        vars[i, j] = model.AddVar(0.0, 1.0, distance2( i, j), GRB.BINARY, "x" + i + "_" + j);
                        vars[j, i] = vars[i, j];
                    }
                }

                // Degree-2 constraints

                for (int i = 0; i < n; i++)
                {
                    GRBLinExpr expr = 0;
                    for (int j = 0; j < n; j++)
                        expr.AddTerm(1.0, vars[i, j]);
                    model.AddConstr(expr == 2.0, "deg2_" + i);
                }
                for (int i = 0; i < n - 1; i++)
                {
                    GRBLinExpr expr = 0;
                    expr.AddTerm(1, vars[i, i + 1]);
                    model.AddConstr(expr == 0, "TurnCon" + i);
                }
                for (int i = 0; i < n; i++)
                    vars[i, i].UB = 0.0;
                model.Write("tsp11.lp");
                // Forbid edge from node back to itself

                

                model.SetCallback(new TSPClass(vars));
                model.Optimize();

                if (model.SolCount > 0)
                {
                    int[] tour = findsubtour(model.Get(GRB.DoubleAttr.X, vars));
                    t1.Stop();
                    TimeSpan ts = t1.Elapsed;
                    timeStr = ts.TotalMilliseconds.ToString();
                    finalTour = tour;
                    Console.Write("Tour: ");
                    
                    for (int i = 0; i < tour.Length; i++)
                    {
                        if (i < tour.Length - 1)
                        {
                            diss += distance2(tour[i], tour[i + 1]);
                        }
                        else
                        {
                            diss += distance2(tour[i], tour[0]);
                        }
                        Console.Write(tour[i] + " ");
                    }
                    Console.WriteLine("*********");
                    Console.WriteLine(diss);
                    
                    Console.WriteLine();
                }

                // Dispose of model and environment
                model.Dispose();
                env.Dispose();

            }
            catch (GRBException e)
            {
                Console.WriteLine("Error code: " + e.ErrorCode + ". " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

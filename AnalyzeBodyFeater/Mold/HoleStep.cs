using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using CycBasic;

namespace AnalyzeBodyFeater
{
    public abstract class HoleStep : IDisplayObject, IComparable<HoleStep>
    {
        /// <summary>
        /// 起点
        /// </summary>
        public Point3d StartPos { get; set; }
        /// <summary>
        /// 终点
        /// </summary>
        public Point3d EndPos { get; set; }
        /// <summary>
        /// 最大半径
        /// </summary>
        public double MaxDia { get; set; }
        /// <summary>
        /// 最小半径
        /// </summary>
        public double MinDia { get; set; }

        public Face Face { get; set; }
        /// <summary>
        /// 面数据
        /// </summary>
        public CycFaceData FaceData { get; set; }
        /// <summary>
        /// 圆弧边
        /// </summary>
        public List<ArcEdgeData> ArcEdge { get; set; } = new List<ArcEdgeData>();
        /// <summary>
        /// 高度
        /// </summary>
        public double HoleStepHigth { get; set; }
        /// <summary>
        /// 矩阵
        /// </summary>
        public Matrix4 Matr { get; set; }
        /// <summary>
        /// 高亮显示
        /// </summary>
        /// <param name="highlight"></param>
        public void Highlight(bool highlight)
        {
            if (highlight)
                this.Face.Highlight();
            else
                this.Face.Unhighlight();
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(HoleStep other)
        {
            try
            {
                if (this.MaxDia == this.MinDia)
                {
                    Vector3d vec = new Vector3d();
                    Matrix4 mat = new Matrix4();
                    mat.Identity();
                    if (this.MaxDia >= other.MaxDia)
                    {
                        if (UMathUtils.IsEqual(this.StartPos, other.StartPos))
                        {
                            vec = UMathUtils.GetVector(other.EndPos, this.EndPos);
                        }
                        else
                            vec = UMathUtils.GetVector(other.StartPos, this.StartPos);
                    }
                    else
                    {
                        if (UMathUtils.IsEqual(this.StartPos, other.StartPos))
                        {
                            vec = UMathUtils.GetVector(this.EndPos, other.EndPos);
                        }
                        else
                            vec = UMathUtils.GetVector(this.StartPos, other.StartPos);
                    }
                    mat.TransformToZAxis(this.StartPos, vec);
                    Point3d center1 = UMathUtils.GetMiddle(this.StartPos, this.EndPos);
                    Point3d center2 = UMathUtils.GetMiddle(other.StartPos, other.EndPos);
                    mat.ApplyPos(ref center1);
                    mat.ApplyPos(ref center2);
                    if (Math.Round(center1.Z, 4) >= Math.Round(center2.Z, 4))
                        return -1;
                    else
                        return 1;
                }
                else
                {
                    Point3d center1 = UMathUtils.GetMiddle(this.StartPos, this.EndPos);
                    Point3d center2 = UMathUtils.GetMiddle(other.StartPos, other.EndPos);
                    this.Matr.ApplyPos(ref center1);
                    this.Matr.ApplyPos(ref center2);
                    if (Math.Round(center1.Z, 4) >= Math.Round(center2.Z, 4))
                        return -1;
                    else
                        return 1;
                }

            }
            catch (Exception ex)
            {
                LogMgr.WriteLog("CycHoleStrp.CompareTo:" + ex.Message);
                return 1;
            }

        }

        public override string ToString()
        {
            return this.MaxDia.ToString() + "," + this.MinDia.ToString() + "," + this.HoleStepHigth.ToString();
        }
        /// <summary>
        /// 判断是否是同一个孔
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public bool IsTheSameHole(HoleStep hs)
        {
            double angle = UMathUtils.Angle(this.FaceData.Dir, hs.FaceData.Dir);
            if (UMathUtils.IsEqual(angle, 0) == false && UMathUtils.IsEqual(angle, Math.PI) == false)
            {
                return false;
            }

            Vector3d vec1 = UMathUtils.GetVector(this.FaceData.Point, hs.FaceData.Point);
            Vector3d vec2 = UMathUtils.GetVector(hs.FaceData.Point, this.FaceData.Point);
            angle = UMathUtils.Angle(this.FaceData.Dir, vec1);
            if (UMathUtils.IsEqual(angle, 0) == false && UMathUtils.IsEqual(angle, Math.PI) == false)
            {
                return false;
            }
            if (CycTraceARay.AskTraceARay(this.Face.GetBody(), this.FaceData.Point, vec1) || CycTraceARay.AskTraceARay(hs.Face.GetBody(), hs.FaceData.Point, vec2))

            {
                return false;
            }
            return true;
        }

        public Edge AskTopEdgeOfHoel()
        {
            if (this.ArcEdge.Count == 1)
            {
                return this.ArcEdge[0].Edge;
            }
            else
            {
                Point3d pt1 = this.ArcEdge[0].Center;
                Point3d pt2 = this.ArcEdge[1].Center;
                this.Matr.ApplyPos(ref pt1);
                this.Matr.ApplyPos(ref pt2);
                if (Math.Round(pt1.Z, 4) >= Math.Round(pt2.Z, 4))
                    return this.ArcEdge[0].Edge;
                else
                    return this.ArcEdge[1].Edge;
            }
        }

        public abstract void ComputeHoleStepAttr();

    }
}

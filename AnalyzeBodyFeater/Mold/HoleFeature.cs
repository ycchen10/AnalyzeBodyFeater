using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using CycBasic;

namespace AnalyzeBodyFeater
{
    public abstract  class HoleFeature : IEquatable<HoleFeature>, IDisplayObject
    {
        /// <summary>
        /// 阶梯
        /// </summary>
        public List<HoleStep> StepList = new List<HoleStep>();   
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 中心点
        /// </summary>
        public Point3d Origin { get; set; }
        /// <summary>
        ///轴向
        /// </summary>
        public Vector3d Direction { get; set; }
        /// <summary>
        /// 顶边
        /// </summary>
        public Edge TopEdge { get; set; } = null;
        /// <summary>
        /// 孔高度
        /// </summary>
        public double HoleHigth { get; set; }

        public bool Equals(HoleFeature other)
        {
            if (this.Name == other.Name)
            {
                double angle = UMathUtils.Angle(this.Direction, other.Direction);
                if (UMathUtils.IsEqual(angle, 0))
                {
                    Matrix4 mat = new Matrix4();
                    mat.Identity();
                    mat.TransformToZAxis(this.Origin, this.Direction);
                    Point3d thisPt = this.Origin;
                    Point3d otherPt = other.Origin;
                    mat.ApplyPos(ref thisPt);
                    mat.ApplyPos(ref otherPt);
                    if (UMathUtils.IsEqual(thisPt.Z, otherPt.Z))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public void Highlight(bool highlight)
        {
            foreach (HoleStep hs in this.StepList)
            {

                hs.Highlight(highlight);

            }
        }
        /// <summary>
        /// 写入属性
        /// </summary>
        public abstract void ComputeHoleFeatureAttr();
     
    }
}

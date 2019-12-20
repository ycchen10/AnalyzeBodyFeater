using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using CycBasic;

namespace AnalyzeBodyFeater
{
    public class CylinderHole : HoleStep
    {
        /// <summary>
        /// 圆柱孔
        /// </summary>
        /// <param name="face"></param>
        public CylinderHole(Face face)
        {
            this.Face = face;
            this.FaceData = CycFaceUtils.AskFaceData(face);

        }
        public override void ComputeHoleStepAttr()
        {
            string errorMsg = "";
            try
            {             
                Matrix4 mat = new Matrix4();
                mat.Identity();
                mat.TransformToZAxis(this.FaceData.Point, this.FaceData.Dir);
                this.Matr = mat;
                foreach (Edge eg in this.Face.GetEdges())
                {
                    if (eg.SolidEdgeType == Edge.EdgeType.Circular)
                    {
                        this.ArcEdge.Add(CycEdgeUtils.GetArcData(eg, ref errorMsg));
                    }
                }
                if (this.ArcEdge.Count == 2)
                {
                    Vector3d vec = UMathUtils.GetVector(this.ArcEdge[0].Center, this.ArcEdge[1].Center);
                    double ange = UMathUtils.Angle(vec, this.FaceData.Dir);
                    this.MaxDia = this.ArcEdge[0].Radius;
                    this.MinDia = this.ArcEdge[0].Radius;
                    if (UMathUtils.IsEqual(ange, 0))
                    {
                        this.StartPos = this.ArcEdge[0].Center;
                        this.EndPos = this.ArcEdge[1].Center;
                    }
                    else
                    {
                        this.StartPos = this.ArcEdge[1].Center;
                        this.EndPos = this.ArcEdge[2].Center;
                    }

                    this.HoleStepHigth = UMathUtils.GetDis(this.StartPos, this.EndPos);
                }
                else
                    errorMsg = this.Face.Tag.ToString() + "边错误！";
                
            }
            catch (Exception ex)
            {
                LogMgr.WriteLog("Circle.ComputeHoleStepAttr." + this.Face.Tag.ToString() + errorMsg + ex.Message);
            }

        }
    }
}

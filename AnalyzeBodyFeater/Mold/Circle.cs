using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using CycBasic;

namespace AnalyzeBodyFeater
{
    /// <summary>
    /// 圆环
    /// </summary>
    public class Circle : HoleStep
    {

        public Circle_Type_t Type { get; set; }

        public Circle(Face face)
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
                this.StartPos = this.FaceData.Point;
                this.EndPos = this.FaceData.Point;
                this.HoleStepHigth = 0;
                foreach (Edge eg in this.Face.GetEdges())
                {
                    if (eg.SolidEdgeType == Edge.EdgeType.Circular)
                    {
                        this.ArcEdge.Add(CycEdgeUtils.GetArcData(eg, ref errorMsg));
                    }
                }
                if (this.ArcEdge.Count == 1 || this.ArcEdge.Count == 2)
                {
                    if (this.ArcEdge.Count == 1)
                    {
                        this.Type = Circle_Type_t.Circle_Whole_Circle_Type_t;
                        this.MaxDia = this.ArcEdge[0].Radius;
                        this.MinDia = 0;

                    }
                    else
                    {
                        this.Type = Circle_Type_t.Circle_Circle_Type_t;
                        if (this.ArcEdge[0].Radius >= this.ArcEdge[1].Radius)
                        {
                            this.MaxDia = this.ArcEdge[0].Radius;
                            this.MinDia = this.ArcEdge[1].Radius;
                        }
                        else
                        {
                            this.MaxDia = this.ArcEdge[1].Radius;
                            this.MinDia = this.ArcEdge[0].Radius;
                        }

                    }


                }
                else
                {
                    errorMsg = this.Face.Tag.ToString() + "边错误！";
                }
            }
            catch(Exception ex)
            {
                LogMgr.WriteLog("Circle.ComputeHoleStepAttr." + this.Face.Tag.ToString() + errorMsg+ex.Message);
            }
        }
    }

    public enum Circle_Type_t
    {
        Circle_Circle_Type_t = 0,        //圆环
        Circle_Whole_Circle_Type_t = 1   //圆
    }
}

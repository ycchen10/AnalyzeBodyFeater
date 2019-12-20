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
    /// 孔圆锥面
    /// </summary>
    public class CircularCone : HoleStep
    {
        /// <summary>
        /// 圆锥类型
        /// </summary>
        public CilcularCone_Type_t Type { get; set; }

        public CircularCone(Face face)
        {
            this.Face = face;
            this.FaceData = CycBasic.CycFaceUtils.AskFaceData(face);
        }
        public override void ComputeHoleStepAttr()
        {
            string err = "";
            try
            {               
                Matrix4 mat = new Matrix4();
                mat.Identity();
                foreach (Edge eg in this.Face.GetEdges())
                {
                    if (eg.SolidEdgeType == Edge.EdgeType.Circular)
                    {
                        this.ArcEdge.Add(CycEdgeUtils.GetArcData(eg, ref err));
                    }
                }
                if (this.ArcEdge.Count == 1 || this.ArcEdge.Count == 2)
                {
                    if (this.ArcEdge.Count == 1)
                    {
                        this.Type = CilcularCone_Type_t.CilcularCone_Cone_type_t;
                        this.StartPos = this.ArcEdge[0].Center;
                        this.EndPos = UMathUtils.GetSymmetry(this.StartPos, this.FaceData.Point);
                        this.MaxDia = Math.Round(this.ArcEdge[0].Radius, 4);
                        this.MinDia = 0;
                       
                    }
                    else
                    {
                        this.Type = CilcularCone_Type_t.CilcularCone_Tur_type_t;
                        if (this.ArcEdge[0].Radius >= this.ArcEdge[1].Radius)
                        {
                            this.StartPos = this.ArcEdge[0].Center;
                            this.MaxDia = this.ArcEdge[0].Radius;
                            this.EndPos = this.ArcEdge[1].Center;
                            this.MinDia = this.ArcEdge[1].Radius;
                        }
                        else
                        {
                            this.StartPos = this.ArcEdge[1].Center;
                            this.MaxDia = this.ArcEdge[1].Radius;
                            this.EndPos = this.ArcEdge[0].Center;
                            this.MinDia = this.ArcEdge[0].Radius;
                        }
                      
                    }
                    Vector3d vec = UMathUtils.GetVector(this.StartPos, this.EndPos);
                    mat.TransformToZAxis(this.StartPos, vec);
                    this.Matr = mat;
                    this.HoleStepHigth = Math.Round(UMathUtils.GetDis(this.EndPos, this.StartPos), 4);
                }
                else
                    err = err + this.Face.Tag.ToString() + "错误";
            }
            catch (Exception ex)
            {
                LogMgr.WriteLog("CircularCone.ComputeHoleStepAttr" + err + this.Face.Tag.ToString() + "错误"+ex.Message);
            }
        }


    }

    public enum CilcularCone_Type_t
    {
        CilcularCone_Tur_type_t = 0,       //两条边圆锥
        CilcularCone_Cone_type_t = 1,      //一条边圆锥
    }
}

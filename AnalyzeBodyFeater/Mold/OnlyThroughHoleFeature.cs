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
    /// 单一通孔
    /// </summary>
    public class OnlyThroughHoleFeature : HoleFeature
    {
        public OnlyThroughHoleFeature(HoleCollection hc)
        {
            this.StepList = hc.StepList;
        }
        public override void ComputeHoleFeatureAttr()
        {
            string err = "";
            try
            {
                if (this.StepList.Count > 0)
                {
                    this.StepList.Sort();
                    this.Origin = this.StepList[0].StartPos;
                    if (this.StepList.Count == 1)
                    {
                        this.HoleHigth = this.StepList[0].HoleStepHigth;
                        this.Direction = this.StepList[0].FaceData.Dir;
                        this.Name = this.StepList[0].ToString();
                        this.TopEdge = this.StepList[0].AskTopEdgeOfHoel();
                    }
                    else
                    {
                        Matrix4 mat = new Matrix4();
                        mat.Identity();
                        this.TopEdge = this.StepList[0].AskTopEdgeOfHoel();
                        this.Direction = UMathUtils.GetVector(this.StepList[1].EndPos, this.StepList[0].StartPos);
                        mat.TransformToZAxis(this.Origin, this.Direction);
                        foreach (HoleStep hs in StepList)
                        {
                            this.Name += hs.ToString();
                            this.HoleHigth += hs.HoleStepHigth;
                            hs.Matr = mat;
                        }
                    }

                }
                else
                    err = "StepList.........错误！";
            }
            catch(Exception ex)
            {
                LogMgr.WriteLog("OnlyThroughHoleFeature.ComputeHoleFeatureAttr.........错误！" + err + ex.Message);
            }

        }




    }
}

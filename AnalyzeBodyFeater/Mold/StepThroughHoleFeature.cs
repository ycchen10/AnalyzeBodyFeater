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
    /// 台阶通孔
    /// </summary>
    public class StepThroughHoleFeature : HoleFeature
    {
        /// <summary>
        /// 圆环面
        /// </summary>
        public List<Circle> CircleList = new List<Circle>();

        public StepThroughHoleFeature(HoleCollection hc)
        {
            this.StepList = hc.StepList;
            this.CircleList = hc.CircleList;
        }
        public override void ComputeHoleFeatureAttr()
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
                if(hs is Circle)
                {
                    CircleList.Add(hs as Circle);
                }
            }
        }
    }
}

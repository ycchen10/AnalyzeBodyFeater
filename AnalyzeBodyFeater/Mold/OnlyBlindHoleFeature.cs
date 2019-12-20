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
    /// 单一盲孔
    /// </summary>
    public class OnlyBlindHoleFeature : HoleFeature
    {
        public OnlyBlindHoleFeature(HoleCollection hc)
        {
            this.StepList = hc.StepList;
        }
        public override void ComputeHoleFeatureAttr()
        {
            throw new NotImplementedException();
        }
    }
}

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
    /// 台阶盲孔
    /// </summary>
    public class StepBlindHoleFeature : HoleFeature
    {
        public List<Circle> CircleList { get; set; } = new List<Circle>();

        public StepBlindHoleFeature(HoleCollection hc)
        {

        }
        public override void ComputeHoleFeatureAttr()
        {
            throw new NotImplementedException();
        }
    }
}

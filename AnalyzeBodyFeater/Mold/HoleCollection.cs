using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
namespace AnalyzeBodyFeater
{
    public class HoleCollection
    {
        private List<HoleStep> m_stepList = new List<HoleStep>();

        public List<HoleStep> StepList
        {
            get { return m_stepList; }
        }
        public HoleCollection(HoleStep hs)
        {
            this.m_stepList.Add(hs);
        }
  
    }
}

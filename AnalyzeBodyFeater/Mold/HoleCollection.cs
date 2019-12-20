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

        private List<Circle> m_circleList = new List<Circle>();
        public List<HoleStep> StepList
        {
            get
            {
                m_stepList.Sort();
                return m_stepList;
            }
        }

        public List<Circle> CircleList
        {
            get
            {
                return m_circleList;
            }      
        }

        public HoleCollection(HoleStep hs)
        {
            this.m_stepList.Add(hs);
            if(hs is Circle)
            {
                CircleList.Add(hs as Circle);
            }
        }
        public bool FindHole(HoleStep hs)
        {
            if (m_stepList[0].IsTheSameHole(hs))
            {
                m_stepList.Add(hs);
                if (hs is Circle)
                {
                    CircleList.Add(hs as Circle);
                }
                return true;
            }
            else
                return false;
        }

        public HoleFeature GetHoleFeature()
        {
            if
        }
    }
}

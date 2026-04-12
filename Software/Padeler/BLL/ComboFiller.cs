using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ComboFiller // Kristian Katulić
    {
        public List<string> GetFrequenciesOfPlay()
        {
            return new List<string>
            {
                "Daily",
                "Weekly",
                "Monthly"
            };
        }
        public List<string> GetLevelsOfPlay()
        {
            return new List<string>
            {
                "Beginner",
                "Lower intermediate",
                "Intermediate",
                "Advanced",
                "Professional"
            };
        }
        public List<string> GetPositions()
        {
            return new List<string>
            {
                "Left",
                "Right"
            };
        }
        public List<string> GetGenders() {
            return new List<string>
            {
                "Male",
                "Female",
                "Other"
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiegandLibWP8
{
    public partial class SimpleCard
    {
        public int BadgeNumber { get; protected set; }
        public int FacilityCode { get; protected set; }

        protected SimpleCard()
        { }  // of SimpleCard()

        public SimpleCard(int p_BadgeNumber, int p_FacilityCode)
            : this()
        {
            // TODO: check validity of Badge Number

            // TODO: check validity of Facility Code

            BadgeNumber = p_BadgeNumber;
            FacilityCode = p_FacilityCode;

        }  // of SimpleCard()

    }  // of class SimpleCard

}  // of namespace WiegandLibWP8

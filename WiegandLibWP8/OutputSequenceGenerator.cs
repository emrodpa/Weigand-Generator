using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiegandLibWP8
{
    public partial class OutputSequenceFactory
    {
        public OutputSequenceFactory()
        {

        }  // of OutputSequenceGenerator()

        public OutputSequence GetOutputSequenceObject(SimpleCard p_Card, ProtocolTypes p_ProtocolType)
        {
            OutputSequence l_OutputSequence = null;

            switch (p_ProtocolType)
            {
                case ProtocolTypes.Weigand26:
                    l_OutputSequence = new OutputWiegand26(p_Card);
                    break;

                case ProtocolTypes.Weigand35:
                    l_OutputSequence = new OutputWiegand35(p_Card);
                    break;

                default:
                    throw new Exception("it shouldn't get here");

            }  // of switch

            return l_OutputSequence;

        }  // of OutputSequenceGenerator()

    }  // of class OutputSequenceFactory()

}  // of namespace WiegandLibWP8()

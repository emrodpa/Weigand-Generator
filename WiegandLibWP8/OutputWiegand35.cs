using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiegandLibWP8
{
    public partial class OutputWiegand35 : OutputSequence
    {
        public OutputWiegand35(SimpleCard p_Card)
            : base(p_Card, ProtocolTypes.Weigand35)
        {
            
        }  // of OutputWiegand35()

        public override void ComputeSequence(ProtocolTypes p_ProtocolType)
        {
            int l_BadgeLengthInBinary = m_ProtocolLength.GetProtocolBadgeLengthInBinary(p_ProtocolType);
            int l_FacilityCodeLengthInBinary = m_ProtocolLength.GetProtocolFacilityCodeLengthInBinary(p_ProtocolType); ;
            int l_FullSequenceLength = m_ProtocolLength.GetProtocolFullSequenceLength(p_ProtocolType);

            char[] l_BadgeInBinary = new char[l_BadgeLengthInBinary];

            for (int i = 0; i < l_BadgeInBinary.Length; i++)
                l_BadgeInBinary[i] = '0';

            char[] l_FacilityInBinary = new char[l_FacilityCodeLengthInBinary];

            for (int i = 0; i < l_FacilityInBinary.Length; i++)
                l_FacilityInBinary[i] = '0';

            char[] l_CombinedBinary = new char[l_BadgeLengthInBinary + l_FacilityCodeLengthInBinary + 3];

            for (int i = 0; i < l_CombinedBinary.Length; i++)
                l_CombinedBinary[i] = '0';

            char[] l_FullOutputSequence = new char[l_FullSequenceLength];

            for (int i = 0; i < l_FullOutputSequence.Length; i++)
                l_FullOutputSequence[i] = '0';

            l_BadgeInBinary = GetBinarySequenceFromInt(Card.BadgeNumber, l_BadgeLengthInBinary);

            l_FacilityInBinary = GetBinarySequenceFromInt(Card.FacilityCode, l_FacilityCodeLengthInBinary);

            l_FacilityInBinary.CopyTo(l_CombinedBinary, 2);
            l_BadgeInBinary.CopyTo(l_CombinedBinary, l_FacilityInBinary.Length + 2);

            // calculate second bit Even Parity
            int l_ParityCounter = 0;
            int l_StepCounter = -1;
            for (int i = 2; i < l_CombinedBinary.Length - 1; i++)
            {
                l_StepCounter++;
                if (l_StepCounter != 2)
                {
                    if (l_CombinedBinary[i] == '1')
                        l_ParityCounter++;
                }
                else
                {
                    l_StepCounter = -1;
                }  // of if/else

            }  // of foreach

            l_CombinedBinary[1] = (l_ParityCounter % 2 == 0) ? '0' : '1';

            // calculate last bit as ODD parity
            l_ParityCounter = 0;
            l_StepCounter = -1;
            for (int i = 1; i < l_CombinedBinary.Length - 2; i++)
            {
                l_StepCounter++;
                if (l_StepCounter != 2)
                {
                    if (l_CombinedBinary[i] == '1')
                        l_ParityCounter++;
                }
                else
                {
                    l_StepCounter = -1;
                }  // of if/else

            }  // of foreach

            l_CombinedBinary[l_CombinedBinary.Length - 1] = (l_ParityCounter % 2 != 0) ? '0' : '1';

            // calculate first bit as ODD parity
            l_ParityCounter = 0;
            for (int i = 1; i < l_CombinedBinary.Length; i++)
            {
                if (l_CombinedBinary[i] == '1')
                    l_ParityCounter++;

            }  // of foreach

            l_CombinedBinary[0] = (l_ParityCounter % 2 != 0) ? '0' : '1';

            l_CombinedBinary.CopyTo(l_FullOutputSequence, 0);

            BinaryOutput = GetStringFromCharArray(l_FullOutputSequence);

            BinaryOutputExactLength = GetStringFromCharArray(l_CombinedBinary);

            List<byte> l_ListBytes = GetBytesFromFullSequence(l_FullOutputSequence);

            StringBuilder l_sbHexSequence = new StringBuilder();

            foreach (byte b in l_ListBytes)
            {
                l_sbHexSequence.Append(b.ToString("X2"));
            }  // of 

            HexOutput = l_sbHexSequence.ToString();

        }  // of ComputeSequence()

    }  // of class OutputWiegand35

}  // of namespace WiegandLibWP8

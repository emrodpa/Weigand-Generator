using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiegandLibWP8
{
    public partial class OutputWiegand26 : OutputSequence
    {
        public OutputWiegand26(SimpleCard p_Card)
            : base(p_Card, ProtocolTypes.Weigand26)
        {
            
        }  // of OutputWiegand26()

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

            char[] l_CombinedBinary = new char[l_BadgeLengthInBinary + l_FacilityCodeLengthInBinary + 2];

            for (int i = 0; i < l_CombinedBinary.Length; i++)
                l_CombinedBinary[i] = '0';

            char[] l_FullOutputSequence = new char[l_FullSequenceLength];

            for (int i = 0; i < l_FullOutputSequence.Length; i++)
                l_FullOutputSequence[i] = '0';

            l_BadgeInBinary = GetBinarySequenceFromInt(Card.BadgeNumber, l_BadgeLengthInBinary);

            l_FacilityInBinary = GetBinarySequenceFromInt(Card.FacilityCode, l_FacilityCodeLengthInBinary);

            l_FacilityInBinary.CopyTo(l_CombinedBinary, 1);
            l_BadgeInBinary.CopyTo(l_CombinedBinary, l_FacilityInBinary.Length + 1);

            // calculate first bit Even Parity
            int l_ParityCounter = 0;
            for (int i = 1; i < 13; i++)
            {
                if (l_CombinedBinary[i] == '1')
                    l_ParityCounter++;

            }  // of foreach

            l_CombinedBinary[0] = (l_ParityCounter % 2 == 0) ? '0' : '1';

            // calculate last bit as ODD parity
            l_ParityCounter = 0;
            for (int i = 13; i < 25; i++)
            {
                if (l_CombinedBinary[i] == '1')
                    l_ParityCounter++;

            }  // of foreach

            l_CombinedBinary[l_CombinedBinary.Length - 1] = (l_ParityCounter % 2 != 0) ? '0' : '1';

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

    }
}  // of namespace WiegandLibWP8

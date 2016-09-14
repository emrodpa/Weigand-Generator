using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiegandLibWP8
{
    public partial class OutputSequence
    {
        protected List<bool> m_BinaryList;
        protected ProtocolLength m_ProtocolLength;

        public string HexOutput { get; protected set; }

        public string BinaryOutput { get; protected set; }

        /// <summary>
        /// this is the binary to lentgh in the protocol specification
        /// as it comes out of the card reader (not trailing zeroes)
        /// </summary>
        public string BinaryOutputExactLength { get; protected set; }

        public SimpleCard Card { get; protected set; }
        public ProtocolTypes ProtocolType { get; protected set; }

        protected OutputSequence()
        {
            m_BinaryList = new List<bool>();

            m_ProtocolLength = new ProtocolLength();

        }  // of OutputSequence()

        public OutputSequence(SimpleCard p_Card, ProtocolTypes p_ProtocolType)
            : this()
        {
            Card = p_Card;

            m_BinaryList = new List<bool>(m_ProtocolLength.GetProtocolFullSequenceLength(p_ProtocolType));

            ComputeSequence(p_ProtocolType);

        }  // of OutputSequence()

        /// <summary>
        /// should be implemented in derived classes
        /// </summary>
        public virtual void ComputeSequence(ProtocolTypes p_ProtocolType)
        {
            throw new Exception("not implemented method");
            /*
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

            List<byte> l_ListBytes = GetBytesFromFullSequence(l_FullOutputSequence);

            StringBuilder l_sbHexSequence = new StringBuilder();

            foreach (byte b in l_ListBytes)
            {
                l_sbHexSequence.Append(b.ToString("X2"));
            }  // of 

            HexOutput = l_sbHexSequence.ToString();
            */
        }  // of ComputeSequence()

        #region General Methods

        public List<byte> GetBytesFromFullSequence(char[] p_FullSequence)
        {
            // separate the char[] into x bitArrays of 8 bits each
            int l_NumberOfBytes = p_FullSequence.Length / 8;

            List<byte> l_ListBytes = new List<byte>(l_NumberOfBytes);

            List<bool> l_BoolArray;
            int l_Integer = 0;
            int l_nCounter = 0;
            for (int i = 0; i < l_NumberOfBytes; i++)
            {
                l_BoolArray = new List<bool>(8);
                for (int j = 0; j < 8; j++)
                    l_BoolArray.Insert(0, (p_FullSequence[i * 8 + j] == '1') ? true : false);

                int[] l_AuxIntArray = new int[1];

                l_Integer = 0;
                l_nCounter = 0;
                foreach (bool bit in l_BoolArray)
                {
                    if (bit)
                        l_Integer += (Int32)Math.Pow(2.0, (double)l_nCounter);
                    l_nCounter++;

                }  // of foreach

                l_AuxIntArray[0] = l_Integer;

                l_ListBytes.Add(BitConverter.GetBytes(l_AuxIntArray[0])[0]);

            }  // of for

            return l_ListBytes;

        }  // of GetBytesFromFullSequence()

        public char[] GetBinarySequenceFromInt(int p_Int, int p_NumberofBits)
        {
            char[] l_BitList = new char[p_NumberofBits];

            int pos = p_NumberofBits - 1;
            int i = 0;

            while (i < p_NumberofBits)
            {
                if ((p_Int & (1 << i)) != 0)
                {
                    l_BitList[pos] = '1';
                }
                else
                {
                    l_BitList[pos] = '0';
                }
                pos--;
                i++;

            }  // of while

            /*
            StringBuilder l_sbResult=new StringBuilder();

            foreach (char c in l_BitList)
                l_sbResult.Append(c);

            return l_sbResult.ToString();
            */

            return l_BitList;

        }  // of GetBinarySequenceFromInt()

        public string GetStringFromCharArray(char[] p_Array)
        {
            StringBuilder l_sb = new StringBuilder();

            foreach (char c in p_Array)
                l_sb.Append(c);

            return l_sb.ToString();

        }  // of GetStringFromCharArray()

        #endregion

    }  // of class OutputSequence

}  // of namespace WiegandLibWP8

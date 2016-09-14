using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeigandGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBoxProtocol.SelectedIndex = 0;

        }  // of Form1()

        private void btnComputeSequence_Click(object sender, EventArgs e)
        {
            btnComputeSequence.Enabled = false;

            int l_BadgeLengthInBinary        = 0;
            int l_FacilityCodeLengthInBinary = 0;
            int l_FullSequenceLength         = 0;

            switch((string)comboBoxProtocol.SelectedItem)
            {
                case "Weigand 35 Bits":
                    l_BadgeLengthInBinary        = 20;
                    l_FacilityCodeLengthInBinary = 12;
                    l_FullSequenceLength         = 40;
                    break;

                default:
                    throw new Exception("It shouldn't get here");
                            
            }  // of switch

            char[] l_BadgeInBinary    = new char[l_BadgeLengthInBinary];

            for (int i = 0; i < l_BadgeInBinary.Length; i++)
                l_BadgeInBinary[i] = '0';

            char[] l_FacilityInBinary = new char[l_FacilityCodeLengthInBinary];

            for (int i = 0; i < l_FacilityInBinary.Length; i++)
                l_FacilityInBinary[i] = '0';

            char[] l_CombinedBinary   = new char[l_BadgeLengthInBinary+l_FacilityCodeLengthInBinary+3];

            for (int i = 0; i < l_CombinedBinary.Length; i++)
                l_CombinedBinary[i] = '0';

            char[] l_FullOutputSequence = new char[l_FullSequenceLength];

            for (int i = 0; i < l_FullOutputSequence.Length; i++)
                l_FullOutputSequence[i] = '0';

            l_BadgeInBinary = GetBinarySequenceFromInt(Int32.Parse(tbxBadge.Text),l_BadgeLengthInBinary);

            Console.WriteLine("The badge in binary is: {0}", GetStringFromCharArray(l_BadgeInBinary));

            l_FacilityInBinary = GetBinarySequenceFromInt(Int32.Parse(tbxFacilityCode.Text), l_FacilityCodeLengthInBinary);

            Console.WriteLine("The l_FacilityInBinary in binary is: {0}", GetStringFromCharArray(l_FacilityInBinary));

            l_FacilityInBinary.CopyTo(l_CombinedBinary,2);
            l_BadgeInBinary.CopyTo(l_CombinedBinary,l_FacilityInBinary.Length + 2);

            Console.WriteLine("The l_CombinedBinary in binary is: {0}", GetStringFromCharArray(l_CombinedBinary));

            // calculate second bit Even Parity
            int l_ParityCounter = 0;
            int l_StepCounter = -1;
            for(int i=2; i<l_CombinedBinary.Length-1; i++)
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

            l_CombinedBinary[1] = (l_ParityCounter % 2 == 0)?'0':'1';

            // calculate last bit as ODD parity
            l_ParityCounter = 0;
            l_StepCounter = -1;
            for (int i = 1; i < l_CombinedBinary.Length-2; i++)
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

            tbxBinaryOutput.Text = GetStringFromCharArray(l_FullOutputSequence);

            List<byte> l_ListBytes = GetBytesFromFullSequence(l_FullOutputSequence);

            StringBuilder l_sbHexSequence = new StringBuilder();

            foreach (byte b in l_ListBytes)
            {
                l_sbHexSequence.Append(b.ToString("X2"));
            }  // of 

            this.tbxHexOutput.Text = l_sbHexSequence.ToString();

        }  // of btnComputeSequence_Click()


        public string GetStringFromCharArray(char[] p_Array)
        {
            StringBuilder l_sb = new StringBuilder();

            foreach (char c in p_Array)
                l_sb.Append(c);

            return l_sb.ToString();
        
        }  // of GetStringFromCharArray()

        public char[] GetBinarySequenceFromInt(int p_Int, int p_NumberofBits)
        {
            char[] l_BitList = new char[p_NumberofBits];

            int pos = p_NumberofBits-1;
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
        

        public List<byte> GetBytesFromFullSequence(char[] p_FullSequence)
        {
            // separate the char[] into x bitArrays of 8 bits each
            int l_NumberOfBytes = p_FullSequence.Length/8;

            List<byte> l_ListBytes = new List<byte>(l_NumberOfBytes);

            List<bool> l_BoolArray;
            int l_Integer = 0;
            int l_nCounter = 0;
            for (int i = 0; i < l_NumberOfBytes; i++)
            {
                l_BoolArray = new List<bool>(8);
                for (int j = 0; j < 8; j++)
                    l_BoolArray.Insert(0, (p_FullSequence[i*8 + j] == '1') ? true : false );

                int[] l_AuxIntArray = new int[1];

                l_Integer = 0;
                l_nCounter = 0;
                foreach (bool bit in l_BoolArray)
                {
                    if (bit)
                        l_Integer += (Int32)Math.Pow(2.0,(double)l_nCounter);
                    l_nCounter++;

                }  // of foreach

                l_AuxIntArray[0] = l_Integer;

                l_ListBytes.Add(BitConverter.GetBytes(l_AuxIntArray[0])[0]);
                
            }  // of for

            return l_ListBytes;

        }  // of GetBytesFromFullSequence()
        
        
         /*
        public List<byte> GetBytesFromFullSequence(char[] p_FullSequence)
        {
            // separate the char[] into x bitArrays of 8 bits each
            int l_NumberOfBytes = p_FullSequence.Length/8;

            List<byte> l_ListBytes = new List<byte>(l_NumberOfBytes);

            BitArray l_BitArray;
            List<bool> l_BoolArray;
            for (int i = 0; i < l_NumberOfBytes; i++)
            {
                l_BoolArray = new List<bool>(8);
                for (int j = 0; j < 8; j++)
                    l_BoolArray.Insert(0, (p_FullSequence[i*8 + j] == '1') ? true : false );

                l_BitArray = new BitArray(l_BoolArray.ToArray());

                int[] l_AuxIntArray = new int[1];

                l_BitArray.CopyTo(l_AuxIntArray,0);

                //l_ListBytes.Insert(0,BitConverter.GetBytes(l_AuxIntArray[0])[0]);

                l_ListBytes.Add(BitConverter.GetBytes(l_AuxIntArray[0])[0]);
                
            }  // of for


            return l_ListBytes;

        }  // of GetBytesFromFullSequence()
         */
         
    }  // of class Form1


}  // of namespace WeigandGenerator

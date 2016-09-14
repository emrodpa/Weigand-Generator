using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeigandLib
{
    /// <summary>
    /// remember to update class' constructor below each time
    /// a new protocol type is added to this enum
    /// </summary>
    public enum ProtocolTypes { Unknown, Weigand26, Weigand35 }

    /// <summary>
    /// holder for a dictionary of each protocol and length
    /// </summary>
    public partial class ProtocolLength
    {
        protected Dictionary<ProtocolTypes, int> m_DicProtocolFullSequenceLength;
        protected Dictionary<ProtocolTypes, int> m_DicProtocolBadgeLengthInBinary;
        protected Dictionary<ProtocolTypes, int> m_DicProtocolFacilityCodeLengthInBinary;

        public ProtocolLength()
        {
            m_DicProtocolFullSequenceLength = new Dictionary<ProtocolTypes, int>();
            m_DicProtocolBadgeLengthInBinary = new Dictionary<ProtocolTypes, int>();
            m_DicProtocolFacilityCodeLengthInBinary = new Dictionary<ProtocolTypes, int>();

            // load the data
            m_DicProtocolFullSequenceLength.Add(ProtocolTypes.Unknown, 0);
            m_DicProtocolFullSequenceLength.Add(ProtocolTypes.Weigand26, 32);
            m_DicProtocolFullSequenceLength.Add(ProtocolTypes.Weigand35, 40);

            m_DicProtocolBadgeLengthInBinary.Add(ProtocolTypes.Unknown, 0);
            m_DicProtocolBadgeLengthInBinary.Add(ProtocolTypes.Weigand26, 16);
            m_DicProtocolBadgeLengthInBinary.Add(ProtocolTypes.Weigand35, 20);


            m_DicProtocolFacilityCodeLengthInBinary.Add(ProtocolTypes.Unknown, 0);
            m_DicProtocolFacilityCodeLengthInBinary.Add(ProtocolTypes.Weigand26, 8);
            m_DicProtocolFacilityCodeLengthInBinary.Add(ProtocolTypes.Weigand35, 12);

            // sanity check
            // TODO: verify FullSequenceLength =  BadgeLengthInBinary + FacilityCodeLengthInBinary for each protocol type

        }  // of ProtocolLength()

        public int GetProtocolFullSequenceLength(ProtocolTypes p_ProtocolType)
        {
            return m_DicProtocolFullSequenceLength[p_ProtocolType];

        }  // of GetProtocolFullSequenceLength()

        public int GetProtocolBadgeLengthInBinary(ProtocolTypes p_ProtocolType)
        {
            return m_DicProtocolBadgeLengthInBinary[p_ProtocolType];

        }  // of GetProtocolBadgeLengthInBinary()

        public int GetProtocolFacilityCodeLengthInBinary(ProtocolTypes p_ProtocolType)
        {
            return m_DicProtocolFacilityCodeLengthInBinary[p_ProtocolType];

        }  // of GetProtocolFacilityCodeLengthInBinary()
        
    }  // of class ProtocolLength

}  // of namespace WeigandLib

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using WiegandLibWP8;

namespace wp8WiegandCalculator
{
    public partial class PageHelp : PhoneApplicationPage
    {
        private ProtocolTypes m_Protocol;
        private SelectedTexblocks m_SelectedTexblock;
        private OutputMethods m_OutputMethod;

        public PageHelp()
        {
            InitializeComponent();

            m_Protocol = ProtocolTypes.Unknown;
            m_SelectedTexblock = SelectedTexblocks.None;
            m_OutputMethod = OutputMethods.None;

        }  // of PageHelp()

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Uri l_ImageUri = new Uri("/Help/wiegand26.png", UriKind.Relative);
            switch (m_Protocol)
            {
                case ProtocolTypes.Unknown:
                    break;

                case ProtocolTypes.Weigand26:
                    l_ImageUri = new Uri("/Help/wiegand26.png", UriKind.Relative);
                    tbkProtocolName.Text = "Wiegand 26 Bits";
                    break;

                case ProtocolTypes.Weigand35:
                    l_ImageUri = new Uri("/Help/hid35.png", UriKind.Relative);
                    tbkProtocolName.Text = "Wiegand 35 Bits Corporate 1000";
                    break;
            
                default:
                    throw new Exception("it shouldn't get here");

            }  // of switch over Protocol Type

            BitmapImage bmp = new BitmapImage(l_ImageUri);
            imgProtocolExplanation.Source = bmp;

            // output method
            switch (m_OutputMethod)
            {
                case OutputMethods.None:
                    break;

                case OutputMethods.Binary:
                    tbkOutputMethodDescription.Text = string.Format("the binary output shows the \'1\' and \'0\' as it comes out of the card reader. Data0 lows are presented as zeroes and Data1 lows are presented as ones.");
                    break;

                case OutputMethods.Hexadecimal:
                    tbkOutputMethodDescription.Text = string.Format("the hexadecimal output shows the bytes in hexadecimal after adding a certain number of trailing zeros to the binary output. example: for the Weigand 35 bits format, five trailing zeroes are needed in order to form five bytes.");
                    break;

                default:
                    throw new Exception("it shouldn't get here");

            }  // of switch over output method

            // Selected Textblock
            switch (m_SelectedTexblock)
            {
                case SelectedTexblocks.None:
                    break;

                case SelectedTexblocks.FacilityCode:
                    tbkSelectedTexblockDescription.Text = string.Format("RFID cards used for access control will typically be programmed to transmit 26 to 40 bits. These bits can be broken down into 3 categories: facility code, card code, and parity bits. The \"facility code\" allows different buildings, regions or companies to have the same card codes but still have a different card overall.");
                    break;

                case SelectedTexblocks.BadgeID:
                    tbkSelectedTexblockDescription.Text = string.Format("RFID cards used for access control will typically be programmed to transmit 26 to 40 bits. These bits can be broken down into 3 categories: facility code, card code, and parity bits.The \"Badge ID\" (a.k.a. \"card code\") is the code assigned to your particular card. It should be unique within the sytem, and is usually printed on the card itself.");
                    break;

                default:
                    throw new Exception("it shouldn't get here");

            }  // of switch over SelectedTexblocks

        }  // of PhoneApplicationPage_Loaded()

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get a dictionary of query string keys and values.
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;

            // get Protocol
            if (queryStrings.ContainsKey("Protocol"))
            {
                m_Protocol = (ProtocolTypes)Enum.Parse(typeof(ProtocolTypes), queryStrings["Protocol"]);
            }

            // get Selected Textblock
            if (queryStrings.ContainsKey("SelectedTexblock"))
            {
                m_SelectedTexblock = (SelectedTexblocks)Enum.Parse(typeof(SelectedTexblocks), queryStrings["SelectedTexblock"]);
            }

            // get Selected Textblock
            if (queryStrings.ContainsKey("OutputMethod"))
            {
                m_OutputMethod = (OutputMethods)Enum.Parse(typeof(OutputMethods), queryStrings["OutputMethod"]);
            }

        }  // of OnNavigatedTo()

    }  // of class PageHelp

}  // of namespace wp8WiegandCalculator
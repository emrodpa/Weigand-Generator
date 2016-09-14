using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI;
using Windows.UI.Text;
using WeigandLib;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace winappWiegandCalculator
{
    public enum SelectedTextboxes { None, FacilityCode, BadgeID, Output }
    public enum OutputMethods { None, Binary, Hexadecimal }

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class pHome : winappWiegandCalculator.Common.LayoutAwarePage
    {
        private ProtocolTypes m_Protocol;
        private SelectedTextboxes m_SelectedTextbox;
        private OutputMethods m_OutputMethod;

        private OutputSequenceFactory m_OutputSequenceFactory;
        private OutputSequence m_OutputSequence;

        private bool m_PageHasBeenLoaded;

        //This is a variable for the help popup.
        private Popup m_NotificationPopup = new Popup();

        private List<bool> m_ListData;

        public pHome()
        {
            this.InitializeComponent();

            m_PageHasBeenLoaded = false;

        } // of pHome()

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            bool l_bRecalculate = false;

            m_OutputSequenceFactory = new OutputSequenceFactory();

            if (pageState != null)
            {
                if (pageState.ContainsKey("SimpleCard"))
                {
                    SimpleCard l_Card = (SimpleCard)pageState["SimpleCard"];
                    tbxFacilityCode.Text = l_Card.FacilityCode.ToString();
                    tbxBadgeNumber.Text = l_Card.BadgeNumber.ToString();
                }  // of if

                if (pageState.ContainsKey("OutputMethod"))
                    OutputMethodChanged((OutputMethods)pageState["OutputMethod"]);
                else
                    OutputMethodChanged(OutputMethods.None);

                if (m_OutputMethod == OutputMethods.Binary)
                    rbtnBinary.IsChecked = true;
                else
                    rbtnHexadecimal.IsChecked = true;

                if (pageState.ContainsKey("SelectedTextbox"))
                    SelectedTextBoxChanged((SelectedTextboxes)pageState["SelectedTextbox"]);
                else
                    SelectedTextBoxChanged(SelectedTextboxes.None);

                if (pageState.ContainsKey("Protocol"))
                    ProtocolChanged((ProtocolTypes)pageState["Protocol"]);
                else
                    ProtocolChanged(ProtocolTypes.Unknown);

                if (m_Protocol == ProtocolTypes.Weigand35)
                    rbtnWiegand35x.IsChecked = true;
                else
                    rbtnWiegand26x.IsChecked = true;

                l_bRecalculate = (pageState.ContainsKey("Protocol")
                    && pageState.ContainsKey("SelectedTextbox")
                    && pageState.ContainsKey("OutputMethod")
                    && pageState.ContainsKey("SimpleCard")
                 );
            
            }  // of if

            if (l_bRecalculate)
                CalculateAndDisplayOutput();

        }  // of LoadState()

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (pageState.ContainsKey("Protocol"))
                pageState["Protocol"] = m_Protocol;
            else
                pageState.Add("Protocol", m_Protocol);

            if (pageState.ContainsKey("SelectedTextbox"))
                pageState["SelectedTextbox"] = m_SelectedTextbox;
            else
                pageState.Add("SelectedTextbox", m_SelectedTextbox);

            if (pageState.ContainsKey("OutputMethod"))
                pageState["OutputMethod"] = m_OutputMethod;
            else
                pageState.Add("OutputMethod", m_OutputMethod);

            if (m_OutputSequence != null)
            {
                if (pageState.ContainsKey("SimpleCard"))
                    pageState["SimpleCard"] = m_OutputSequence.Card;
                else
                    pageState.Add("SimpleCard", m_OutputSequence.Card);            
            }  // of if

        }  // of SaveState()

        #region NumericPad methods

        private void WriteNumberToOutput(string p_strNumber)
        {
            ClearSequenceOutputObject();

            TextBox l_Selected = GetSelectedTextBoxObject();

            if (l_Selected == null)
                return;

            l_Selected.Text = string.Format("{0}{1}", l_Selected.Text, p_strNumber);

        }  // of WriteNumberToOutput()

        private void tbkNumber7_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("7");
        }

        private void tbkNumber8_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("8");

        }

        private void tbkNumber9_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("9");

        }

        private void tbkNumber4_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("4");

        }

        private void tbkNumber5_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("5");

        }

        private void tbkNumber6_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("6");

        }

        private void tbkNumber3_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("3");

        }

        private void tbkNumber2_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("2");

        }

        private void tbkNumber1_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("1");

        }

        private void tbkNumber0_Click_1(object sender, RoutedEventArgs e)
        {
            WriteNumberToOutput("0");
        }

        private void tbkR_Click_1(object sender, RoutedEventArgs e)
        {
            ClearSequenceOutputObject();

            TextBox l_Selected = GetSelectedTextBoxObject();

            if (l_Selected == null)
                return;

            if (l_Selected.Text.Length > 0)
            {
                l_Selected.Text = l_Selected.Text.Substring(0, l_Selected.Text.Length - 1);
            }

        }  // of tbkR_Click_1()

        private void tbkDel_Click_1(object sender, RoutedEventArgs e)
        {
            ClearSequenceOutputObject();

            TextBox l_Selected = GetSelectedTextBoxObject();

            if (l_Selected == null)
                return;

            l_Selected.Text = "";

        }  // of tbkDel_Click_1()

        #endregion NumericPad methods

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            CalculateAndDisplayOutput();

        }  // of btnResult_Click()

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            m_PageHasBeenLoaded = true;

            /*
            // to force the page (landscape) orientation to the popup
            if (!LayoutRoot.Children.Contains(m_NotificationPopup))
                LayoutRoot.Children.Add(m_NotificationPopup);
            */
        }  // of pageRoot_Loaded()

        private void SelectedTextBoxChanged(SelectedTextboxes p_SelectedTextbox)
        {
            m_SelectedTextbox = p_SelectedTextbox;

            switch (m_SelectedTextbox)
            {
                case SelectedTextboxes.None:
                    break;

                case SelectedTextboxes.FacilityCode:
                    break;

                case SelectedTextboxes.BadgeID:
                    break;

                default:
                    throw new Exception("It shouldn't get here");

            }  // of switch

        }  // of SelectedTextBoxChanged()

        public TextBox GetSelectedTextBoxObject()
        {
            if (m_SelectedTextbox == SelectedTextboxes.None || m_SelectedTextbox == SelectedTextboxes.Output)
                return null;

            TextBox l_Selected = (m_SelectedTextbox == SelectedTextboxes.FacilityCode) ? tbxFacilityCode : tbxBadgeNumber;

            return l_Selected;

        }  // of GetSelectedTextBoxObject()

        private void ClearSequenceOutputObject()
        {
            tbxOutputSequence.Text = "";
            ucCanvasControl.ClearDrawing();

        }  // of ClearSequenceOutputObject()

        private void CalculateAndDisplayOutput()
        {
            int l_FacilityCode;
            int l_BadgeID;
            SimpleCard l_Card;

            HideNotification();

            if (!Int32.TryParse(tbxFacilityCode.Text, out l_FacilityCode))
            {
                ShowNotification("\n Facility code should be a number.\n");
                return;
            }

            if (!Int32.TryParse(tbxBadgeNumber.Text, out l_BadgeID))
            {
                ShowNotification("\n Card Number should be a number. \n");
                return;
            }

            try
            {
                l_Card = new SimpleCard(l_BadgeID, l_FacilityCode);

                m_OutputSequence = m_OutputSequenceFactory.GetOutputSequenceObject(l_Card, m_Protocol);

                tbxOutputSequence.Text = (m_OutputMethod == OutputMethods.Binary) ? m_OutputSequence.BinaryOutputExactLength : m_OutputSequence.HexOutput;

                RedrawUcLinesControl();
            }
            catch (Exception ex)
            {
                ShowNotification(string.Format("Error: {0}", ex.Message));
                return;
            }  // of try/catch

        }  // of CalculateAndDisplayOutput()

        public void RedrawUcLinesControl()
        {
            ucCanvasControl.ClearDrawing();
            ucCanvasControl.ListData = m_OutputSequence.BinaryList;
            ucCanvasControl.DrawLinesAndText();

        
        }  // of RedrawUcLinesControl()

        private void rbtnWiegand26x_Checked_1(object sender, RoutedEventArgs e)
        {
            ProtocolChanged(ProtocolTypes.Weigand26);

        }  // of rbtnWiegand26x_Checked_1()

        private void rbtnWiegand35x_Checked_1(object sender, RoutedEventArgs e)
        {
            ProtocolChanged(ProtocolTypes.Weigand35);

        }  // of rbtnWiegand35x_Checked_1()

        private void ProtocolChanged(ProtocolTypes p_Protocol)
        {
            m_Protocol = p_Protocol;

            if (!m_PageHasBeenLoaded)
                return;

            // clear the output string
            ClearSequenceOutputObject();

            //recalculate


            // show new output string


        }  // of ProtocolChanged()

        private void rbtnBinary_Checked_1(object sender, RoutedEventArgs e)
        {
            OutputMethodChanged(OutputMethods.Binary);

        }  // of rbtnBinary_Checked_1()

        private void rbtnHexadecimal_Checked_1(object sender, RoutedEventArgs e)
        {
            OutputMethodChanged(OutputMethods.Hexadecimal);

        }  // of rbtnHexadecimal_Checked_1()

        private void OutputMethodChanged(OutputMethods p_OutputMethod)
        {
            m_OutputMethod = p_OutputMethod;

            if (!m_PageHasBeenLoaded)
                return;

            // clear the output string
            ClearSequenceOutputObject();

            // show new output string
            CalculateAndDisplayOutput();

        }  // of OutputMethodChanged()

        private void tbxFacilityCode_GotFocus_1(object sender, RoutedEventArgs e)
        {
            SelectedTextBoxChanged(SelectedTextboxes.FacilityCode);

        }  // of tbxFacilityCode_GotFocus_1()

        private void tbxBadgeNumber_GotFocus_1(object sender, RoutedEventArgs e)
        {
            SelectedTextBoxChanged(SelectedTextboxes.BadgeID);

        }  // of tbxBadgeNumber_GotFocus_1()

        public void ShowNotification(string p_Message)
        {
            this.tbkNotification.Text = p_Message;
            tbkNotification.Visibility = Windows.UI.Xaml.Visibility.Visible;
        
        }  // of ShowNotification()

        public void HideNotification()
        {
            this.tbkNotification.Text = "";
            tbkNotification.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }  // of HideNotification()

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(splitpageHelp));
        }  // of Button_Click()

    }  // of class pHome

}  // of namespace winappWiegandCalculator

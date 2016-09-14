using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;

using System.Windows.Media;
using wp8WiegandCalculator.Resources;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Marketplace;
using WiegandLibWP8;

namespace wp8WiegandCalculator
{
    public enum SelectedTexblocks { None, FacilityCode, BadgeID, Output }
    public enum OutputMethods { None, Binary, Hexadecimal }

    public partial class MainPage : PhoneApplicationPage
    {
        private ProtocolTypes m_Protocol;
        private SelectedTexblocks m_SelectedTexblock;
        private OutputMethods m_OutputMethod;

        private OutputSequenceFactory m_OutputSequenceFactory;
        private OutputSequence m_OutputSequence;

        private bool m_PageHasBeenLoaded;

        //This is a variable for the help popup.
        private Popup m_NotificationPopup = new Popup();

        private LicenseInformation m_LicenseInformation;
        private bool? m_IsTrialMode;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            m_PageHasBeenLoaded = false;

            if (!m_IsTrialMode.HasValue)
            {
                LicenseInformation m_LicenseInformation = new LicenseInformation();
                m_IsTrialMode = m_LicenseInformation.IsTrial();

            }  // of if

        }  // of MainPage()

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            SelectedTextBlockChanged(SelectedTexblocks.None);

            m_OutputSequenceFactory = new OutputSequenceFactory();
            m_OutputSequence = null;

            m_PageHasBeenLoaded = true;

            // to force the page (landscape) orientation to the popup
            if (!LayoutRoot.Children.Contains(m_NotificationPopup))
                LayoutRoot.Children.Add(m_NotificationPopup);

            if (m_IsTrialMode.Value)
            {
                tbkAppTitle.Text = "WIEGAND CALCULATOR - TRIAL";
            }  // of if

        }  // of PhoneApplicationPage_Loaded_1()

        private void stackPanelFacilityCode_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SelectedTextBlockChanged(SelectedTexblocks.FacilityCode);
            
        }  // of stackPanelFacilityCode_Tap()

        private void stackPanelBadgeNumber_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SelectedTextBlockChanged(SelectedTexblocks.BadgeID);

        }  // of stackPanelBadgeNumber_Tap()

        private void SelectedTextBlockChanged(SelectedTexblocks p_SelectedTexblock)
        {
            m_SelectedTexblock = p_SelectedTexblock;

            switch(m_SelectedTexblock)
            {
                case SelectedTexblocks.None:
                    {
                        if (tbkFacilityCode.Text.Trim().Length == 0)
                            FillTextBlockWithDimmedText(tbkFacilityCode, "tap here");
                        if (tbkBadgeNumber.Text.Trim().Length == 0)
                            FillTextBlockWithDimmedText(tbkBadgeNumber, "tap here");
                    }
                    break;

                case SelectedTexblocks.FacilityCode:
                    if (tbkFacilityCode.Text.Equals("tap here"))
                        tbkFacilityCode.Text = "";
                    break;

                case SelectedTexblocks.BadgeID:
                    if (tbkBadgeNumber.Text.Equals("tap here"))
                        tbkBadgeNumber.Text = "";
                    break;
            
                default:
                    throw new Exception("It shouldn't get here");
            
            }  // of switch
        
        }  // of SelectedTextBlockChanged()

        private void FillTextBlockWithDimmedText(TextBlock p_TextBlock, string p_Text)
        {
            p_TextBlock.Text = p_Text;
            p_TextBlock.FontStyle = FontStyles.Italic;
            SolidColorBrush l_SolidColorBrush = new SolidColorBrush(Colors.LightGray);
            p_TextBlock.Foreground = l_SolidColorBrush;
        
        }  // of FillTextBlockWithDimmedText()

        public TextBlock GetSelectedTextBlockObject()
        {
            if (m_SelectedTexblock == SelectedTexblocks.None || m_SelectedTexblock == SelectedTexblocks.Output)
                return null;

            TextBlock l_Selected = (m_SelectedTexblock == SelectedTexblocks.FacilityCode) ? tbkFacilityCode : tbkBadgeNumber;

            return l_Selected;
        
        }  // of GetSelectedTextBlockObject()

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            ClearSequenceOutputObject();

            TextBlock l_Selected = GetSelectedTextBlockObject();

            if (l_Selected == null)
                return;

            Button b = (Button)sender;

            l_Selected.Text = string.Format("{0}{1}", l_Selected.Text,b.Content.ToString());

        }  // of Number_Click()

        private void ClearSequenceOutputObject()
        {

            tbkOutputSequence.Text = "";
        
        }  // of ClearSequenceOutputObject()

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            ClearSequenceOutputObject();

            TextBlock l_Selected = GetSelectedTextBlockObject();

            if (l_Selected == null)
                return;

            l_Selected.Text = "";

        }  // of Del_Click()

        private void R_Click(object sender, RoutedEventArgs e)
        {
            ClearSequenceOutputObject();

            TextBlock l_Selected = GetSelectedTextBlockObject();

            if (l_Selected == null)
                return;

            if (l_Selected.Text.Length > 0)
            {
                l_Selected.Text = l_Selected.Text.Substring(0, l_Selected.Text.Length - 1);
            }

        }  // of R_Click()

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            CalculateAndDisplayOutput();

        }  // of Result_Click()

        private void CalculateAndDisplayOutput()
        {
            int l_FacilityCode;
            int l_BadgeID;
            SimpleCard l_Card;

            if (!Int32.TryParse(tbkFacilityCode.Text, out l_FacilityCode))
            {
                DisplayNotificationPopup("\n Facility code should be a number.\n");
                return;
            }

            if (!Int32.TryParse(tbkBadgeNumber.Text, out l_BadgeID))
            {
                DisplayNotificationPopup("\n Card Number should be a number. \n");
                return;
            }

            try
            {
                l_Card = new SimpleCard(l_BadgeID, l_FacilityCode);

                m_OutputSequence = m_OutputSequenceFactory.GetOutputSequenceObject(l_Card, m_Protocol);

                tbkOutputSequence.Text = (m_OutputMethod == OutputMethods.Binary) ? m_OutputSequence.BinaryOutputExactLength : m_OutputSequence.HexOutput;            
            }
            catch(Exception ex)
            {
                DisplayNotificationPopup(string.Format("Error: {0}",ex.Message));
                return;        
            }  // of try/catch
        
        }  // of CalculateAndDisplayOutput()

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
            tbkOutputSequence.Text = "";
            
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
            tbkOutputSequence.Text = "";

            // show new output string
            CalculateAndDisplayOutput();

        }  // of OutputMethodChanged()

        private void SaveButton_Click(object sender, EventArgs e)
        {


        }  //  of SaveButton_Click()

        /// <summary>
        /// Click event handler for the help button.
        ///This will create a popup/message box for help and add content to the popup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayNotificationPopup(string p_Text)
        {
            String l_Text = p_Text;

            //Stack panel with a black background.
            StackPanel panelHelp = new StackPanel();
            panelHelp.Background = new SolidColorBrush(Colors.Black);
            panelHelp.Width  = 400;
            panelHelp.Height = 200;
            panelHelp.Orientation = System.Windows.Controls.Orientation.Vertical;

            //Create a white border.
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.White);
            border.BorderThickness = new Thickness(7.0);

            //Create a close button to exit popup.
            Button close = new Button();
            close.Content = "Close";
            close.Width = 120;
            close.Margin = new Thickness(5.0);
            close.Click += new RoutedEventHandler(close_Click);
            close.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

            //Create helper text
            TextBlock textblockHelp = new TextBlock();
            textblockHelp.FontSize = 24;
            textblockHelp.Foreground = new SolidColorBrush(Colors.White);
            textblockHelp.TextWrapping = TextWrapping.Wrap;
            textblockHelp.Text = l_Text;
            textblockHelp.Margin = new Thickness(5.0);
            textblockHelp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            //Add controls to stack panel
            panelHelp.Children.Add(textblockHelp);
            panelHelp.Children.Add(close);
            border.Child = panelHelp;

            // Set the Child property of Popup to the border 
            // that contains a stackpanel, textblock and button.
            m_NotificationPopup.Child = border;

            // Set where the popup will show up on the screen.   
            m_NotificationPopup.VerticalOffset = 100;
            m_NotificationPopup.HorizontalOffset = 85;

            // Open the popup.
            m_NotificationPopup.IsOpen = true;

        }  // of DisplayNotificationPopup()

        /// <summary>
        /// Click event handler for the close button on the help popup.
        /// Closes the poupup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void close_Click(object sender, RoutedEventArgs e)
        {
            m_NotificationPopup.IsOpen = false;

        }  // of close_Click()

        private void HelpButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/PageHelp.xaml?Protocol={0}&SelectedTexblock={1}&OutputMethod={2}", m_Protocol, m_SelectedTexblock, m_OutputMethod), UriKind.Relative));
        }  // of HelpButton_Click()

        private void EmailBarIconButton_Click(object sender, EventArgs e)
        {
            if (m_OutputSequence == null)
            {
                DisplayNotificationPopup("\n You need to calculate the Wiegand output before sending it in email.");
                return;
            } // of if

            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = string.Format("{0} output for card with badge id {1} and facility code {2}", m_Protocol, m_OutputSequence.Card.BadgeNumber, m_OutputSequence.Card.FacilityCode);

            if (m_IsTrialMode.Value)
            {
                emailComposeTask.Body = string.Format("Binary: {0} \r\n Hexadecimal: {1}", "not available in trial mode of the app", "not available in trial mode of the app");
            }
            else
            {
                emailComposeTask.Body = string.Format("Binary: {0} \r\n Hexadecimal: {1}", m_OutputSequence.BinaryOutputExactLength, m_OutputSequence.HexOutput);
            }  // of if/else
            //emailComposeTask.To = "recipient@example.com";
            //emailComposeTask.Cc = "cc@example.com";
            //emailComposeTask.Bcc = "bcc@example.com";

            emailComposeTask.Show();

        }  // of EmailBarIconButton_Click()

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        /*
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            tb.Text += b.Content.ToString();
        }

        private void Result_click(object sender, RoutedEventArgs e)
        {
            try
            {
                result();
            }
            catch (Exception exc)
            {
                tb.Text = "Error!";
            }
        }

        private void result()
        {
            String op;
            int iOp = 0;
            if (tb.Text.Contains("+"))
            {
                iOp = tb.Text.IndexOf("+");
            }
            else if (tb.Text.Contains("-"))
            {
                iOp = tb.Text.IndexOf("-");
            }
            else if (tb.Text.Contains("*"))
            {
                iOp = tb.Text.IndexOf("*");
            }
            else if (tb.Text.Contains("/"))
            {
                iOp = tb.Text.IndexOf("/");
            }
            else
            {
                //error
            }

            op = tb.Text.Substring(iOp, 1);
            double op1 = Convert.ToDouble(tb.Text.Substring(0, iOp));
            double op2 = Convert.ToDouble(tb.Text.Substring(iOp + 1, tb.Text.Length - iOp - 1));

            if (op == "+")
            {
                tb.Text += "=" + (op1 + op2);
            }
            else if (op == "-")
            {
                tb.Text += "=" + (op1 - op2);
            }
            else if (op == "*")
            {
                tb.Text += "=" + (op1 * op2);
            }
            else
            {
                tb.Text += "=" + (op1 / op2);
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            tb.Text = "";
        }

        private void R_Click(object sender, RoutedEventArgs e)
        {
            if (tb.Text.Length > 0)
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
            }
        }
        */

    }  // of class MainPage

}  // of namespace wp8WiegandCalculator
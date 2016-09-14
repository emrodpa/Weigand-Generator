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
using WeigandLib;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace winappTestWiegand
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

        }  // of MainPage()

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SimpleCard l_Card = new SimpleCard(961, 178);
            OutputSequence l_OutputSequence = new OutputSequence(l_Card, ProtocolTypes.Weigand35);

            string l_BinaryOutput = l_OutputSequence.BinaryOutput;

            string l_HexOutput = l_OutputSequence.HexOutput;


        }  // of OnNavigatedTo()

    }  // of class MainPage

}  // of namespace winappTestWiegand

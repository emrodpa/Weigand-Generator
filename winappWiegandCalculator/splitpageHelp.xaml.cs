using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using WeigandLib;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace winappWiegandCalculator
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class splitpageHelp : winappWiegandCalculator.Common.LayoutAwarePage
    {
        public FeedData DataFeed;

        public splitpageHelp()
        {
            this.InitializeComponent();

            string l_Explanation = "the binary output shows the \'1\' and \'0\' as it comes out of the card reader.\n Data0 lows are presented as zeroes and Data1 lows are presented as ones.\n";
                

            List<Data> l_DataList = new List<Data>(2)
            {
                  new Data(ProtocolTypes.Weigand35
                      ,new Uri("/Help/hid35.png", UriKind.Relative)
                      ,l_Explanation + string.Format("The hexadecimal output shows the bytes in hexadecimal after adding a certain number of trailing zeros to the binary output.\n example: for the Wiegand 35 bits format, five trailing zeroes are needed in order to form five bytes.\n")
                      )
                , new Data(ProtocolTypes.Weigand26
                    ,new Uri("/Help/wiegand26.png", UriKind.Relative)
                    ,l_Explanation + string.Format("The hexadecimal output shows the bytes in hexadecimal after adding a certain number of trailing zeros to the binary output.\n example: for the Wiegand 26 bits format, six trailing zeroes are needed in order to form four bytes.\n")
                    )
            };

            DataFeed = new FeedData("Protocols","help describing the output signal formats for a Wiegand card reader", l_DataList);

        }  // of splitpageHelp()

        #region Page state management

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
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]

            if (DataFeed != null)
            {
                this.DefaultViewModel["Group"] = DataFeed;
                this.DefaultViewModel["Items"] = DataFeed.Items;
            }


            if (pageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    // TODO: Invoke this.itemsViewSource.View.MoveCurrentTo() with the selected
                    //       item as specified by the value of pageState["SelectedItem"]
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = this.itemsViewSource.View.CurrentItem;
                // TODO: Derive a serializable navigation parameter and assign it to
                //       pageState["SelectedItem"]
            }
        }

        #endregion

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed, or null
        /// for the current view state.  This parameter is optional with null as the default
        /// value.</param>
        /// <returns>True when the view state in question is portrait or snapped, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is Snapped)
        /// displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return to
                // the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // When logical page navigation is not in effect, or when there is no selected
                // item, use the default back button behavior.
                base.GoBack(sender, e);
            }
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion

    }  // of class splitpageHelp

    public class Data
    {
        public ProtocolTypes ProtocolType { get; set; }
        public string Title
        {
            get
            {
                string l_Title = "";
                switch (ProtocolType)
                {
                    case ProtocolTypes.Weigand26:
                        l_Title = "Wiegand 26 Bits";
                        break;

                    case ProtocolTypes.Weigand35:
                        l_Title = "Wiegand 35 Bits Corporate 1000";
                        break;

                    default:
                        l_Title = "unknown protocol";
                        break;

                }  // of switch

                return l_Title;
            }
        }

        public Uri ImageFilepath { get; set; }
        public string Content { get; set; }

        public string imagepath
        {
            get
            {
                return ImageFilepath.ToString(); 
            }
        }

        public Data()
        { }  // of Data()

        public Data(ProtocolTypes p_ProtocolType, Uri p_ImageFilepath, string p_Content)
            : this()
        {
            ProtocolType = p_ProtocolType;
            ImageFilepath = p_ImageFilepath;
            Content = p_Content;
        }  // of Data()

    }  // of class Data

    public class FeedData
    {
        public string Title { get; set; }
        public string Description { get; set; }

        private List<Data> _Items = new List<Data>();
        public List<Data> Items
        {
            get
            {
                return this._Items;
            }
        }

        public FeedData(string p_Title, string p_Description, List<Data> p_Items)
        {
            Title = p_Title;
            Description = p_Description;
            _Items = p_Items;

        }  // of FeedData()

    }  // of class FeedData

}  // of namespace winappWiegandCalculator

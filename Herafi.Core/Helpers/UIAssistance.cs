using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Herafi.Core.Helpers
{
    public class UIAssistance
    {
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }

    public class TranslatingUI
    {
        private static ResourceContext _resourceContextObj = new ResourceContext();


        public static ResourceContext ResourceContextObj { get => _resourceContextObj; }
        public static ResourceMap SettingsResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("SettingsStrings"); }
        public static ResourceMap SignInResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("SignInStrings"); }
        public static ResourceMap MainResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("MainStrings"); }
        public static ResourceMap UsersResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("UsersStrings"); }
        public static ResourceMap CraftmenResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("CraftmenStrings"); }
        public static ResourceMap AnalyzesResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("AnalyzesStrings"); }
        public static ResourceMap DashboardResourceMap { get => ResourceManager.Current.MainResourceMap.GetSubtree("DashboardStrings"); }

    }

    public class NavigationHelper
    {
        public static Frame ContentFrame { get; set; }

    }
}

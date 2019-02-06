using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MediaPlayer.UI
{
    public class GridViewSort
    {
        public static ICommand GetCommand(DependencyObject obj) => (ICommand)obj.GetValue(CommandProperty);

        public static void SetCommand(DependencyObject obj, ICommand value) => obj.SetValue(CommandProperty, value);

        public static string GetPropertyName(DependencyObject obj) => (string)obj.GetValue(PropertyNameProperty);

        public static void SetPropertyName(DependencyObject obj, string value) => obj.SetValue(PropertyNameProperty, value);

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(GridViewSort), new UIPropertyMetadata(null));

        public static bool GetAutoSort(DependencyObject obj) => (bool)obj.GetValue(AutoSortProperty);

        public static void SetAutoSort(DependencyObject obj, bool value) => obj.SetValue(AutoSortProperty, value);

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(GridViewSort), 
            new UIPropertyMetadata( null, (o, e) =>
            {
                if (o is ItemsControl listView)
                {
                    if (!GetAutoSort(listView))
                    {
                        if (e.OldValue != null && e.NewValue == null)
                        {
                            listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                        if (e.OldValue == null && e.NewValue != null)
                        {
                            listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                    }
                }
            }));

        public static readonly DependencyProperty AutoSortProperty = DependencyProperty.RegisterAttached("AutoSort", typeof(bool), typeof(GridViewSort),
            new UIPropertyMetadata( false, (o, e) =>
            {
                if (o is ListView listView)
                {
                    if (GetCommand(listView) == null)
                    {
                        bool oldValue = (bool)e.OldValue;
                        bool newValue = (bool)e.NewValue;
                        if (oldValue && !newValue)
                        {
                            listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                        if (!oldValue && newValue)
                        {
                            listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                    }
                }
            }));

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName);
                        }
                    }
                }
            }
        }

        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

        public static void ApplySort(ICollectionView view, string propertyName)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }
    }
}

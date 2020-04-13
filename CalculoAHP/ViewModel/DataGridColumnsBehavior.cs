using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CalculoAHP.ViewModel
{
    public class DataGridColumnsBehavior
    {
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.RegisterAttached("BindableColumns",
                                                typeof(ObservableCollection<DataGridColumn>),
                                                typeof(DataGridColumnsBehavior),
                                                new UIPropertyMetadata(null, BindableColumnsPropertyChanged));
        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = source as DataGrid;
            dataGrid.Columns.Clear();

            if (!(e.NewValue is ObservableCollection<DataGridColumn> columns))
                return;
           
            foreach (DataGridColumn column in columns)
            {
                var dataGridOwnerProperty = column.GetType().GetProperty("DataGridOwner", BindingFlags.Instance | BindingFlags.NonPublic);

                if (dataGridOwnerProperty != null)
                    dataGridOwnerProperty.SetValue(column, null);
                
                dataGrid.Columns.Add(column);
            }

            void handler(object sender, NotifyCollectionChangedEventArgs e2)
            {
                columns.CollectionChanged -= handler;

                NotifyCollectionChangedEventArgs ne = e2 as NotifyCollectionChangedEventArgs;

                if (ne.Action == NotifyCollectionChangedAction.Reset)
                {
                    dataGrid.Columns.Clear();

                    if (ne.NewItems == null)
                        return;

                    foreach (DataGridColumn column in ne.NewItems)
                        dataGrid.Columns.Add(column);
                }

                else if (ne.Action == NotifyCollectionChangedAction.Add)
                {
                    if (ne.NewItems == null)
                        return;

                    foreach (DataGridColumn column in ne.NewItems)
                        dataGrid.Columns.Add(column);
                }

                else if (ne.Action == NotifyCollectionChangedAction.Move)
                    dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);

                else if (ne.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (ne.OldItems == null)
                        return;

                    foreach (DataGridColumn column in ne.OldItems)
                        dataGrid.Columns.Remove(column);
                }

                else if (ne.Action == NotifyCollectionChangedAction.Replace)
                    dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;
            }

            columns.CollectionChanged += handler;
        }

        public static void SetBindableColumns(DependencyObject element, ObservableCollection<DataGridColumn> value)
        {
            element.SetValue(BindableColumnsProperty, value);
        }

        public static ObservableCollection<DataGridColumn> GetBindableColumns(DependencyObject element)
        {
            return (ObservableCollection<DataGridColumn>)element.GetValue(BindableColumnsProperty);
        }
    }
}

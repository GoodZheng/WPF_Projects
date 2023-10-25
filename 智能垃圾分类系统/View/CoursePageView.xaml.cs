using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 智能垃圾分类系统.Model;
using 智能垃圾分类系统.ViewModel;

namespace 智能垃圾分类系统.View
{
    /// <summary>
    /// CoursePageView.xaml 的交互逻辑
    /// </summary>
    public partial class CoursePageView : UserControl
    {
        public CoursePageView()
        {
            InitializeComponent();
            this.DataContext = new CoursePageViewModel();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            string teacher = button.Content.ToString();


            ICollectionView view = CollectionViewSource.GetDefaultView(this.icCourses.ItemsSource);
            if (teacher == "全部")
            {
                view.Filter = null;

                //排序
                //view.SortDescriptions.Add(new SortDescription("CourseName", ListSortDirection.Descending));
            }
            else
            {
                view.Filter = new Predicate<object>((o) =>
                {
                    return (o as CourseModel).Teachers.Exists(t => t == teacher);
                });
            }
        }
    }
}

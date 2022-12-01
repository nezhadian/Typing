using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Typing.Pages
{
    /// <summary>
    /// Interaction logic for FreeModePage.xaml
    /// </summary>
    public partial class FreeModePage : Page
    {
        public FreeModePage()
        {
            InitializeComponent();
            
        }

        private void PausePlayCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = sender is FreeModePage;
        }

        private void PausePlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }
    }
    public static class GameCommands
    {
        public static readonly RoutedUICommand PausePlayCommand = new RoutedUICommand
            (
                "PausePlayCommand",
                "PausePlayCommand",
                typeof(GameCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape)
                }
            );
    }
}

namespace ArtMoreWPF
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class CustomCommands
    {
        public static readonly RoutedUICommand BeginTimer = new RoutedUICommand
        (
            "Begin Timer",
            "BeginTimer",
            typeof(CustomCommands)
        );

        public static readonly RoutedUICommand EndTimer = new RoutedUICommand
        (
            "Stop Timer",
            "StopTimer",
            typeof(CustomCommands)
        );
    }
}

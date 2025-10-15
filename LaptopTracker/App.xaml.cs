using LaptopTracker.Database;
using System.Windows;

namespace LaptopTracker
{
    public partial class App : Application
    {
        public static LITEntities entities = new LITEntities();
    }
}
